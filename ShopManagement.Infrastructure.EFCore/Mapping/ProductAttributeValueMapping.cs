using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.ProductAttributeAgg;

namespace ShopManagement.Infrastructure.EFCore.Mapping
{
    public class ProductAttributeValueMapping : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            builder.ToTable("ProductAttributeValues");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.AttributeDefinitionId).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(1000);
            builder.Property(x => x.CreatedAt).IsRequired();

            // FK to attribute definition configured on the other side as well
        }
    }
}
