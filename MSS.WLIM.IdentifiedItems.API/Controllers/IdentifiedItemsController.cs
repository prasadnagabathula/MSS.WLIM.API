using Microsoft.AspNetCore.Mvc;
using MSS.WLIM.DataServices.Models;
using MSS.WLIM.IdentifiedItem.API.Services;
using MSS.WLIM.DataServices.Data;



namespace MSS.WLIM.IdentifiedItem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentifiedItemController : ControllerBase
    {
        private readonly IIdentifiedItemServices _IdentifiedServices;
        private readonly DataBaseContext _context;

        private readonly ILogger<IdentifiedItemController> _logger;


        public IdentifiedItemController(IIdentifiedItemServices service, ILogger<IdentifiedItemController> logger)
        {
            _IdentifiedServices = service;
            _logger = logger;
        }

        // GET: api/<IdentifiedItemsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentifiedItems>>> GetAll()
        {
            var IdentifiedItems = await _IdentifiedServices.GetAll();

            return Ok(IdentifiedItems);
        }

        // GET api/<IdentifiedItemsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<IdentifiedItemsController>
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file, [FromForm] IdentifiedItems item)
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

                var IdentifiedItems = new IdentifiedItems()
                {
                    Id = item.Id,
                    ItemDescription = item.ItemDescription,
                    BrandMake = item.BrandMake,
                    ModelVersion = item.ModelVersion,
                    Color = item.Color,
                    SerialNumber = item.SerialNumber,
                    DistinguishingFeatures = item.DistinguishingFeatures,
                    Condition = item.Condition,
                    IdentifiedDate = item.IdentifiedDate,
                    IdentifiedLocation = item.IdentifiedLocation,
                    IsActive = item.IsActive,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    Photos = item.Id + "_" + file.FileName,
                    Category = item.Category,
                    Tags = String.Join(",", item.Tags),
                    Itemobject = String.Join(",", item.Itemobject)

                };

                // Add the new item to the context
                await _context.WHTblIdentifiedItems.AddAsync(IdentifiedItems);

                // Save the changes to the database
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log the exception if necessary and return an error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }



            return Ok(new { FilePath = filePath, Message = "File uploaded successfully." });


        }

        // PUT api/<IdentifiedItemsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IdentifiedItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
