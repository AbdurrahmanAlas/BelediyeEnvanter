using System.ComponentModel.DataAnnotations;

namespace BelediyeEnvanter.Models;

public class Category { public int Id { get; set; } [Required, MaxLength(100)] public string Name { get; set; } = ""; }
public class AuditLog { public int Id { get; set; } [Required, MaxLength(100)] public string Kullanici { get; set; } = ""; [Required, MaxLength(100)] public string Islem { get; set; } = ""; [MaxLength(500)] public string Aciklama { get; set; } = ""; public DateTime Tarih { get; set; } = DateTime.Now; }

public class Department
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Name { get; set; } = "";
    public List<Directorate> Directorates { get; set; } = [];
}

public class Directorate
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Name { get; set; } = "";
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public List<Room> Rooms { get; set; } = [];
}

public class Room
{
    public int Id { get; set; }
    [Required, MaxLength(50)] public string RoomNumber { get; set; } = "";
    [MaxLength(150)] public string Building { get; set; } = "";
    [MaxLength(50)] public string Floor { get; set; } = "";
    public int DirectorateId { get; set; }
    public Directorate? Directorate { get; set; }
    public List<Asset> Assets { get; set; } = [];
}

public class Asset
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Name { get; set; } = "";
    [Required(ErrorMessage = "Kategori seçilmelidir."), MaxLength(100)] public string Category { get; set; } = "";
    [MaxLength(100)] public string BrandModel { get; set; } = "";
    [MaxLength(100)] public string SerialNumber { get; set; } = "";
    [MaxLength(100)] public string InventoryNumber { get; set; } = "";
    [MaxLength(100)] public string ResponsiblePerson { get; set; } = "";
    [MaxLength(120)] public string KullananPersonel { get; set; } = "";
    [MaxLength(50)] public string Status { get; set; } = "Aktif";
    [Range(1, int.MaxValue, ErrorMessage = "Oda seçilmelidir.")] public int RoomId { get; set; }
    public Room? Room { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
