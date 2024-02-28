using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Model;

public partial class Genre
{
    public int GenreId { get; set; }

    [Required(ErrorMessage = "Введіть щось")]
    [Display(Name = "Жанр")]
    public string GenreName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
