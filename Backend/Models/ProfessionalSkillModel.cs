using BusinessLogic.Entities;

namespace Backend.Models;

public class ProfessionalSkillModel
{
    public int ProfessionalID { get; set; }
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public decimal HourlyRate { get; set; }
    public string Visibility { get; set; }
    public User User { get; set; }
    public List<ProfessionalSkill> ProfessionalSkills { get; set; }
    public List<Experience> Experiences { get; set; }
}