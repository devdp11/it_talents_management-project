using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class User
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Jobproposal> Jobproposals { get; set; } = new List<Jobproposal>();

    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();
}
