using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class JobproposalSkill
{
    public int Jobproposalid { get; set; }

    public int Skillid { get; set; }

    public int Minyearsexperience { get; set; }

    public virtual Jobproposal Jobproposal { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
