using Build_Xpert.Model;
using Microsoft.EntityFrameworkCore;

namespace Build_Xpert.Services
{
    public class InventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryItem>> GetByProjectIdAsync(int projectId)
        {
            return await _context.InventoryItems
                .Include(i => i.Proveedor)
                .Include(i => i.Project)
                .Where(i => i.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(InventoryItem item)
        {
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InventoryItem item)
        {
            // 1. Verificar si la entidad ya está siendo rastreada
            var local = _context.InventoryItems.Local.FirstOrDefault(e => e.Id == item.Id);
            if (local != null)
            {
                // 2. Detach la entidad previamente rastreada
                _context.Entry(local).State = EntityState.Detached;
            }

            // 3. Adjuntar la nueva instancia y marcarla como modificada
            _context.Attach(item);
            _context.Entry(item).State = EntityState.Modified;

            // 4. Ignorar relaciones si no las estás modificando
            _context.Entry(item).Reference(i => i.Proveedor).IsModified = false;
            _context.Entry(item).Reference(i => i.Project).IsModified = false;

            await _context.SaveChangesAsync();
        }

        public async Task DesasociarDeProyectoAsync(int itemId)
        {
            var item = await _context.InventoryItems.FindAsync(itemId);
            if (item != null)
            {
                item.ProjectId = null;
                _context.InventoryItems.Update(item);
                await _context.SaveChangesAsync();
            }
        }



        public async Task DeleteAsync(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item != null)
            {
                _context.InventoryItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
