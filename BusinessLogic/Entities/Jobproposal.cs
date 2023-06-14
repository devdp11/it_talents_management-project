using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Jobproposal
{
    public int Jobproposalid { get; set; }

    public int? Userid { get; set; }

    public int? Clientid { get; set; }

    public string Name { get; set; } = null!;

    public string Talentcategory { get; set; } = null!;

    public int Totalhours { get; set; }

    public string Jobdescription { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual ICollection<JobproposalSkill> JobproposalSkills { get; set; } = new List<JobproposalSkill>();

    public virtual User? User { get; set; }
}
