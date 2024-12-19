using Microsoft.EntityFrameworkCore;
using MSS.WLIM.DataServices.Data;
using MSS.WLIM.DataServices.Models;

namespace MSS.WLIM.Upload.API.Services
{
    public class WareHouseItemService : IWareHouseItemService
    {
        private readonly DataBaseContext _context;


        public WareHouseItemService(DataBaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<WareHouseItem> Add(WareHouseItem item)
        {
            // Add the new item to the DbSet
            await _context.WareHouseItems.AddAsync(item);


            // Save changes to the database
            await _context.SaveChangesAsync();

            return item;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<WareHouseItem> Get(string id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<WareHouseItem>> GetAll()
        {
            var identifiedItem = await _context.WareHouseItems.ToListAsync();

            var warehouseItemDto = new List<WareHouseItem>();

            foreach (var d in identifiedItem)
            {
                warehouseItemDto.Add(new WareHouseItem
                {
                    Id = d.Id,
                    Category = d.Category,
                    Tags = d.Tags,
                    ItemDescription = d.ItemDescription,
                    WarehouseLocation = d.WarehouseLocation,
                    Comments = d.Comments,
                    CreatedBy = d.CreatedBy,
                    CreatedDate = d.CreatedDate,
                    UpdatedBy = d.UpdatedBy,
                    UpdatedDate = d.UpdatedDate,
                    FilePath = d.FilePath,
                    Donated = d.Donated,
                    QRCodeContent = d.QRCodeContent,

                });
            }

            return warehouseItemDto;

        }

        public async Task<WareHouseItem> GetById(string id)
        {
            var identifiedItem = await _context.WareHouseItems.FindAsync(id);

            if (identifiedItem == null)
            {
                return null;
            }

            var warehouseItemDto = new WareHouseItem
            {
                Id = identifiedItem.Id,
                Category = identifiedItem.Category,
                Tags = identifiedItem.Tags,
                ItemDescription = identifiedItem.ItemDescription,
                WarehouseLocation = identifiedItem.WarehouseLocation,
                Comments = identifiedItem.Comments,
                CreatedBy = identifiedItem.CreatedBy,
                CreatedDate = identifiedItem.CreatedDate,
                UpdatedBy = identifiedItem.UpdatedBy,
                UpdatedDate = identifiedItem.UpdatedDate,
                FilePath = identifiedItem.FilePath,
                Donated = identifiedItem.Donated,
                QRCodeContent = identifiedItem.QRCodeContent,
            };

            return warehouseItemDto;
        }


        public Task<WareHouseItem> Update(WareHouseItem employee)
        {
            throw new NotImplementedException();
        }
    }
}
