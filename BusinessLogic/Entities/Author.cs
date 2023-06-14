using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Author
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
