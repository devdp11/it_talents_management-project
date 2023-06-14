using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class ProfessionalSkill
{
    public int Professionalid { get; set; }

    public int Skillid { get; set; }

    public int Yearsexperience { get; set; }

    public virtual Professional Professional { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
