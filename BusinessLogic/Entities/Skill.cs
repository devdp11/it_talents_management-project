using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Skill
{
    public int Skillid { get; set; }

    public string Name { get; set; } = null!;

    public string Professionalarea { get; set; } = null!;

    public virtual ICollection<JobproposalSkill> JobproposalSkills { get; set; } = new List<JobproposalSkill>();

    public virtual ICollection<ProfessionalSkill> ProfessionalSkills { get; set; } = new List<ProfessionalSkill>();
}
