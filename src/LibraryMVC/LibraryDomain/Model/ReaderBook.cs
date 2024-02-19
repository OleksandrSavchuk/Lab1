using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class ReaderBook : Entity
{
    public int ReaderBookId { get; set; }

    public int? ReaderId { get; set; }

    public int? BookId { get; set; }

    public DateOnly? BorrowDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Reader? Reader { get; set; }
}
