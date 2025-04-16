using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Build_Xpert.Model;


namespace Build_Xpert.Services
{
    public class SupplierService
    {
        private readonly ApplicationDbContext _context;

        public SupplierService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // 🔹 Obtener todos los proveedores
        public async Task<List<Supplier>> GetProveedoresAsync()
        {
            return await _context.Suppliers
                .Include(p => p.SupplierPayment)
                .Include(p => p.SupplierOrder)
                .AsNoTracking() // Mejora el rendimiento al no rastrear las entidades
                .ToListAsync();
        }

        // 🔹 Obtener un proveedor por ID
        public async Task<Supplier> GetProveedorByIdAsync(int id)
        {
            return await _context.Suppliers
                .Include(p => p.SupplierPayment)
                .Include(p => p.SupplierOrder)
                .AsNoTracking() // Mejora el rendimiento al no rastrear las entidades
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // 🔹 Agregar un nuevo proveedor
        public async Task AddProveedorAsync(Supplier proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor), "El proveedor no puede ser nulo.");

            await _context.Suppliers.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }

        // 🔹 Actualizar un proveedor existente
public async Task UpdateProveedorAsync(Supplier proveedor)
{
    if (proveedor == null)
        throw new ArgumentNullException(nameof(proveedor), "El proveedor no puede ser nulo.");

    var existingProveedor = await _context.Suppliers.FindAsync(proveedor.Id);
    if (existingProveedor == null)
        throw new KeyNotFoundException("Proveedor no encontrado.");

    // 🔹 Actualizar manualmente solo las propiedades necesarias
    existingProveedor.Name = proveedor.Name;
    existingProveedor.ServiceType = proveedor.ServiceType;
    existingProveedor.Contact = proveedor.Contact;
    existingProveedor.Address = proveedor.Address;
    existingProveedor.ServiceType = proveedor.ServiceType;

    await _context.SaveChangesAsync();
}


        // 🔹 Eliminar un proveedor
        public async Task DeleteProveedorAsync(int id)
        {
            var proveedor = await _context.Suppliers
                .Include(p => p.SupplierOrder)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null)
                throw new KeyNotFoundException("Proveedor no encontrado.");

            if (proveedor.SupplierOrder.Any())
                throw new InvalidOperationException("No se puede eliminar el proveedor porque tiene pedidos asociados.");

            _context.Suppliers.Remove(proveedor);
            await _context.SaveChangesAsync();
        }


        // 🔹 Obtener todos los pagos de un proveedor
        public async Task<List<SupplierPayment>> GetPagosByProveedorIdAsync(int proveedorId)
        {
            return await _context.SupplierPayments
                .Where(p => p.SupplierId == proveedorId)
                .OrderByDescending(p => p.Date)
                .AsNoTracking() // Mejora el rendimiento al no rastrear las entidades
                .ToListAsync();
        }

        // 🔹 Registrar un nuevo pago para un proveedor
        public async Task AddPagoAsync(SupplierPayment pago)
        {
            if (pago == null)
                throw new ArgumentNullException(nameof(pago), "El pago no puede ser nulo.");

            _context.SupplierPayments.Add(pago);
            await _context.SaveChangesAsync();
        }

        // 🔹 Obtener todos los pedidos de un proveedor
        public async Task<List<SupplierOrder>> GetPedidosByProveedorIdAsync(int proveedorId)
        {
            return await _context.SupplierOrders
                .Where(p => p.SupplierId == proveedorId)
                .Include(p => p.Project) 
                .AsNoTracking()
                .ToListAsync();
        }


        // 🔹 Crear un nuevo pedido con un proveedor
        public async Task AddPedidoAsync(SupplierOrder pedido)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            _context.SupplierOrders.Add(pedido);

            // 🔹 Solo agregamos al inventario si se asocia a un proyecto y proveedor
            if (pedido.ProjectId.HasValue && pedido.ProjectId.Value > 0)
            {
                var item = new InventoryItem
                {
                    Name = pedido.Material,
                    Category = "Material de obra",
                    Stock = pedido.Quantity,
                    Price = pedido.Amount,
                    ProjectId = pedido.ProjectId.Value,
                    SupplierId = pedido.SupplierId
                };

                await _context.InventoryItems.AddAsync(item);
            }

            await _context.SaveChangesAsync();
        }


        // 🔹 Eliminar un pago
        public async Task DeletePagoAsync(int id)
        {
            var pago = await _context.SupplierPayments.FindAsync(id);
            if (pago == null)
                throw new KeyNotFoundException("Pago no encontrado.");

            _context.SupplierPayments.Remove(pago);
            await _context.SaveChangesAsync();
        }

        // 🔹 Eliminar un pedido
        public async Task DeletePedidoAsync(int id)
        {
            var pedido = await _context.SupplierOrders.FindAsync(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            _context.SupplierOrders.Remove(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SupplierOrder>> GetPedidosByProjectIdAsync(int projectId)
        {
            return await _context.SupplierOrders
                .Where(p => p.ProjectId == projectId)
                .Include(p => p.Supplier)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ConvertPedidoToInventarioAsync(int pedidoId)
        {
            var pedido = await _context.SupplierOrders
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            // Crear nuevo item de inventario
            var inventarioItem = new InventoryItem
            {
                Name = pedido.Material,
                Category = "Sin categoría", // Podés mejorar esto desde un dropdown
                Stock = pedido.Quantity,
                Price = pedido.Amount / pedido.Quantity, // Precio unitario
                SupplierId = pedido.SupplierId,
                ProjectId = (int)pedido.ProjectId
            };

            _context.InventoryItems.Add(inventarioItem);

            // Cambiar el estado del pedido (opcional)
            pedido.Status = "Aprobado";

            await _context.SaveChangesAsync();
        }


    }
}