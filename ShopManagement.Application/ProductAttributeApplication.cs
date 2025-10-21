using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopManagement.Application.Contracts.DTOs;
using ShopManagement.Domain.ProductAttributeAgg;

namespace ShopManagement.Application
{
    public interface IProductAttributeApplication
    {
        // Async methods
        Task<long> CreateAttributeAsync(CreateAttributeDto dto);
        Task EditAttributeAsync(EditAttributeDto dto);
        Task<List<AttributeDefinitionDto>> GetAllAttributesAsync();
        Task SetProductAttributesAsync(long productId, List<AttributeValueDto> values);
        Task<List<AttributeValueDto>> GetProductAttributesAsync(long productId);

        // Sync methods (برای Razor Page ها)
        List<AttributeDefinitionDto> GetAllAttributes();
        void SetProductAttributes(long productId, List<AttributeValueDto> attributes);
        List<AttributeValueDto> GetProductAttributesByProductId(long productId);
    }

    public class ProductAttributeApplication : IProductAttributeApplication
    {
        private readonly IProductAttributeRepository _repository;

        public ProductAttributeApplication(IProductAttributeRepository repository)
        {
            _repository = repository;
        }

        // -------------------- Create --------------------
        public async Task<long> CreateAttributeAsync(CreateAttributeDto dto)
        {
            var def = new ProductAttributeDefinition(dto.Name, dto.DataType, dto.IsRequired, dto.AllowedValuesJson);
            return await _repository.CreateDefinitionAsync(def);
        }

        // -------------------- Edit --------------------
        public async Task EditAttributeAsync(EditAttributeDto dto)
        {
            var def = await _repository.GetDefinitionByIdAsync(dto.Id);
            if (def == null) return;
            def.Edit(dto.Name, dto.DataType, dto.IsRequired, dto.AllowedValuesJson);
            await _repository.EditDefinitionAsync(def);
        }

        // -------------------- Get All (async) --------------------
        public async Task<List<AttributeDefinitionDto>> GetAllAttributesAsync()
        {
            var list = await _repository.GetAllDefinitionsAsync();
            return list.Select(d => new AttributeDefinitionDto
            {
                Id = d.Id,
                Name = d.Name,
                DataType = d.DataType,
                IsRequired = d.IsRequired,
                AllowedValuesJson = d.AllowedValuesJson
            }).ToList();
        }

        // -------------------- Set Attributes (async) --------------------
        public async Task SetProductAttributesAsync(long productId, List<AttributeValueDto> values)
        {
            if (values == null || !values.Any())
                return;

            foreach (var val in values)
            {
                var entity = new ProductAttributeValue(productId, val.AttributeDefinitionId, val.Value);
                await _repository.SetProductAttributeValueAsync(entity);
            }
        }

        // -------------------- Get by Product (async) --------------------
        public async Task<List<AttributeValueDto>> GetProductAttributesAsync(long productId)
        {
            var list = await _repository.GetAttributesByProductIdAsync(productId);
            return list.Select(v => new AttributeValueDto
            {
                ProductId = v.ProductId,
                AttributeDefinitionId = v.AttributeDefinitionId,
                Value = v.Value
            }).ToList();
        }

        // -------------------- Get All (sync) --------------------
        public List<AttributeDefinitionDto> GetAllAttributes()
        {
            var list = _repository.GetAllDefinitions();
            return list.Select(d => new AttributeDefinitionDto
            {
                Id = d.Id,
                Name = d.Name,
                DataType = d.DataType,
                IsRequired = d.IsRequired,
                AllowedValuesJson = d.AllowedValuesJson
            }).ToList();
        }

        // -------------------- Set Attributes (sync) --------------------
        public void SetProductAttributes(long productId, List<AttributeValueDto> attributes)
        {
            if (attributes == null || !attributes.Any())
                return;

            var entities = attributes.Select(a =>
                new ProductAttributeValue(productId, a.AttributeDefinitionId, a.Value)).ToList();

            _repository.SetProductAttributes(entities);
        }

        // -------------------- Get by Product (sync) --------------------
        public List<AttributeValueDto> GetProductAttributesByProductId(long productId)
        {
            var list = _repository.GetAttributesByProductId(productId);
            return list.Select(v => new AttributeValueDto
            {
                ProductId = v.ProductId,
                AttributeDefinitionId = v.AttributeDefinitionId,
                Value = v.Value
            }).ToList();
        }
    }
}
