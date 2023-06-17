
using BusinessLogic.Entities;

namespace Backend.Models;

public class JobProposalSkillModel
{
    public int JobProposalID { get; set; }
    public int SkillID { get; set; }
    public int MinYearsExperience { get; set; }
    public SkillModel Skill { get; set; }
    public JobProposalModel Jobproposal { get; set; }
}
