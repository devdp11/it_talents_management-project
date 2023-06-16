
using BusinessLogic.Entities;

namespace Backend.Models;

public class JobProposalSkillModel
{
    public int JobProposalID { get; set; }
    public int SkillID { get; set; }
    public int MinYearsExperience { get; set; }
    public Skill Skill { get; set; }
    public Jobproposal Jobproposal { get; set; }
}
