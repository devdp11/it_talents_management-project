namespace Frontend.Models;

public class UpdateProfessionalModel
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public decimal HourlyRate { get; set; }
    public bool Visibility { get; set; }
    public List<AddProfessionalSkillModel>? ProfessionalSkills { get; set; }
    public List<AddExperienceModel>? Experiences { get; set; }
}