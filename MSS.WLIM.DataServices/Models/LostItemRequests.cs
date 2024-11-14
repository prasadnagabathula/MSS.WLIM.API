using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSS.WLIM.DataServices.Models
{
    public class LostItemRequests : AuditData
    {
        public string Description { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? DistinguishingFeatures { get; set; }
        public string? ItemCategory { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? DateTimeWhenLost { get; set; }
        public string? Location { get; set; }
        public decimal? ItemValue { get; set; }
        public string? ItemPhoto { get; set; }
        public string? ProofofOwnership { get; set; }
        public string? HowtheItemLost { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? OtherRelevantDetails { get; set; }
        public string? ClaimId { get; set; }
        [ForeignKey("ClaimId")]
        public WareHouseItem? WareHouseItem { get; set; }

    }

    public class LostItemRequestPhoto
    {
        //public string? Id { get; set; }

       // [FileExtensions(Extensions = "png,jpg,jpeg", ErrorMessage = "Photo must be a .png, .jpg, or .jpeg file.")]
        public IFormFile? ItemPhoto { get; set; }

    }

    public class LostItemRequestsViewModel
    {
        public string Description { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? DistinguishingFeatures { get; set; }
        public string? ItemCategory { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? DateTimeWhenLost { get; set; }
        public string? Location { get; set; }
        public decimal? ItemValue { get; set; }
        public string? ItemPhoto { get; set; }
        public string? ProofofOwnership { get; set; }
        public string? HowtheItemLost { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? OtherRelevantDetails { get; set; }
        public string? ClaimId { get; set; }

    }

    public class DashboardData
    {
        public Dictionary<string, int[]> data { get; set; } = new Dictionary<string, int[]>();
        public LostItemRequestClaimCount lostItemRequestClaimCount { get; set; } = new LostItemRequestClaimCount();
    }

    public class LostItemRequestClaimCount
    {
        public int ClaimRequestCount { get; set; } = 0;
        public int PendingRequestCount { get; set; } = 0;
        public int SuccessRequestCount { get; set; } = 0;
        public int IdentifiedItemsCount { get; set; } = 0;         
    }
    public class Age
    {
        public int? age { get; set; }
    }

}
