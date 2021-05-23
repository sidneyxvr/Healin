using Healin.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Healin.Infrastructure.Identity.Data
{
    public class ApplicationDbContext :
        IdentityDbContext<
        ApplicationUser,
        IdentityRole<Guid>, Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(options =>
            {
                options.ToTable("ApplicationUser");

                options.HasKey(user => user.Id);

                options.Property(user => user.UserName).HasColumnType("varchar(254)");
                options.Property(user => user.NormalizedUserName).HasColumnType("varchar(254)");

                options.Property(user => user.Email).HasColumnType("varchar(254)");
                options.Property(user => user.NormalizedEmail).HasColumnType("varchar(254)");

                options.Property(user => user.PhoneNumber).HasColumnType("varchar(20)");

                options.Property(user => user.SecurityStamp).HasColumnType("varchar(256)");
                options.Property(user => user.ConcurrencyStamp).HasColumnType("varchar(256)");
                options.Property(user => user.PasswordHash).HasColumnType("varchar(256)");

                options.HasMany<IdentityUserClaim<Guid>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

                options.HasMany<IdentityUserLogin<Guid>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

                options.HasMany<IdentityUserToken<Guid>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

                options.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<IdentityRole<Guid>>(options =>
            {
                options.ToTable("ApplicationRole");

                options.HasKey(role => role.Id);

                options.Property(role => role.Name).HasColumnType("varchar(256)");
                options.Property(role => role.NormalizedName).HasColumnType("varchar(256)");

            });

            builder.Entity<IdentityRoleClaim<Guid>>(options =>
            {
                options.HasKey(roleClaim => roleClaim.Id);

                options.Property(roleClaim => roleClaim.ClaimType).HasColumnType("varchar(50)");
                options.Property(roleClaim => roleClaim.ClaimValue).HasColumnType("varchar(50)");

                options.ToTable("ApplicationClaim");
            });

            builder.Entity<IdentityUserLogin<Guid>>(options =>
            {
                options.ToTable("ApplicationUserLogin");

                options.HasKey(userLogin => new { userLogin.ProviderKey, userLogin.LoginProvider });

                options.Property(userLogin => userLogin.LoginProvider).HasColumnType("varchar(128)");
                options.Property(userLogin => userLogin.ProviderKey).HasColumnType("varchar(128)");
                options.Property(userLogin => userLogin.ProviderDisplayName).HasColumnType("varchar(50)");
            });

            builder.Entity<IdentityUserToken<Guid>>(options =>
            {
                options.ToTable("ApplicationUserToken");

                options.HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name });

                options.Property(userToken => userToken.LoginProvider).HasColumnType("varchar(128)");
                options.Property(userToken => userToken.Name).HasColumnType("varchar(128)");
                options.Property(userToken => userToken.Value).HasColumnType("varchar(256)");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(options =>
            {
                options.ToTable("ApplicationRoleClaim");

                options.HasKey(roleClaim => roleClaim.Id);
            });

            builder.Entity<IdentityUserRole<Guid>>(options =>
            {
                options.ToTable("ApplicationUserRole");

                options.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
            });

            builder.Entity<IdentityUserClaim<Guid>>(options =>
            {
                options.ToTable("ApplicationUserClaim");

                options.HasKey(userClaim => userClaim.Id);

                options.Property(userClaim => userClaim.ClaimValue).HasColumnType("varchar(50)");
                options.Property(userClaim => userClaim.ClaimType).HasColumnType("varchar(50)");
            });
        }
    }
}
