using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Role
{
    public int Roleid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
