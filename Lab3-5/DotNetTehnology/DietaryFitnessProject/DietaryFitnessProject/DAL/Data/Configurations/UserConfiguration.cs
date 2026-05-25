using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietaryFitnessProject.DAL.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_User_Height_NonNegative", "[Height] >= 0");
            tableBuilder.HasCheckConstraint("CK_User_Weight_NonNegative", "[Weight] >= 0");
            tableBuilder.HasCheckConstraint("CK_User_Age_Range", "[Age] >= 10 AND [Age] <= 120");
        });

        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId).HasColumnName("UserId").UseIdentityColumn();
        builder.Property(x => x.FirstName).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasColumnName("Email").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Gender).HasColumnName("Gender").HasConversion<int>().IsRequired();
        builder.Property(x => x.Age).HasColumnName("Age").IsRequired().HasDefaultValue(25);
        builder.Property(x => x.Height).HasColumnName("Height").HasColumnType("float").IsRequired();
        builder.Property(x => x.Weight).HasColumnName("Weight").HasColumnType("float").IsRequired();
        builder.Property(x => x.ActivityLevel).HasColumnName("ActivityLevel").HasConversion<int>().IsRequired();
        builder.Property(x => x.Goal).HasColumnName("Goal").HasConversion<int>().IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(512).IsRequired();
        builder.Property(x => x.PasswordSalt).HasColumnName("PasswordSalt").HasMaxLength(256).IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("CreatedAtUtc").IsRequired();
        builder.Property(x => x.LastLoginAtUtc).HasColumnName("LastLoginAtUtc").IsRequired(false);

        builder.HasIndex(x => x.Email).IsUnique();
    }
}
