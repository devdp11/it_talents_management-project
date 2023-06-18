using BusinessLogic.Entities;

namespace BusinessLogic.Models;

public class CreateProfessionalModel
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public decimal HourlyRate { get; set; }
    public string Visibility { get; set; }
}