using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;

        public Vehicle? Vehicle { get; set; }  // Mối quan hệ với Vehicle
        public Customer? Customer { get; set; }  // Mối quan hệ với Customer
        public int Quantity { get; set; } // Thêm số lượng
    }

    public enum PaymentMethod
    {
        [Display(Name = "Cash")]
        Cash,

        [Display(Name = "Thanh toán chuyển khoản")]
        BankTransfer,

        [Display(Name = "Paypal")]
        Paypal
    }

}