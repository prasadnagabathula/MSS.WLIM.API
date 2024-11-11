﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MSS.WLIM.DataServices.Data;
using MSS.WLIM.DataServices.Models;
using MSS.WLIM.DataServices.Repositories;
using System.Drawing;

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
            var lostItemRequests = await _context.WHTblLostItemRequest
                .Include(t => t.WareHouseItem)
                .ToListAsync();

            var LostItemRequestsDto = new List<LostItemRequests>();

            foreach (var d in lostItemRequests)
            {
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
                    ProofofOwnership = d.ProofofOwnership,
                    HowtheItemLost = d.HowtheItemLost,
                    ReferenceNumber = d.ReferenceNumber,
                    AdditionalInformation = d.AdditionalInformation,
                    OtherRelevantDetails = d.OtherRelevantDetails,
                    IsActive = d.IsActive,
                    CreatedBy = d.CreatedBy,
                    CreatedDate = d.CreatedDate,
                    UpdatedBy = d.UpdatedBy,
                    UpdatedDate = d.UpdatedDate
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
                OtherRelevantDetails = lostItemRequest.OtherRelevantDetails,
                IsActive = lostItemRequest.IsActive,
                CreatedBy = lostItemRequest.CreatedBy,
                CreatedDate = lostItemRequest.CreatedDate,
                UpdatedBy = lostItemRequest.UpdatedBy,
                UpdatedDate = lostItemRequest.UpdatedDate
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
                AdditionalInformation = _object.AdditionalInformation,
                OtherRelevantDetails = _object.OtherRelevantDetails,
                IsActive = _object.IsActive,
                CreatedBy = employeeName,
                CreatedDate = DateTime.Now
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
                OtherRelevantDetails = _object.OtherRelevantDetails,
                IsActive = true,
                CreatedBy = employeeName,
                CreatedDate = DateTime.Now,
                ClaimId = _object.ClaimId
            };

           _context.WHTblLostItemRequest.Add(lostItemRequest);
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

        public async Task<LostItemRequestClamCount> ClaimCount()
        {
            return new LostItemRequestClamCount
            {
                ClaimRequestCount = await _context.WHTblLostItemRequest.CountAsync(),
                PendingRequestCount = await _context.WHTblLostItemRequest.Where(r => r.IsActive == true).CountAsync(),
                SuccessRequestCount = await _context.WHTblLostItemRequest.Where(r => r.IsActive == false).CountAsync(),
                IdentifiedItemsCount = await _context.WHTblIdentifiedItems.CountAsync(),
            };
        }
    }
}
