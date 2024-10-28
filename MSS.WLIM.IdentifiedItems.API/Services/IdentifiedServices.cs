using MSS.WLIM.DataServices.Data;
using Microsoft.EntityFrameworkCore;
using MSS.WLIM.DataServices.Models;
using MSS.WLIM.DataServices.Repositories;


namespace MSS.WLIM.IdentifiedItem.API.Services
{
    public class IdentifiedServices: IIdentifiedItemServices
    {
        private readonly IRepository<IdentifiedItems> _repository;
        private readonly DataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentifiedServices(IRepository<IdentifiedItems> repository, DataBaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IdentifiedItems> Add(IdentifiedItems item)
        {
            var admin = new IdentifiedItems
            {
                Photos = item.Photos,
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
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,

            };
            // Add the new item to the DbSet
            await _context.WHTblIdentifiedItems.AddAsync(admin);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return item;
        }


        public async Task<IEnumerable<IdentifiedItems>> GetAll()
        {
            var IdentifiedItems = await _context.WHTblIdentifiedItems.ToListAsync();

            var IdentifiedItemsDto = new List<IdentifiedItems>();
            foreach (var item in IdentifiedItems)
            {
                IdentifiedItemsDto.Add(new IdentifiedItems
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
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedDate = item.UpdatedDate
                });
            }
            return IdentifiedItemsDto;
        }

        public Task<IdentifiedItems> Update(IdentifiedItems employee)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentifiedItems> Get(string id)
        {
            throw new NotImplementedException();
        }


    }

}
