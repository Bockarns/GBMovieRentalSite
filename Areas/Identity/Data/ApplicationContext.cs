using GBMovieRentalSite.Areas.Identity.Data;
using GBMovieRentalSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GBMovieRentalSite.Areas.Identity.Data;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Movie config
        builder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Description).HasMaxLength(1000);
            entity.Property(m => m.PricePerDay).HasColumnType("decimal(18,5)");
        });
        //Rental config
        builder.Entity<Rental>(entity =>
        {
            entity.HasKey(r => r.Id);

            //USer Relation
            entity.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict); //Prevent cascade delete

            //Movie relation
            entity.HasOne(r => r.Movie).WithMany().HasForeignKey(r => r.MovieId).OnDelete(DeleteBehavior.Restrict);

            entity.Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");
        });

        builder.Entity<Review>(entity =>
        {
            entity.HasKey(rv => rv.Id);

            //USer Relation
            entity.HasOne(rv => rv.User).WithMany().HasForeignKey(rv => rv.UserId).OnDelete(DeleteBehavior.Restrict);

            //Movie relation
            entity.HasOne(rv => rv.Movie).WithMany().HasForeignKey(rv => rv.MovieId).OnDelete(DeleteBehavior.Restrict);

            //Rental relation
            entity.HasOne(rv => rv.Rental).WithOne().HasForeignKey<Review>(rv => rv.RentalId).OnDelete(DeleteBehavior.Restrict);

            entity.Property(rv => rv.Comment).HasMaxLength(2000);
            entity.Property(rv => rv.Rating).IsRequired();
        });
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        RenameIdentityTables(builder);
    }
    protected void RenameIdentityTables(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("MovieRental");
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "Users");
        });
        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Roles");
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable(name: "UserRoles");
        });
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable(name: "UserClaims");
        });
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable(name: "UserLogin");
        });
        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable(name: "RoleClaims");
        });
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable(name: "UserTokens");
        });

    }
    
}
