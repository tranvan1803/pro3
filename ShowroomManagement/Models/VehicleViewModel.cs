using Microsoft.AspNetCore.Http; // Để hỗ trợ IFormFile
using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class VehicleViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Brand { get; set; }

        public string? ModelNo { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string? Status { get; set; }

        public string? ImagePath { get; set; }
        [Display(Name = "Vehicle Image")]
        public IFormFile? Image { get; set; }
        public int Id { get; internal set; }
    }
}
