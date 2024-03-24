using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Model;

public partial class Book
{

    public int BookId { get; set; }
    [Required(ErrorMessage = "Введіть щось")]
    [Display(Name = "Книга")]
    public string Title { get; set; } = null!;
    [Display(Name = "Рік")]
    [Range(1, 2024, ErrorMessage = "Please enter a valid year between 0 and 2024.")]
    public int? PublishedYear { get; set; }

    public virtual ICollection<ReaderBook> ReaderBooks { get; set; } = new List<ReaderBook>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
        