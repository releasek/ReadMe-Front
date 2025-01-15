namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminUserRoleRel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public virtual AdminRole AdminRole { get; set; }

        public virtual AdminUser AdminUser { get; set; }
    }
}
