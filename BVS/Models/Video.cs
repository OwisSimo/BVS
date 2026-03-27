using System.ComponentModel.DataAnnotations;

namespace BVS.Models
{
    public class Video
    {
        public int VideoID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        [Display(Name = "Total Copies")]
        public int TotalCopies { get; set; }

        [Display(Name = "Max Rental Days")]
        [Range(1, 3)]
        public int MaxDays { get; set; }

        public decimal RentalRate => Category == "DVD" ? 50 : 25;

        public ICollection<Rental> Rentals { get; set; }
    }
}