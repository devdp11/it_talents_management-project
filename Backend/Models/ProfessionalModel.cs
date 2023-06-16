using BusinessLogic.Entities;

namespace Backend.Models;

public class ProfessionalModel
{
    public int ProfessionalID { get; set; }
    public int SkillID { get; set; }
    public int YearsExperience { get; set; }
    public Professional Professional { get; set; }
    public Skill Skill { get; set; }
}