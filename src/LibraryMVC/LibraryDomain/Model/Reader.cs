using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Reader : Entity
{
    public int ReaderId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<ReaderBook> ReaderBooks { get; set; } = new List<ReaderBook>();
}
