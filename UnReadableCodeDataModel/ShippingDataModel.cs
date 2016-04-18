namespace UnReadableCodeDataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ShippingDataModel : DbContext
    {
        public ShippingDataModel()
            : base("name=ShippingDataModel")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Item> ITEMS { get; set; }
        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>()
                .Property(e => e.Cost)
                .HasPrecision(19, 4);
        }
    }
}
