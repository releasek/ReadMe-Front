namespace ReadMe_Front.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminPermission
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public bool CanManageMembers { get; set; }

        public bool CanManageProducts { get; set; }

        public bool CanManageOrders { get; set; }

        public bool CanManagePromotions { get; set; }

        public bool CanViewReports { get; set; }

        public bool CanConfigureSystem { get; set; }
    }
}
