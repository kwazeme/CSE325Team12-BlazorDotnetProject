
using Microsoft.EntityFrameworkCore;
using PropertyHub.Data;

public class MaintenanceService
{
    private readonly ApplicationDbContext _context;

    public MaintenanceService(ApplicationDbContext context) => _context = context;

    public List<MaintenanceRequest> GetAll()
    {
        // I use .Include to load the related Listing data
        return _context.MaintenanceRequests
                       .Include(r => r.Listing) 
                       .ToList();
    }

    public List<Listing> GetListings() => _context.Listings.ToList();
    
    public MaintenanceRequest GetById(int id) => _context.MaintenanceRequests.Find(id);

    public void Save(MaintenanceRequest request)
    {
        if (request.Id == 0) 
        {
            _context.MaintenanceRequests.Add(request);
        }
        else 
        {
            _context.MaintenanceRequests.Update(request);
        }
        _context.SaveChanges();
    }
}