namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Coupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coupon()
        {
            member_coupons = new HashSet<Member_Coupons>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string code { get; set; }

        public decimal discount_amount { get; set; }

        public decimal min_spend { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        public bool? is_member_only { get; set; }

        public DateTime? created_at { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Member_Coupons> member_coupons { get; set; }
    }
}
