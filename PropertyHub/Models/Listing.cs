using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;
using Newtonsoft.Json;

namespace PropertyHub.Data
{
    public class Listing
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Property Name")]
        public string? Title {get; set; }

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

        // add Maintenance Centric parameters to Listings
        [Required]
        public int YearBuilt { get; set; }
        [Required]
        public string ImageUrl { get; set;} = "/image/default-property_img.jpg";

        // Navigation Property
        public ICollection<Lease>? Leases { get; set; }
    }
}
