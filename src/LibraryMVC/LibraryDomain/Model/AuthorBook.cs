using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class AuthorBook : Entity
{
    public int AuthorBookId { get; set; }

    public int? AuthorId { get; set; }

    public int? BookId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Book? Book { get; set; }
}
