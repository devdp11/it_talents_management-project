using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Client
{
    public int Clientid { get; set; }

    public int? Userid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Jobproposal> Jobproposals { get; set; } = new List<Jobproposal>();

    public virtual User? User { get; set; }
}
