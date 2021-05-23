using Healin.Domain.Models;
using Healin.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healin.Infrastructure.Data.Mappings
{
    public class PatientMapping : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patient");

            builder.Property(user => user.Name).HasColumnType("varchar(100)");

            builder.Property(user => user.BirthDate).HasColumnType("date");

            builder.OwnsOne(user => user.Address, address =>
            {
                address.Property(a => a.City).HasColumnType("varchar(40)").HasColumnName("City");

                address.Property(a => a.Complement).HasColumnType("varchar(50)").HasColumnName("Complement");

                address.Property(a => a.Country).HasColumnType("varchar(10)").HasColumnName("Country");

                address.Property(a => a.District).HasColumnType("varchar(50)").HasColumnName("District");

                address.Property(a => a.Number).HasColumnType("varchar(5)").HasColumnName("Number");

                address.Property(a => a.PostalCode).HasColumnType("char(8)").HasColumnName("PostalCode");

                address.Property(a => a.State).HasColumnType("char(2)").HasColumnName("State");

                address.Property(a => a.Street).HasColumnType("varchar(50)").HasColumnName("Street");
            });

            builder.OwnsOne(user => user.ImagePath, imagePath =>
            {
                imagePath.Property(e => e.Path).HasColumnType("varchar(200)").HasColumnName("ImagePath");
            });

            builder.OwnsOne(user => user.Cpf, document =>
            {
                document.Property(e => e.Value).HasColumnType("varchar(11)").HasColumnName("Cpf");
            });

            builder.OwnsOne(user => user.Email, email =>
            {
                email.Property(e => e.Value).HasColumnType("varchar(254)").HasColumnName("Email");
            });

            builder.OwnsOne(user => user.Phone, phone =>
            {
                phone.Property(e => e.Value).HasColumnType("varchar(20)").HasColumnName("PhoneNumber");
            });
        }
    }
}
