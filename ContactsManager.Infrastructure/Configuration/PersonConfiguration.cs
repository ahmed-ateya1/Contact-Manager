using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x=>x.PersonId);
            builder.Property(x => x.PersonId)
                .ValueGeneratedNever();

            builder.Property(x => x.Address)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x=>x.Email)
                .HasColumnType("varchar(80)")
                .IsRequired();

            builder.Property(x => x.ReceiveNewsLetters);

            builder.Property(x => x.DateOfBirth)
                .HasColumnName("date")
                .IsRequired();
            builder.Property(x => x.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345");

            builder.HasOne(x => x.Country)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.ToTable("Persons");
        }
    }
}
