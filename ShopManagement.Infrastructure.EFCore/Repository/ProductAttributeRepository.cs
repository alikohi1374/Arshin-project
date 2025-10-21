using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAttributeAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductAttributeRepository : IProductAttributeRepository
    {
        private readonly ShopContext _context;

        public ProductAttributeRepository(ShopContext context)
        {
            _context = context;
        }

        // -------------------- Create Definition --------------------
        public async Task<long> CreateDefinitionAsync(ProductAttributeDefinition definition)
        {
            await _context.Set<ProductAttributeDefinition>().AddAsync(definition);
            await _context.SaveChangesAsync();
            return definition.Id;
        }

        // -------------------- Edit Definition --------------------
        public async Task EditDefinitionAsync(ProductAttributeDefinition definition)
        {
            _context.Set<ProductAttributeDefinition>().Update(definition);
            await _context.SaveChangesAsync();
        }

        // -------------------- Get By Id --------------------
        public async Task<ProductAttributeDefinition> GetDefinitionByIdAsync(long id)
            => await _context.Set<ProductAttributeDefinition>()
                             .Include(d => d.AttributeValues)
                             .FirstOrDefaultAsync(d => d.Id == id);

        // -------------------- Get All (async) --------------------
        public async Task<List<ProductAttributeDefinition>> GetAllDefinitionsAsync()
            => await _context.Set<ProductAttributeDefinition>().ToListAsync();

        // -------------------- Set Single Product Attribute Value (async) --------------------
        public async Task SetProductAttributeValueAsync(ProductAttributeValue value)
        {
            var existed = await _context.Set<ProductAttributeValue>()
                .FirstOrDefaultAsync(v => v.ProductId == value.ProductId && v.AttributeDefinitionId == value.AttributeDefinitionId);

            if (existed == null)
            {
                await _context.Set<ProductAttributeValue>().AddAsync(value);
            }
            else
            {
                existed.EditValue(value.Value);
                _context.Set<ProductAttributeValue>().Update(existed);
            }

            await _context.SaveChangesAsync();
        }

        // -------------------- Get Attributes By Product (async) --------------------
        public async Task<List<ProductAttributeValue>> GetAttributesByProductIdAsync(long productId)
            => await _context.Set<ProductAttributeValue>()
                             .Where(v => v.ProductId == productId)
                             .Include(v => v.AttributeDefinition)
                             .ToListAsync();

        // -------------------- Get All (sync) --------------------
        public List<ProductAttributeDefinition> GetAllDefinitions()
        {
            return _context.Set<ProductAttributeDefinition>()
                           .AsNoTracking()
                           .ToList();
        }

        // -------------------- Set Product Attributes (sync) --------------------
        public void SetProductAttributes(List<ProductAttributeValue> entities)
        {
            if (entities == null || !entities.Any())
                return;

            foreach (var entity in entities)
            {
                var existed = _context.Set<ProductAttributeValue>()
                    .FirstOrDefault(v => v.ProductId == entity.ProductId &&
                                         v.AttributeDefinitionId == entity.AttributeDefinitionId);

                if (existed == null)
                {
                    _context.Set<ProductAttributeValue>().Add(entity);
                }
                else
                {
                    existed.EditValue(entity.Value);
                    _context.Set<ProductAttributeValue>().Update(existed);
                }
            }

            _context.SaveChanges();
        }

        // -------------------- Get Attributes By Product (sync) --------------------
        public List<ProductAttributeValue> GetAttributesByProductId(long productId)
        {
            return _context.Set<ProductAttributeValue>()
                           .Include(v => v.AttributeDefinition)
                           .Where(v => v.ProductId == productId)
                           .AsNoTracking()
                           .ToList();
        }

        // -------------------- Delete --------------------
        public async Task DeleteAttributeValueAsync(long id)
        {
            var item = await _context.Set<ProductAttributeValue>().FindAsync(id);
            if (item == null) return;
            _context.Set<ProductAttributeValue>().Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
