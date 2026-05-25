using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietaryFitnessProject.DAL.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_Product_CaloriesPer100g_NonNegative", "[ProductCaloriesPer100g] >= 0");
            tableBuilder.HasCheckConstraint("CK_Product_ProteinsPer100g_NonNegative", "[ProductProteinsPer100g] >= 0");
            tableBuilder.HasCheckConstraint("CK_Product_FatsPer100g_NonNegative", "[ProductFatsPer100g] >= 0");
            tableBuilder.HasCheckConstraint("CK_Product_CarbsPer100g_NonNegative", "[ProductCarbsPer100g] >= 0");
        });

        builder.HasKey(x => x.ProductId);
        builder.Property(x => x.ProductId).HasColumnName("ProductId").UseIdentityColumn();
        builder.Property(x => x.ProductName).HasColumnName("ProductName").HasMaxLength(100).IsRequired();
        builder.Property(x => x.ProductCaloriesPer100g).HasColumnName("ProductCaloriesPer100g").HasColumnType("float").IsRequired();
        builder.Property(x => x.ProductProteinsPer100g).HasColumnName("ProductProteinsPer100g").HasColumnType("float").IsRequired();
        builder.Property(x => x.ProductFatsPer100g).HasColumnName("ProductFatsPer100g").HasColumnType("float").IsRequired();
        builder.Property(x => x.ProductCarbsPer100g).HasColumnName("ProductCarbsPer100g").HasColumnType("float").IsRequired();
    }
}
