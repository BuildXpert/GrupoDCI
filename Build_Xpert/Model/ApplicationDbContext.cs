using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Build_Xpert.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPhaseTasks> ProjectPhaseTasks { get; set; }
        public DbSet<ProjectPhase> ProjectPhases { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierPayment> SupplierPayments { get; set; }
        public DbSet<SupplierOrder> SupplierOrders { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryItemUsage> InventoryItemUsage { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔹 Configurar relación entre Project y Cliente (Usuario)
            builder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Configurar relación entre Project y Administrador (Usuario)
            builder.Entity<Project>()
                .HasOne(p => p.Admin)
                .WithMany()
                .HasForeignKey(p => p.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Configurar precisión para `decimal`
            builder.Entity<Project>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            builder.Entity<Property>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            // 🔹 Configurar relación entre `PedidoProveedor` y `Proveedor`
            builder.Entity<SupplierOrder>()
                .HasOne(p => p.Supplier)
                .WithMany(p => p.SupplierOrder)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Asegurar que `Material` sea tratado como un simple string y no como una relación
            builder.Entity<SupplierOrder>()
                .Property(p => p.Material)
                .HasColumnType("nvarchar(255)")
                .IsRequired();

            // 🔹 Configurar relación entre `PagoProveedor` y `Proveedor`
            builder.Entity<SupplierPayment>()
                .HasOne(p => p.Supplier)
                .WithMany(p => p.SupplierPayment)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectPhase>()
                .HasMany(p => p.ProjectPhaseTasks)
                .WithOne() // o .WithOne(t => t.ProjectPhase) si está definido así
                .HasForeignKey(t => t.PhaseId);

            builder.Entity<ProjectPhaseTasks>()
                .Property(p => p.Description)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(max)"); // Si esperas un texto largo

        }
    }
}