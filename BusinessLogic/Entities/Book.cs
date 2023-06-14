using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public int PublicationYear { get; set; }

    public Guid? AuthorId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Author? Author { get; set; }
}
