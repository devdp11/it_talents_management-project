using BusinessLogic.Entities;

namespace BusinessLogic.Models;

public class AddProfessionalSkillModel
{
    public int ProfessionalID { get; set; }
    public int SkillID { get; set; }
    public int YearsExperience { get; set; }
}