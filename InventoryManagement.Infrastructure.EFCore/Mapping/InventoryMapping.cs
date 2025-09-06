using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.EFCore.Mapping
{
   public class InventoryMapping:IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");
            builder.HasKey(x => x.Id);
            builder.OwnsMany(x => x.Operations, modelbuilder =>
            {
                modelbuilder.HasKey(x => x.Id);
                modelbuilder.ToTable("InventoryOperations");
                modelbuilder.WithOwner(x => x.Inventory).HasForeignKey(x => x.InventoryId);
                modelbuilder.Property(x => x.Description).HasMaxLength(5000);
            });
        }
    }
}
