using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Build_Xpert.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<PagoProveedor> Pagos { get; set; }
        public DbSet<PedidoProveedor> Pedidos { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }


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
            builder.Entity<PedidoProveedor>()
                .HasOne(p => p.Proveedor)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Asegurar que `Material` sea tratado como un simple string y no como una relación
            builder.Entity<PedidoProveedor>()
                .Property(p => p.Material)
                .HasColumnType("nvarchar(255)")
                .IsRequired();

            // 🔹 Configurar relación entre `PagoProveedor` y `Proveedor`
            builder.Entity<PagoProveedor>()
                .HasOne(p => p.Proveedor)
                .WithMany(p => p.Pagos)
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}