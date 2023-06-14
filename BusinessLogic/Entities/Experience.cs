using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Experience
{
    public int Experienceid { get; set; }

    public int? Professionalid { get; set; }

    public string Title { get; set; } = null!;

    public string Company { get; set; } = null!;

    public int Startyear { get; set; }

    public int? Endyear { get; set; }

    public virtual Professional? Professional { get; set; }
}
