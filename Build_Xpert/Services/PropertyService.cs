using Build_Xpert.Model;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Build_Xpert.Services
{
    public class PropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Property>> GetPropertiesAsync()
        {
            return await _context.Properties.ToListAsync() ?? new List<Property>();
        }

        public async Task AddPropertyAsync(Property property)
        {
            if (property == null)
                throw new ArgumentException("La propiedad no puede ser nula.");

            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePropertyAsync(Property property)
        {
            if (property == null)
                throw new ArgumentException("La propiedad no puede ser nula.");

            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePropertyStatusAsync(int id, string status)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                property.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePropertyAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Property>> GetFilteredPropertiesAsync(string searchText, string status)
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.Name.Contains(searchText) || p.Description.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }

            return await query.ToListAsync();
        }

        public async Task UpdatePropertyDetailsAsync(int id, string constructionType, string province, string canton, double constructionSize, double landSize, int bedrooms, int bathrooms, decimal price, int floors, bool hasGarage, int garageCapacity, bool isCondominium)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                property.ConstructionType = constructionType;
                property.Province = province;
                property.Canton = canton;
                property.ConstructionSize = constructionSize;
                property.LandSize = landSize;
                property.Bedrooms = bedrooms;
                property.Bathrooms = bathrooms;
                property.Price = price;
                property.Floors = floors;
                property.HasGarage = hasGarage;
                property.GarageCapacity = garageCapacity;
                property.IsCondominium = isCondominium;

                await _context.SaveChangesAsync();
            }
        }
    }
}