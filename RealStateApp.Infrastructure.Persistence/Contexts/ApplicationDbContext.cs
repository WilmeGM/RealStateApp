using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        #region "Tables"
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        public DbSet<Improvement> Improvements { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<FavoriteProperty> FavoriteProperties { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        #endregion

        #region "Auditable Entities Configuration"
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region "Table names"
            modelBuilder.Entity<Property>().ToTable("Properties");
            modelBuilder.Entity<PropertyType>().ToTable("PropertyTypes");
            modelBuilder.Entity<SaleType>().ToTable("SaleTypes");
            modelBuilder.Entity<Improvement>().ToTable("Improvements");
            modelBuilder.Entity<Offer>().ToTable("Offers");
            modelBuilder.Entity<FavoriteProperty>().ToTable("FavoriteProperties");
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");
            #endregion

            #region "Primary Keys"
            modelBuilder.Entity<Property>().HasKey(p => p.Id);
            modelBuilder.Entity<PropertyType>().HasKey(b => b.Id);
            modelBuilder.Entity<SaleType>().HasKey(t => t.Id);
            modelBuilder.Entity<Improvement>().HasKey(t => t.Id);
            modelBuilder.Entity<Offer>().HasKey(t => t.Id);
            modelBuilder.Entity<FavoriteProperty>().HasKey(t => t.Id);
            modelBuilder.Entity<ChatMessage>().HasKey(cm => cm.Id);
            #endregion

            #region "Foreign Keys and Relationships" 
            // PropertyType -> Properties (1:N)
            modelBuilder.Entity<PropertyType>()
                .HasMany(pt => pt.Properties)
                .WithOne(p => p.PropertyType)
                .HasForeignKey(p => p.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // SaleType -> Properties (1:N)
            modelBuilder.Entity<SaleType>()
                .HasMany(st => st.Properties)
                .WithOne(p => p.SaleType)
                .HasForeignKey(p => p.SaleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Property -> Offers (1:N)
            modelBuilder.Entity<Property>()
                .HasMany(p => p.Offers)
                .WithOne(o => o.Property)
                .HasForeignKey(o => o.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property -> FavoriteProperties (1:N)
            modelBuilder.Entity<Property>()
                .HasMany(p => p.FavoriteProperties)
                .WithOne(fp => fp.Property)
                .HasForeignKey(fp => fp.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            //Property -> ChatMessages (1:N)
            modelBuilder.Entity<Property>()
                .HasMany(p => p.ChatMessages)
                .WithOne(cm => cm.Property)
                .HasForeignKey(cm => cm.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region "Indexes and Constraints"

            modelBuilder.Entity<Property>()
                .HasIndex(p => p.UniqueCode)
                .IsUnique();

            #endregion

            #region "Property Configuration"
            modelBuilder.Entity<Property>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Offer>()
                .Property(o => o.Amount)
                .HasPrecision(18, 4);
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
