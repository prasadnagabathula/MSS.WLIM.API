using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSS.WLIM.DataServices.Models
{
    [Table("WHTblIdentifiedItems")]
    public class IdentifiedItems
    {

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Photos { get; set; }
        public string? ItemDescription { get; set; }
        public string? BrandMake { get; set; }
        public string? ModelVersion { get; set; }
        public string? Color { get; set; }
        public string? SerialNumber { get; set; }
        public string? DistinguishingFeatures { get; set; }
        public string? Condition { get; set; }
        public DateTime? IdentifiedDate { get; set; }
        public string? IdentifiedLocation { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public string? Comments { get; set; }

        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = "SYSTEM";
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
