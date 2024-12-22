using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class OrderCreateViewModel
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Vehicles { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> PaymentMethods { get; set; } = new List<SelectListItem>();
        public string? VehicleName { get; set; } // Thông tin xe
        public decimal Price { get; set; } // Giá xe
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

    }
    

}
