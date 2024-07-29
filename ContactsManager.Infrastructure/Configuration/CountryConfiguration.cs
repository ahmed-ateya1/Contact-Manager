using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.CountryId);
            builder.Property(e=>e.CountryId)
                .ValueGeneratedNever();

            builder.Property(x => x.CountryName)
            .HasColumnType("varchar(80)") 
            .IsRequired();

            builder.HasIndex(e => e.CountryId)
                .IsUnique();
            builder.ToTable("Countries");
        }
    }
}
