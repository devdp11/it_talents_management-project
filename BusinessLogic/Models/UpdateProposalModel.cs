namespace Frontend.Models;

public class UpdateProposalModel
{
    public string Name { get; set; }
    
    public string TalentCategory { get; set; }
    public int TotalHours { get; set; }
    public string JobDescription { get; set; }
    public List<AddProposalSkillModel>? ProposalSkills { get; set; }
}