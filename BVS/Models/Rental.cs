using System.ComponentModel.DataAnnotations;

namespace BVS.Models
{
    public class Rental
    {
        public int RentalID { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int VideoID { get; set; }
        public Video Video { get; set; }

        [Display(Name = "Rent Date")]
        public DateTime RentDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; }

        public decimal BaseFee => Video?.Category == "DVD" ? 50 : 25;

        public decimal OverdueFine
        {
            get
            {
                if (IsReturned) return 0;
                var today = DateTime.Today;
                var days = (today - DueDate).Days;
                return days > 0 ? days * 5 : 0;
            }
        }

        public decimal TotalFee => BaseFee + OverdueFine;
    }
}