namespace ShowroomManagement.Models
{
    public class VehicleRegistration
    {
        public int Id { get; set; }
        public int VehicleId { get; set; } // Liên kết với Vehicle
        public string RegistrationNumber { get; set; } = string.Empty; // Số đăng ký
        public DateTime RegistrationDate { get; set; } = DateTime.Now; // Ngày đăng ký
        public string OwnerName { get; set; } = string.Empty; // Tên chủ sở hữu

        // Navigation property
        public Vehicle? Vehicle { get; set; }
    }
}
