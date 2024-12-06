namespace ShowroomManagement.Models;
public class Vehicle
{
    public int Id { get; set; }
    public string ModelNo { get; set; } = string.Empty; // Không cho phép null
    public string Name { get; set; } = string.Empty;    // Không cho phép null
    public string Brand { get; set; } = string.Empty;   // Không cho phép null
    public decimal Price { get; set; }
    public string Status { get; set; } = "Available";   // Giá trị mặc định
    public string? ImagePath { get; set; }

}
