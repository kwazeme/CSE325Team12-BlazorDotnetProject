using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyHub.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Listing> Listings { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Lease> Leases { get; set; }

    // Add DbSet for MaintenanceRequest
    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Listing>().HasData(
        new Listing
        {
            Id = 101,
            Title = "Midnight Valencia",
            Address = "123 Main St",
            City = "Pretoria",
            State = "Gauteng",
            ListingType = "2 Bedroom Condo",
            RentAmount = 234.30m,
            OwnerId = "sample-user-id",
            YearBuilt = 2022,
            ImageUrl = "/images/default-property.jpg"
        }
    );

    // Note: only seed mapped properties. Do not seed computed or [NotMapped] properties.
    modelBuilder.Entity<MaintenanceRequest>().HasData(
        new MaintenanceRequest
        {
            Id = 1,
            ListingId = 101,
            Title = "Leaking Faucet",
            // Use a fixed DateTime for HasData (migrations require constant values)
            ScheduledDate = new DateTime(2026, 2, 2, 0, 0, 0, DateTimeKind.Utc),
            // If CompletedDate is the persisted field, seed that (null = not completed)
            IsCompleted = false
        }
    );
    }
}

