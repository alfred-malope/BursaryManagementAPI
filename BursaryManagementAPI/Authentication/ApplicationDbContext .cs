using BursaryManagementAPI.Models.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public DbSet<ApplicationUser> User { get; set; }
    public DbSet<ContactDetails> ContactDetails { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships and constraints
        builder.Entity<ApplicationUser>()
            .HasOne(user => user.ContactDetails)
            .WithMany(contacts => contacts.User)
            .HasForeignKey(user => user.ContactID);

        // Configure many-to-many relationship between ApplicationUser and IdentityRole
        builder.Entity<UserRole>()
            .HasKey(userRole => new { userRole.UserID, userRole.RoleID });


        //one user can have many roles
        builder.Entity<UserRole>()
            .HasOne(userRole => userRole.User)
            .WithMany(user => user.UserRole)
            .HasForeignKey(userRole => userRole.UserID);


        builder.Entity<UserRole>()
            .HasOne(userRole => userRole.Role)
            .WithMany()
            .HasForeignKey(userRole => userRole.RoleID);

        // Additional configurations for the ContactDetails entity, if needed

        // Ensure the ContactDetails table has a unique constraint on Email
        builder.Entity<ContactDetails>()
            .HasIndex(contact => contact.Email)
            .IsUnique();
    }
}
