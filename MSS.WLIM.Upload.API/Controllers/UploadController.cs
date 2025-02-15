﻿using Microsoft.AspNetCore.Mvc;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using MSS.WLIM.Upload.API.Services;
using MSS.WLIM.DataServices.Data;
using MSS.WLIM.DataServices.Models;

namespace MSS.WLIM.Upload.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWareHouseItemService _warehouseItemService;
        private readonly DataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadController(IWareHouseItemService wareHouseItemService, DataBaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _warehouseItemService = wareHouseItemService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] WareHouseItemViewModel item)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, item.Id + "_" + file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (item == null)
            {
                return BadRequest("Item data is required.");
            }

            try
            {
                var maxSequenceNumber = _context.WareHouseItems
                                     .Where(x => x.CreatedDate.Date == DateTime.Today)
                                     .Max(x => (int?)x.QRSequenceNumber) ?? 0;

                var SessionUsername = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;
                var qrGeneratedAt = DateTime.Now;
                var qrSequenceNumber = maxSequenceNumber + 1;

                var warehouseItem = new WareHouseItem()
                {
                    Id = item.Id,
                    Category = item.Category == "null" || string.IsNullOrEmpty(item.Category) ? "Unknown" : item.Category,
                    CreatedBy = SessionUsername,
                    CreatedDate = DateTime.Now,
                    FilePath = item.Id + "_" + file.FileName,
                    WarehouseLocation = item.WarehouseLocation,
                    Status = "Photo Captured",
                    Tags = String.Join(",", item.Tags),
                    ItemDescription = item.ItemDescription,
                    Comments = item.Comments,
                    IdentifiedLocation = item.IdentifiedLocation,
                    IdentifiedDate = item.IdentifiedDate,
                    QRSequenceNumber = qrSequenceNumber,
                    QRGeneratedAt = qrGeneratedAt,
                    QRCodeContent = item.Id + "-" + qrGeneratedAt.ToString("MM-dd-yyyy") + "-" + qrSequenceNumber.ToString(),
                    Donated = false
                };

                await _context.WareHouseItems.AddAsync(warehouseItem);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    FilePath = filePath,
                    ItemId = warehouseItem.Id,
                    QRGeneratedAt = warehouseItem.QRGeneratedAt,
                    QRSequenceNumber = warehouseItem.QRSequenceNumber,
                    QRCodeContent = warehouseItem.QRCodeContent,
                    Message = "File uploaded successfully."
                });
            }
            catch (Exception ex)
            {
                // Log the exception if necessary and return an error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }            
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(IFormFile file, [FromForm] WareHouseItemViewModel item)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "search");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            // Load the uploaded image
            var uploadedImage = new Image<Bgr, byte>(filePath);
            var uploadedImageGray = uploadedImage.Convert<Gray, byte>();

            // Initialize ORB detector
            //var orb = new ORBDetector(500);
            ORB orb = new ORB(500);
            var uploadedKeypoints = new VectorOfKeyPoint();
            var uploadedDescriptors = new Mat();

            Mat descriptors1 = new Mat();
            Mat descriptors2 = new Mat();

            // Detect keypoints and compute descriptors for the uploaded image
            orb.DetectAndCompute(uploadedImageGray, null, uploadedKeypoints, uploadedDescriptors, false);

            List<string> filesMatched = new List<string>();

            //Get list of uploaded items based on matched category from the database

            var wareHouseItems = _context.WareHouseItems.Where(x => (x.Status != "Claimed" && x.Category != null && item.Category != null && x.Category.Contains(item.Category)) || (x.ItemDescription != null && item.ItemDescription != null && x.ItemDescription.Contains(item.ItemDescription)))?.ToList();
            wareHouseItems ??= new List<WareHouseItem>();

            // Split the incoming tags from React input into a list
            var inputTags = item.Tags?.Split(',').Select(t => t.Trim()).ToList() ?? new List<string>();


            // Retrieve items where Tags is not null
            var itemsWithTags = _context.WareHouseItems
                                .Where(x => x.Tags != null && x.Status != "Claimed")
                                .ToList();

            foreach (var tag in inputTags) // Loop through each tag from the React input
            {
                // Filter items in memory to check if any split tag in the database contains the input tag
                var tagItems = itemsWithTags
                                .Where(x => x.Tags
                                             .Split(',')
                                             .Any(dbTag => dbTag.Trim().Contains(tag, StringComparison.OrdinalIgnoreCase))
                                             && x.Status != "Claimed")
                                .ToList();

                foreach (var tagItem in tagItems)
                {
                    if (!wareHouseItems.Any(x => x.Id == tagItem.Id)) // Add if not already in list
                    {
                        wareHouseItems.Add(tagItem);
                    }
                }
            }
            return Ok(new { FilesMatched = wareHouseItems, Message = filesMatched.Count + " items found" });
        }

        [HttpGet("images/{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine("uploads", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                var image = System.IO.File.OpenRead(imagePath);
                return File(image, "image/jpeg");
            }
            return NotFound();
        }

        [HttpGet("images/search/{tag}")]
        public async Task<IActionResult> SearchImages(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                var wareHouseItems = _context.WareHouseItems.Where(x => ((x.Status != "Claimed") && (x.Status != "Returned")) && ( (x.Category != null && x.Category.Contains(tag)) || (x.ItemDescription != null && x.ItemDescription.Contains(tag))))?.ToList();
                return Ok(wareHouseItems);
            }
            return NotFound();
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetAllImages()
        {

            var wareHouseItemsimages = _context.WareHouseItems.AsParallel<WareHouseItem>().ToList();
            return Ok(wareHouseItemsimages);
        }

        [HttpGet("getAll")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<WareHouseItem>>> GetAll()
        {
            var data = await _warehouseItemService.GetAll();

            return Ok(data);
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult<WareHouseItem>> GetById(string id)
        {
            var data = await _warehouseItemService.GetById(id);
            
            if (data == null)
            {
                return NotFound(new { message = $"Warehouse item with Id = {id} not found." });
            }

            return Ok(data);
        }

        [HttpPatch("update-donated/{id}")]
        public async Task<IActionResult> UpdateDonatedStatus(string id, [FromBody] bool donated)
        {
            try
            {
                var wareHouseItem = await _context.WareHouseItems.FindAsync(id);
                if (wareHouseItem == null)
                {
                    return NotFound("Item not found.");
                }

                wareHouseItem.Donated = donated;

                _context.WareHouseItems.Update(wareHouseItem);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Donated status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Upload/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] WareHouseItem updateWareHouseItem)
        {

            try
            {
                await _warehouseItemService.Update(updateWareHouseItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(updateWareHouseItem);
        }

    }
}

