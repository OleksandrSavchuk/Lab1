using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Author
{
    public int AuthorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
