using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class GenreBook : Entity
{
    public int GenreBookId { get; set; }

    public int? GenreId { get; set; }

    public int? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Genre? Genre { get; set; }
}
