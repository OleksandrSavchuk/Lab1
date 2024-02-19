using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Genre : Entity
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual ICollection<GenreBook> GenreBooks { get; set; } = new List<GenreBook>();
}
