namespace ShowroomManagement.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty; // Khóa cài đặt (e.g., "SiteTitle")
        public string Value { get; set; } = string.Empty; // Giá trị cài đặt (e.g., "My Showroom")
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Ngày tạo
    }
}
