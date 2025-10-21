using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.ProductAttributeAgg;

namespace ShopManagement.Infrastructure.EFCore.Mapping
{
    public class ProductAttributeDefinitionMapping : IEntityTypeConfiguration<ProductAttributeDefinition>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeDefinition> builder)
        {
            builder.ToTable("ProductAttributeDefinitions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DataType).IsRequired().HasMaxLength(50);
            builder.Property(x => x.IsRequired).IsRequired();
            builder.Property(x => x.AllowedValuesJson).HasColumnType("nvarchar(max)");

            builder.HasMany(x => x.AttributeValues)
                .WithOne(x => x.AttributeDefinition)
                .HasForeignKey(x => x.AttributeDefinitionId);
        }
    }
}
