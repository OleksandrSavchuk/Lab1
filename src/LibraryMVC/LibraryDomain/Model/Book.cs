using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Book : Entity
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int? PublishedYear { get; set; }

    public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();

    public virtual ICollection<GenreBook> GenreBooks { get; set; } = new List<GenreBook>();

    public virtual ICollection<ReaderBook> ReaderBooks { get; set; } = new List<ReaderBook>();
}
