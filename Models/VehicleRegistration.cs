namespace ShowroomManagement.Models
{
    public class VehicleRegistration
    {
        public int Id { get; set; }
        public int VehicleId { get; set; } // Liên kết với Vehicle
        public string RegistrationNumber { get; set; } = string.Empty; // Số đăng ký
        public DateTime RegistrationDate { get; set; } = DateTime.Now; // Ngày đăng ký
        public string OwnerName { get; set; } = string.Empty; // Tên chủ sở hữu
        public string Address { get; set; } = string.Empty; // Địa chỉ
        public string PhoneNumber { get; set; } = string.Empty; // Số điện thoại
        public string Email { get; set; } = string.Empty; // Email
        public string OwnershipType { get; set; } = string.Empty; // Loại sở hữu
        public string Color { get; set; } = string.Empty; // Màu sắc
        public string Model { get; set; } = string.Empty; // Mẫu xe
        public string Make { get; set; } = string.Empty; // Hãng xe
        public DateTime? RegistrationExpiryDate { get; set; } // Ngày hết hạn đăng ký

        // Navigation property
        public Vehicle? Vehicle { get; set; }
    }
}