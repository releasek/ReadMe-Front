namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminRoleFunctionRel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int FunctionId { get; set; }

        public virtual AdminRoleFunction AdminRoleFunction { get; set; }

        public virtual AdminRole AdminRole { get; set; }
    }
}
