using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Book
{

    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int? PublishedYear { get; set; }

    public virtual ICollection<ReaderBook> ReaderBooks { get; set; } = new List<ReaderBook>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
