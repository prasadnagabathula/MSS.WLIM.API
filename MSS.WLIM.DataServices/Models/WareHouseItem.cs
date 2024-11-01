using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSS.WLIM.DataServices.Models
{
    public class WareHouseItem 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CreatedBy { get; set; } = "SYSTEM";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Category { get; set; }
        public string? FilePath { get; set; }
        public string? WarehouseLocation { get; set; }
        public string? Status { get; set; }
        public string? Tags { get; set; }
        public string? ItemDescription { get; set; }
        public string? Comments {  get; set; }
        public ICollection<LostItemRequests> LostItemRequests { get; set; }
    }
}
