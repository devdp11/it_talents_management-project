using BusinessLogic.Entities;

namespace BusinessLogic.Models;

public class ProfessionalSkillModel
{
    public int ProfessionalID { get; set; }
    public int SkillID { get; set; }
    public int YearsExperience { get; set; }
    public ProfessionalModel Professional { get; set; }
    public SkillModel Skill { get; set; }
}