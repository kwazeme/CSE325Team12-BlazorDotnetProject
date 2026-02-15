using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PropertyHub.Data;

public static class DbInitializer
{
    private const string SeedEmail = "seedadmin@propertyhub.local";
    private const string SeedPassword = "TempPass#123";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync();

        var owner = await EnsureSeedUserAsync(userManager);
        await SeedListingsAsync(context, owner.Id);
        await SeedTenantsAsync(context, owner.Id);
        await SeedLeasesAsync(context, owner.Id);
        await SeedMaintenanceAsync(context, owner.Id);
    }

    private static async Task<ApplicationUser> EnsureSeedUserAsync(UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByEmailAsync(SeedEmail);
        if (user is not null)
        {
            return user;
        }

        user = new ApplicationUser
        {
            UserName = SeedEmail,
            Email = SeedEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, SeedPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed creating seed user: {errors}");
        }

        return user;
    }

    private static async Task SeedListingsAsync(ApplicationDbContext context, string ownerId)
    {
        if (await context.Listings.AnyAsync())
        {
            return;
        }

        context.Listings.AddRange(
            new Listing
            {
                Title = "Maple Heights Apt 2A",
                Address = "154 Maple St",
                City = "Rexburg",
                State = "ID",
                ListingType = "Apartment",
                RentAmount = 1250m,
                YearBuilt = 2012,
                OwnerId = ownerId
            },
            new Listing
            {
                Title = "Pioneer Townhome",
                Address = "880 Pioneer Rd",
                City = "Idaho Falls",
                State = "ID",
                ListingType = "Townhome",
                RentAmount = 1695m,
                YearBuilt = 2018,
                OwnerId = ownerId
            });

        await context.SaveChangesAsync();
    }

    private static async Task SeedTenantsAsync(ApplicationDbContext context, string ownerId)
    {
        if (await context.Tenants.AnyAsync())
        {
            return;
        }

        context.Tenants.AddRange(
            new Tenant
            {
                Name = "Jordan Lee",
                Email = "jordan.lee@example.com",
                PhoneNumber = "208-555-0101",
                OwnerId = ownerId
            },
            new Tenant
            {
                Name = "Morgan Cruz",
                Email = "morgan.cruz@example.com",
                PhoneNumber = "208-555-0102",
                OwnerId = ownerId
            });

        await context.SaveChangesAsync();
    }

    private static async Task SeedLeasesAsync(ApplicationDbContext context, string ownerId)
    {
        if (await context.Leases.AnyAsync())
        {
            return;
        }

        var listings = await context.Listings
            .Where(l => l.OwnerId == ownerId)
            .OrderBy(l => l.Id)
            .ToListAsync();

        var tenants = await context.Tenants
            .Where(t => t.OwnerId == ownerId)
            .OrderBy(t => t.Id)
            .ToListAsync();

        if (listings.Count < 2 || tenants.Count < 2)
        {
            return;
        }

        context.Leases.AddRange(
            new Lease
            {
                ListingId = listings[0].Id,
                TenantId = tenants[0].Id,
                StartDate = DateTime.Today.AddMonths(-3),
                EndDate = DateTime.Today.AddMonths(9),
                OwnerId = ownerId
            },
            new Lease
            {
                ListingId = listings[1].Id,
                TenantId = tenants[1].Id,
                StartDate = DateTime.Today.AddMonths(-1),
                EndDate = DateTime.Today.AddMonths(11),
                OwnerId = ownerId
            });

        await context.SaveChangesAsync();
    }

    private static async Task SeedMaintenanceAsync(ApplicationDbContext context, string ownerId)
    {
        if (await context.MaintenanceRequests.AnyAsync())
        {
            return;
        }

        var listing = await context.Listings
            .Where(l => l.OwnerId == ownerId)
            .OrderBy(l => l.Id)
            .FirstOrDefaultAsync();

        if (listing is null)
        {
            return;
        }

        context.MaintenanceRequests.AddRange(
            new global::MaintenanceRequest
            {
                ListingId = listing.Id,
                Title = "HVAC Filter Replacement",
                Description = "Quarterly replacement of furnace filter.",
                ScheduledDate = DateTime.Today.AddDays(7),
                IsCompleted = false
            },
            new global::MaintenanceRequest
            {
                ListingId = listing.Id,
                Title = "Smoke Detector Check",
                Description = "Inspect and test all smoke detectors.",
                ScheduledDate = DateTime.Today.AddDays(14),
                IsCompleted = false
            });

        await context.SaveChangesAsync();
    }
}
