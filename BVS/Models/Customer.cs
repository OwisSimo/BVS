using System.ComponentModel.DataAnnotations;

namespace BVS.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}