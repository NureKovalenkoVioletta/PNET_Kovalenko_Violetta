using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietaryFitnessProject.DAL.Data.Configurations;

public class RecipeProductConfiguration : IEntityTypeConfiguration<RecipeProduct>
{
    public void Configure(EntityTypeBuilder<RecipeProduct> builder)
    {
        builder.ToTable("RecipeProduct", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_RecipeProduct_Grams_NonNegative", "[Grams] >= 0");
        });

        builder.HasKey(x => new { x.RecipeId, x.ProductId });
        builder.Property(x => x.RecipeId).HasColumnName("RecipeId");
        builder.Property(x => x.ProductId).HasColumnName("ProductId");
        builder.Property(x => x.Grams).HasColumnName("Grams").HasColumnType("float").IsRequired();

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.RecipeProducts)
            .HasForeignKey(x => x.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.RecipeProducts)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
