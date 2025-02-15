﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSS.WLIM.DataServices.Models
{
    public class AuditData
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsActive { get; set; } = false;
        public string CreatedBy { get; set; } = "SYSTEM";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
