using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Professional
{
    public int Professionalid { get; set; }

    public int? Userid { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal Hourlyrate { get; set; }

    public bool? Visibility { get; set; } = null!;

    public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();

    public virtual ICollection<ProfessionalSkill> ProfessionalSkills { get; set; } = new List<ProfessionalSkill>();

    public virtual User? User { get; set; }
}
