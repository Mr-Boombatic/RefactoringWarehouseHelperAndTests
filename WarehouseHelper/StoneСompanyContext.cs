using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WarehouseHelper
{
    public partial class StoneСompanyContext : DbContext
    {
        public StoneСompanyContext(DbContextOptions<StoneСompanyContext> contextOptions) : base(contextOptions)
        {
            ContextOptions = contextOptions;
            this.Database.EnsureCreated();
        }

       protected DbContextOptions<StoneСompanyContext> ContextOptions { get; }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Slab> Slabs { get; set; }
        public virtual DbSet<Stone> Stones { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-8UCPGM6;Database=StoneСompany;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.CarId).ValueGeneratedNever();

                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.SlabId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Worker)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ContractNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Sales");
            });

            modelBuilder.Entity<Slab>(entity =>
            {
                entity.Property(e => e.SlabId).HasMaxLength(20);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Worker)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Stone>(entity =>
            {
                entity.Property(e => e.StoneId).HasMaxLength(20);

                entity.Property(e => e.PricePerCube).HasColumnType("money");

                entity.Property(e => e.Type).HasMaxLength(20);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Stones)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stones_Stones");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.ContractNumber).HasMaxLength(20);

                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.Email).HasMaxLength(20);

                entity.Property(e => e.Date).HasColumnType("datetime");

                //entity.HasOne(d => d.Car)
                //    .WithMany(p => p.Stones)
                //    .HasForeignKey(d => d.CarId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Stones_Stones");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
