using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MSS.WLIM.DataServices.Data;
using MSS.WLIM.DataServices.Models;
using MSS.WLIM.DataServices.Repositories;
using Serilog;
using System.Drawing;
using System.Linq;

namespace MSS.WLIM.LostItemRequest.API.Services
{
    public class LostItemRequestService : ILostItemRequestService
    {
        private readonly IRepository<LostItemRequests> _repository;
        private readonly DataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LostItemRequestService(IRepository<LostItemRequests> repository, DataBaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<LostItemRequests>> GetAll()
        {
            //return await _context.WHTblLostItemRequest.ToListAsync();
            var lostItemRequests = await _context.WHTblLostItemRequest
                .Include(t => t.WareHouseItem)
                .ToListAsync();

            var LostItemRequestsDto = new List<LostItemRequests>();

            foreach (var d in lostItemRequests)
            {
                Console.WriteLine(d.WareHouseItem.ReceivedBy+"  "+d.WareHouseItem.ReceivedOn);
                LostItemRequestsDto.Add(new LostItemRequests
                {
                    Id = d.Id,
                    Description = d.Description,
                    Color = d.Color,
                    Size = d.Size,
                    Brand = d.Brand,
                    Model = d.Model,
                    DistinguishingFeatures = d.DistinguishingFeatures,
                    ItemCategory = d.ItemCategory,
                    SerialNumber = d.SerialNumber,
                    DateTimeWhenLost = d.DateTimeWhenLost,
                    Location = d.Location,
                    ItemValue = d.ItemValue,
                    ItemPhoto = d.WareHouseItem?.FilePath,
                    ProofofOwnership = d.WareHouseItem?.QRCodeContent,
                    HowtheItemLost = d.HowtheItemLost,
                    ReferenceNumber = d.ReferenceNumber,
                    AdditionalInformation = d.AdditionalInformation,
                    Address = d.Address,
                    OtherRelevantDetails = d.OtherRelevantDetails,
                    IsActive = d.IsActive,
                    CreatedBy = d.CreatedBy,
                    CreatedDate = d.CreatedDate,
                    UpdatedBy = d.UpdatedBy,
                    UpdatedDate = d.UpdatedDate,
                    Status = d.Status,
                    WareHouseItem = new WareHouseItem
                    {
                        ReceivedBy = d.WareHouseItem.ReceivedBy,
                        ReceivedOn = d.WareHouseItem.ReceivedOn
                    },
                });
            }

            return LostItemRequestsDto;
        }

        public async Task<LostItemRequests> Get(string id)
        {
            var lostItemRequest = await _context.WHTblLostItemRequest
                .FirstOrDefaultAsync(t => t.Id == id);

            if (lostItemRequest == null)
                return null;

            return new LostItemRequests
            {
                Id = lostItemRequest.Id,
                Description = lostItemRequest.Description,
                Color = lostItemRequest.Color,
                Size = lostItemRequest.Size,
                Brand = lostItemRequest.Brand,
                Model = lostItemRequest.Model,
                DistinguishingFeatures = lostItemRequest.DistinguishingFeatures,
                ItemCategory = lostItemRequest.ItemCategory,
                SerialNumber = lostItemRequest.SerialNumber,
                DateTimeWhenLost = lostItemRequest.DateTimeWhenLost,
                Location = lostItemRequest.Location,
                ItemValue = lostItemRequest.ItemValue,
                ItemPhoto = lostItemRequest.ItemPhoto,
                ProofofOwnership = lostItemRequest.ProofofOwnership,
                HowtheItemLost = lostItemRequest.HowtheItemLost,
                ReferenceNumber = lostItemRequest.ReferenceNumber,
                AdditionalInformation = lostItemRequest.AdditionalInformation,
                Address = lostItemRequest.Address,
                OtherRelevantDetails = lostItemRequest.OtherRelevantDetails,
                IsActive = lostItemRequest.IsActive,
                CreatedBy = lostItemRequest.CreatedBy,
                CreatedDate = lostItemRequest.CreatedDate,
                UpdatedBy = lostItemRequest.UpdatedBy,
                UpdatedDate = lostItemRequest.UpdatedDate,
                Status = lostItemRequest.Status
            };
        }

        public async Task<LostItemRequests> Add(LostItemRequests _object)
        {
            var employeeName = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;
            var lostItemRequest = new LostItemRequests
            {
                Description = _object.Description,
                Color = _object.Color,
                Size = _object.Size,
                Brand = _object.Brand,
                Model = _object.Model,
                DistinguishingFeatures = _object.DistinguishingFeatures,
                ItemCategory = _object.ItemCategory,
                SerialNumber = _object.SerialNumber,
                DateTimeWhenLost = _object.DateTimeWhenLost,
                Location = _object.Location,
                ItemValue = _object.ItemValue,
                ItemPhoto = _object.ItemPhoto,
                ProofofOwnership = _object.ProofofOwnership,
                HowtheItemLost = _object.HowtheItemLost,
                ReferenceNumber = _object.ReferenceNumber,
                Address = _object.Address,
                AdditionalInformation = _object.AdditionalInformation,
                OtherRelevantDetails = _object.OtherRelevantDetails,
                IsActive = _object.IsActive,
                CreatedBy = employeeName,
                CreatedDate = DateTime.Now,
                Status = _object.Status
            };

            _context.WHTblLostItemRequest.Add(lostItemRequest);
            await _context.SaveChangesAsync();

            _object.Id = lostItemRequest.Id;
            return _object;
        }

        public async Task<LostItemRequests> Claim(LostItemRequestsViewModel _object)
        {
            var employeeName = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;
            var lostItemRequest = new LostItemRequests
            {
                Description = _object.Description,
                Color = _object.Color,
                Size = _object.Size,
                Model = _object.Model,
                Brand = _object.Brand,
                DistinguishingFeatures = _object.DistinguishingFeatures,
                ItemCategory = _object.ItemCategory,
                SerialNumber = _object.SerialNumber,
                DateTimeWhenLost = _object.DateTimeWhenLost,
                Location = _object.Location,
                ItemValue = _object.ItemValue,
                ProofofOwnership = _object.ProofofOwnership,
                HowtheItemLost = _object.HowtheItemLost,
                ReferenceNumber = _object.ReferenceNumber,
                AdditionalInformation = _object.AdditionalInformation,
                Address = _object.Address,
                OtherRelevantDetails = _object.OtherRelevantDetails,
                IsActive = true,
                CreatedBy = employeeName,
                CreatedDate = DateTime.Now,
                ClaimId = _object.ClaimId,
                Status = "Claimed"
            };

            _context.WHTblLostItemRequest.Add(lostItemRequest);
            await _context.SaveChangesAsync();

            var userName = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;
            var WareHouseItem = await _context.WareHouseItems.FindAsync(_object.ClaimId);
            if (WareHouseItem == null)
                throw new KeyNotFoundException("WareHouseItem not found");

            WareHouseItem.Status = "Claimed";
            //WareHouseItem.UpdatedBy = userName;
            //WareHouseItem.UpdatedDate = DateTime.Now;

            _context.Entry(WareHouseItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return lostItemRequest;
        }


        public async Task<string> UploadPhotoAsync(LostItemRequestPhoto lostItemRequestPhoto)
        {
            string fileName = ""; // Variable to hold the file name
            try
            {
                // Check if a file is uploaded and not empty
                if (lostItemRequestPhoto.ItemPhoto != null && lostItemRequestPhoto.ItemPhoto.Length > 0)
                {
                    var file = lostItemRequestPhoto.ItemPhoto;
                    fileName = Path.GetFileName(file.FileName); // Get just the file name

                    // Define the full path where the file will be saved
                    var photoPath = Path.Combine("C:\\Users\\mshaik5\\Desktop\\LostItemPhotos", fileName);

                    // Save file to the specified path
                    using (var stream = System.IO.File.Create(photoPath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading the photo: " + ex.Message);
            }

            return fileName; // Return only the filename if needed
        }

        public async Task<LostItemRequests> Update(LostItemRequests _object)
        {

            var userName = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;
            var lostItemRequest = await _context.WHTblLostItemRequest.FindAsync(_object.Id);

            if (lostItemRequest == null)
                throw new KeyNotFoundException("lostItemRequest not found");

            lostItemRequest.Description = _object.Description;
            //lostItemRequest.Color = _object.Color;
            //lostItemRequest.Size = _object.Size;
            //lostItemRequest.Brand = _object.Brand;
            //lostItemRequest.Model = _object.Model;
            //lostItemRequest.DistinguishingFeatures = _object.DistinguishingFeatures;
            //lostItemRequest.ItemCategory = _object.ItemCategory;
            //lostItemRequest.SerialNumber = _object.SerialNumber;
            //lostItemRequest.DateTimeWhenLost = _object.DateTimeWhenLost;
            //lostItemRequest.Location = _object.Location;
            //lostItemRequest.ItemValue = _object.ItemValue;
            //lostItemRequest.ItemPhoto = _object.ItemPhoto;
            //lostItemRequest.ProofofOwnership = _object.ProofofOwnership;
            //lostItemRequest.HowtheItemLost = _object.HowtheItemLost;
            //lostItemRequest.ReferenceNumber = _object.ReferenceNumber;
            lostItemRequest.AdditionalInformation = _object.AdditionalInformation;
            //lostItemRequest.OtherRelevantDetails = _object.OtherRelevantDetails;
            lostItemRequest.IsActive = _object.IsActive;
            lostItemRequest.Status = _object.Status;
            lostItemRequest.UpdatedBy = userName;
            lostItemRequest.UpdatedDate = DateTime.Now;

            _context.Entry(lostItemRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _object;
        }

        public async Task<bool> Delete(string id)
        {
            // Check if the technology exists
            var existingData = await _repository.Get(id);
            if (existingData == null)
            {
                throw new ArgumentException($"with ID {id} not found.");
            }

            // Call repository to delete the Department
            existingData.IsActive = false; // Soft delete
            await _repository.Update(existingData); // Save changes
            return true;
        }

        public async Task<DashboardData> ClaimCount(string location)
        {
            Dictionary<string, int[]> locationData = new Dictionary<string, int[]>();
            Dictionary<string, int> categoryData = new Dictionary<string, int>();
            int ClaimRequestCount = 0;
            int PendingRequestCount = 0;
            int SuccessRequestCount = 0;
            int IdentifiedItemsCount = 0;
            if (location == "All")
            {
                var locations = await _context.WareHouseItems.Where(r => r.WarehouseLocation != null)
                                       .Select(r => r.WarehouseLocation)
                                       .Distinct()
                                       .ToListAsync();
                foreach (string loc in locations)
                {
                    var data = await _context.WareHouseItems.Where(w => w.WarehouseLocation == loc)
                        .Select(d => EF.Functions.DateDiffDay(d.CreatedDate, d.UpdatedDate ?? DateTime.Now)).ToListAsync();

                    int[] a = [0, 0, 0, 0, 0];

                    foreach (int i in data)
                    {
                        if (i <= 7)
                        {
                            a[0]++;
                        }
                        else if (i <= 30)
                        {
                            a[1]++;
                        }
                        else if (i <= 180)
                        {
                            a[2]++;
                        }
                        else if (i <= 365)
                        {
                            a[3]++;
                        }
                        else
                        {
                            a[4]++;
                        }
                    }

                    locationData.Add(loc, a);
                }


                var categories = await _context.WareHouseItems.Where(r => r.Category != null)
                                            .Select(r => r.Category)
                                            .Distinct()
                                            .ToListAsync();

                foreach (string category in categories)
                {
                    categoryData.Add(category, await _context.WareHouseItems.Where(w => w.Category == category).CountAsync());
                }

                ClaimRequestCount = await _context.WHTblLostItemRequest.CountAsync();
                PendingRequestCount = await _context.WHTblLostItemRequest.Where(r => r.Status == "Claimed").CountAsync();
                SuccessRequestCount = await _context.WHTblLostItemRequest.Where(r => r.Status == "Returned").CountAsync();
                IdentifiedItemsCount = await _context.WareHouseItems.CountAsync();

            }
            else if (await _context.WareHouseItems.Where(w => w.WarehouseLocation == location).CountAsync() > 0)
            {
                var data = await _context.WareHouseItems.Where(w => w.WarehouseLocation == location)
                    .Select(d => EF.Functions.DateDiffDay(d.CreatedDate, d.UpdatedDate ?? DateTime.Now)).ToListAsync();

                int[] a = [0, 0, 0, 0, 0];

                foreach (int i in data)
                {
                    if (i <= 7)
                    {
                        a[0]++;
                    }
                    else if (i <= 30)
                    {
                        a[1]++;
                    }
                    else if (i <= 180)
                    {
                        a[2]++;
                    }
                    else if (i <= 365)
                    {
                        a[3]++;
                    }
                    else
                    {
                        a[4]++;
                    }
                }
                locationData.Add(location, a);
                var categories = await _context.WareHouseItems.Where(r => r.Category != null && r.WarehouseLocation == location)
                                            .Select(r => r.Category)
                                            .Distinct()
                                            .ToListAsync();

                foreach (string category in categories)
                {
                    categoryData.Add(category, await _context.WareHouseItems.Where(w => w.Category == category && w.WarehouseLocation == location).CountAsync());
                }

                ClaimRequestCount = await _context.WHTblLostItemRequest.Where(r => r.Location == location).CountAsync();
                PendingRequestCount = await _context.WHTblLostItemRequest.Where(r => r.Status == "Claimed" && r.Location == location).CountAsync();
                SuccessRequestCount = await _context.WHTblLostItemRequest.Where(r => r.Status == "Returned" && r.Location == location).CountAsync();
                IdentifiedItemsCount = await _context.WareHouseItems.Where(r => r.WarehouseLocation == location).CountAsync();
            }

            return new DashboardData
            {
                data = locationData,
                lostItemRequestClaimCount = new LostItemRequestClaimCount
                {
                    ClaimRequestCount = ClaimRequestCount,
                    PendingRequestCount = PendingRequestCount,
                    SuccessRequestCount = SuccessRequestCount,
                    IdentifiedItemsCount = IdentifiedItemsCount,
                },
                category = categoryData
            };
        }

        public async Task<IEnumerable<WHLocation>> GetLocations()
        {
            return await _context.WHLocation.ToListAsync();
        }

        public async Task<UserDashboardData> UserCountsData(string user)
        {
            var TotalClaimRequests = await _context.WHTblLostItemRequest.Where(r => r.CreatedBy == user).CountAsync();
            /*var ReturnedClaimsCount = await _context.WHTblLostItemRequest.Where(r => r.CreatedBy == user && (r.Status == "Approve" || r.Status == "Reject")).CountAsync();*/
            var ReturnedClaimsCount = await _context.WHTblLostItemRequest.Where(r => r.CreatedBy == user && (r.Status == "Returned")).CountAsync();
            var PendingRequestCount = TotalClaimRequests - ReturnedClaimsCount;

            return new UserDashboardData
            {
                ClaimRequestCount = TotalClaimRequests,
                PendingRequestCount = PendingRequestCount,
                ReturnedCount = ReturnedClaimsCount             
            };
        }

        public async Task<LostItemRequests> UpdateReceiptStatus(LostItemRequests _object)
        {
            var WareHouseItem = await _context.WareHouseItems.FindAsync(_object.Id);
            if (WareHouseItem == null)
                throw new KeyNotFoundException("WareHouseItem not found");

            WareHouseItem.Status = "Returned";

            _context.Entry(WareHouseItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var userName = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;
            var lostItemRequest = await _context.WHTblLostItemRequest
                                    .FirstOrDefaultAsync(x => x.ClaimId == WareHouseItem.Id);

            if (lostItemRequest == null)
                throw new KeyNotFoundException("lostItemRequest not found");

            lostItemRequest.AdditionalInformation = _object.AdditionalInformation;
            lostItemRequest.IsActive = false;
            lostItemRequest.Status = "Returned";
            lostItemRequest.UpdatedBy = userName;
            lostItemRequest.UpdatedDate = DateTime.Now;
            lostItemRequest.DateTimeWhenLost = DateTime.Now;

            _context.Entry(lostItemRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _object;
        }

    }
}
