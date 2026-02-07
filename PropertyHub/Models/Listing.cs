using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyHub.Data
{
    public class Listing
    {
         public int Id { get; set; }

        [Required]
         [Display(Name = "Property Name")]
        public string? Title { get; set ;}

        [Required]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        [Display(Name = "Listing Type")]
        public string? ListingType { get; set; }

        [Required]
        [Display(Name = "Rent Amount")]
        [DataType(DataType.Currency)]
        public decimal RentAmount { get; set; }

        [Required]
        public string? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public ApplicationUser? Owner { get; set; }

        public int YearBuilt { get; set; }

        public string ImageUrl { get; set; } = "/images/default-property.jpg";

        // Relationship: One to Many
        public List<MaintenanceRequest> MaintenanceHistory { get; set; } = new();

        // Navigation Property
        public ICollection<Lease>? Leases { get; set; }
    }
}
