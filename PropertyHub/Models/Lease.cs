using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyHub.Data
{
    public class Lease
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Listing")]
        public int ListingId { get; set; }

        [Required]
        [Display(Name = "Tenant")]
        public int TenantId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        public string? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public ApplicationUser? Owner { get; set; }

        // Navigation Properties
        [ForeignKey("ListingId")]
        public Listing? Listing { get; set; }

        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }
    }
}
