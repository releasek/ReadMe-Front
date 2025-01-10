namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminRoleFunction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdminRoleFunction()
        {
            AdminRoleFunctionRels = new HashSet<AdminRoleFunctionRel>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FunctionName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminRoleFunctionRel> AdminRoleFunctionRels { get; set; }
    }
}
