using BusinessLogic.Entities;

namespace BusinessLogic.Models;

public class ExperienceModel
{
    public int ExperienceID { get; set; }
    public int ProfessionalID { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public ProfessionalModel Professional { get; set; }
}