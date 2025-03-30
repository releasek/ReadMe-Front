namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member_Coupons
    {
        public int id { get; set; }

        public int member_id { get; set; }

        public int coupon_id { get; set; }

        [StringLength(10)]
        public string status { get; set; }

        public DateTime? received_at { get; set; }

        public DateTime? used_at { get; set; }

        public virtual Coupon coupon { get; set; }

        public virtual User User { get; set; }
    }
}
