using BusinessLogic.Entities;

namespace Backend.Models;

public class JobProposalModel
{
    public int JobProposalID { get; set; }
    public int UserID { get; set; }
    public int ClientID { get; set; }
    public string Name { get; set; }
    public string TalentCategory { get; set; }
    public int TotalHours { get; set; }
    public string JobDescription { get; set; }
    public UserModel User { get; set; }
    public ClientModel Client { get; set; }
    public List<JobProposalSkillModel> JobproposalSkill { get; set; }
}