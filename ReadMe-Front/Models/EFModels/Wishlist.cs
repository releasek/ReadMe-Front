namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wishlist")]
    public partial class Wishlist
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
