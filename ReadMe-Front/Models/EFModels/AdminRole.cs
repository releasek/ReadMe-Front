namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminRole
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public int PermissionId { get; set; }

        public int GroupId { get; set; }

        public virtual AdminGroup AdminGroup { get; set; }

        public virtual AdminPermission AdminPermission { get; set; }
    }
}
