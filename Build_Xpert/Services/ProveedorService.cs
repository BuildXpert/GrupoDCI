using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Build_Xpert.Model;


namespace Build_Xpert.Services
{
    public class ProveedorService
    {
        private readonly ApplicationDbContext _context;

        public ProveedorService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // 🔹 Obtener todos los proveedores
        public async Task<List<Proveedor>> GetProveedoresAsync()
        {
            return await _context.Proveedores
                .Include(p => p.Pagos)
                .Include(p => p.Pedidos)
                .AsNoTracking() // Mejora el rendimiento al no rastrear las entidades
                .ToListAsync();
        }

        // 🔹 Obtener un proveedor por ID
        public async Task<Proveedor> GetProveedorByIdAsync(int id)
        {
            return await _context.Proveedores
                .Include(p => p.Pagos)
                .Include(p => p.Pedidos)
                .AsNoTracking() // Mejora el rendimiento al no rastrear las entidades
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // 🔹 Agregar un nuevo proveedor
        public async Task AddProveedorAsync(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor), "El proveedor no puede ser nulo.");

            await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }

        // 🔹 Actualizar un proveedor existente
public async Task UpdateProveedorAsync(Proveedor proveedor)
{
    if (proveedor == null)
        throw new ArgumentNullException(nameof(proveedor), "El proveedor no puede ser nulo.");

    var existingProveedor = await _context.Proveedores.FindAsync(proveedor.Id);
    if (existingProveedor == null)
        throw new KeyNotFoundException("Proveedor no encontrado.");

    // 🔹 Actualizar manualmente solo las propiedades necesarias
    existingProveedor.Nombre = proveedor.Nombre;
    existingProveedor.TipoServicio = proveedor.TipoServicio;
    existingProveedor.Contacto = proveedor.Contacto;
    existingProveedor.Direccion = proveedor.Direccion;
    existingProveedor.Estado = proveedor.Estado;

    await _context.SaveChangesAsync();
}


        // 🔹 Eliminar un proveedor
        public async Task DeleteProveedorAsync(int id)
        {
            var proveedor = await _context.Proveedores
                .Include(p => p.Pedidos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null)
                throw new KeyNotFoundException("Proveedor no encontrado.");

            if (proveedor.Pedidos.Any())
                throw new InvalidOperationException("No se puede eliminar el proveedor porque tiene pedidos asociados.");

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
        }


        // 🔹 Obtener todos los pagos de un proveedor
        public async Task<List<PagoProveedor>> GetPagosByProveedorIdAsync(int proveedorId)
        {
            return await _context.Pagos
                .Where(p => p.ProveedorId == proveedorId)
                .OrderByDescending(p => p.Fecha)
                .AsNoTracking() // Mejora el rendimiento al no rastrear las entidades
                .ToListAsync();
        }

        // 🔹 Registrar un nuevo pago para un proveedor
        public async Task AddPagoAsync(PagoProveedor pago)
        {
            if (pago == null)
                throw new ArgumentNullException(nameof(pago), "El pago no puede ser nulo.");

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
        }

        // 🔹 Obtener todos los pedidos de un proveedor
        public async Task<List<PedidoProveedor>> GetPedidosByProveedorIdAsync(int proveedorId)
        {
            return await _context.Pedidos
                .Where(p => p.ProveedorId == proveedorId)
                .Include(p => p.Project) 
                .AsNoTracking()
                .ToListAsync();
        }


        // 🔹 Crear un nuevo pedido con un proveedor
        public async Task AddPedidoAsync(PedidoProveedor pedido)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            _context.Pedidos.Add(pedido);

            // 🔹 Solo agregamos al inventario si se asocia a un proyecto y proveedor
            if (pedido.ProjectId.HasValue && pedido.ProjectId.Value > 0)
            {
                var item = new InventoryItem
                {
                    Nombre = pedido.Material,
                    Categoria = "Material de obra",
                    Stock = pedido.Cantidad,
                    Precio = pedido.Monto,
                    ProjectId = pedido.ProjectId.Value,
                    ProveedorId = pedido.ProveedorId
                };

                await _context.InventoryItems.AddAsync(item);
            }

            await _context.SaveChangesAsync();
        }


        // 🔹 Eliminar un pago
        public async Task DeletePagoAsync(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
                throw new KeyNotFoundException("Pago no encontrado.");

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
        }

        // 🔹 Eliminar un pedido
        public async Task DeletePedidoAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PedidoProveedor>> GetPedidosByProjectIdAsync(int projectId)
        {
            return await _context.Pedidos
                .Where(p => p.ProjectId == projectId)
                .Include(p => p.Proveedor)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ConvertPedidoToInventarioAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            // Crear nuevo item de inventario
            var inventarioItem = new InventoryItem
            {
                Nombre = pedido.Material,
                Categoria = "Sin categoría", // Podés mejorar esto desde un dropdown
                Stock = pedido.Cantidad,
                Precio = pedido.Monto / pedido.Cantidad, // Precio unitario
                ProveedorId = pedido.ProveedorId,
                ProjectId = (int)pedido.ProjectId
            };

            _context.InventoryItems.Add(inventarioItem);

            // Cambiar el estado del pedido (opcional)
            pedido.Estado = "Aprobado";

            await _context.SaveChangesAsync();
        }


    }
}