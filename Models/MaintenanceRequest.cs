using System.ComponentModel.DataAnnotations;
using PropertyHub.Data;
public class MaintenanceRequest
{
    public int Id { get; set; }
    
    // Foreign Key to Listing
    [Required]
    public int ListingId { get; set; }
    
    public Listing? Listing { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; } = DateTime.Now;
    public bool IsCompleted { get; set; }
}