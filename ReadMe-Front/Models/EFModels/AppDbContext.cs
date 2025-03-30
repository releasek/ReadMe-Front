namespace ReadMe_Front.Models.EFModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext1")
        {
        }

        public virtual DbSet<AdminRoleFunctionRel> AdminRoleFunctionRels { get; set; }
        public virtual DbSet<AdminRoleFunction> AdminRoleFunctions { get; set; }
        public virtual DbSet<AdminRole> AdminRoles { get; set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Coupon> coupons { get; set; }
        public virtual DbSet<Member_Coupons> member_coupons { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ParentCategory> ParentCategories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<AdminUserRoleRel> AdminUserRoleRels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRoleFunction>()
                .HasMany(e => e.AdminRoleFunctionRels)
                .WithRequired(e => e.AdminRoleFunction)
                .HasForeignKey(e => e.FunctionId);

            modelBuilder.Entity<AdminRole>()
                .HasMany(e => e.AdminRoleFunctionRels)
                .WithRequired(e => e.AdminRole)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<AdminRole>()
                .HasMany(e => e.AdminUserRoleRels)
                .WithRequired(e => e.AdminRole)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .HasMany(e => e.AdminUserRoleRels)
                .WithRequired(e => e.AdminUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.discount_amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.min_spend)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Coupon>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Coupon>()
                .HasMany(e => e.member_coupons)
                .WithRequired(e => e.coupon)
                .HasForeignKey(e => e.coupon_id);

            modelBuilder.Entity<Member_Coupons>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParentCategory>()
                .HasMany(e => e.Categories)
                .WithRequired(e => e.ParentCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.CartItems)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.member_coupons)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.member_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Wishlists)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
