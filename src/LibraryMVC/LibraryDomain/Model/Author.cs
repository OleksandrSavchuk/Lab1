using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Model;

public partial class Author
{

    public int AuthorId { get; set; }
    [Required(ErrorMessage = "Введіть щось")]
    [Display(Name = "Ім'я")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "Введіть щось")]
    [Display(Name = "Прізвище")]
    public string LastName { get; set; } = null!;
    [Display(Name = "Дата народження")]
    public DateOnly? Birthday { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}