using BusinessLogic.Entities;

namespace Backend.Models;

public class ProfessionalModel
{
    public int ProfessionalID { get; set; }
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public decimal HourlyRate { get; set; }
    public string Visibility { get; set; }
    public UserModel User { get; set; }
    public List<ProfessionalSkillModel> ProfessionalSkills { get; set; }
    public List<ExperienceModel> Experiences { get; set; }
}