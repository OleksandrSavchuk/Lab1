using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Model;
using LibraryInfrastructure;
using static LibraryInfrastructure.DblibraryContext;
using System.Numerics;
using System.IO;
using NuGet.Packaging;
using ClosedXML.Excel;


namespace LibraryInfrastructure.Controllers
{
    public class BooksController : Controller
    {
        private readonly DblibraryContext _context;

        public BooksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var book = await _context.Books
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var genres = book.Genres;
            var authors = book.Authors;


            return View(book);
        }


        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "LastName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,PublishedYear")] Book book, int[] authors, int[] genres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);

                if (authors != null)
                {
                    var selectedAuthors = await _context.Authors.Where(a => authors.Contains(a.AuthorId)).ToListAsync();
                    book.Authors.AddRange(selectedAuthors);
                }

                if (genres != null)
                {
                    var selectedGenres = await _context.Genres.Where(g => genres.Contains(g.GenreId)).ToListAsync();
                    book.Genres.AddRange(selectedGenres);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "LastName");

            return View(book);
        }


        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,PublishedYear")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books
                .Include(b => b.Authors).Include(g => g.Genres)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            // Видалення книги зі списку авторів, не видаляючи авторів
            foreach (var author in book.Authors)
            {
                author.Books.Remove(book);
            }

            book.Genres.Clear();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (fileExcel == null || fileExcel.Length == 0)
            {
                ModelState.AddModelError("", "Please select an Excel file.");
                return View();
            }

            if (!Path.GetExtension(fileExcel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Please select an Excel file.");
                return View();
            }

            using (var stream = new MemoryStream())
            {
                await fileExcel.CopyToAsync(stream);
                using (var workBook = new XLWorkbook(stream))
                {
                    var worksheet = workBook.Worksheets.First();
                    var books = new List<Book>();
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var title = row.Cell(1).Value.ToString();
                        int publishedYear;
                        if (!int.TryParse(row.Cell(2).Value.ToString(), out publishedYear))
                        {
                            ModelState.AddModelError("", "Invalid published year format in Excel file.");
                            return View();
                        }

                        var genresString = row.Cell(3).Value.ToString();
                        var genresArray = genresString.Split(',').Select(genre => genre.Trim()).ToArray();

                        var authorsString = row.Cell(4).Value.ToString();
                        var authorsArray = authorsString.Split(',').Select(author => author.Trim()).ToArray();

                        var authors = new List<Author>();
                        foreach (var authorName in authorsArray)
                        {
                            var authorNames = authorName.Split(' ');
                            if (authorNames.Length == 2)
                            {
                                var firstName = authorNames[0];
                                var lastName = authorNames[1];

                                var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName);
                                if (existingAuthor != null)
                                {
                                    authors.Add(existingAuthor);
                                }
                                else
                                {
                                    authors.Add(new Author { FirstName = firstName, LastName = lastName });
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Invalid author name format in Excel file.");
                                return View();
                            }
                        }

                        var genres = new List<Genre>();
                        foreach (var genreName in genresArray)
                        {
                            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreName == genreName);
                            if (existingGenre != null)
                            {
                                genres.Add(existingGenre);
                            }
                            else
                            {
                                genres.Add(new Genre { GenreName = genreName });
                            }
                        }

                        var book = new Book
                        {
                            Title = title,
                            PublishedYear = publishedYear,
                            Authors = authors,
                            Genres = genres
                        };

                        books.Add(book);
                    }


                    await _context.Books.AddRangeAsync(books);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Export()
        {
            using (var workbook = new XLWorkbook())
            {
                var books = await _context.Books.Include(book => book.Genres).Include(book => book.Authors).ToListAsync();

                var worksheet = workbook.Worksheets.Add("Books");

                worksheet.Cell("A1").Value = "Title";
                worksheet.Cell("B1").Value = "PublishedYear";
                worksheet.Cell("C1").Value = "Genres";
                worksheet.Cell("D1").Value = "Authors";

                worksheet.Row(1).Style.Font.Bold = true;

                int row = 2;
                foreach (var book in books)
                {
                    worksheet.Cell(row, 1).Value = book.Title;
                    worksheet.Cell(row, 2).Value = book.PublishedYear;
                    string genresString = string.Join(", ", book.Genres.Select(genre => genre.GenreName));
                    worksheet.Cell(row, 3).Value = genresString;
                    string authorsString = string.Join(", ", book.Authors.Select(author => $"{author.FirstName} {author.LastName}"));
                    worksheet.Cell(row, 4).Value = authorsString;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Books.xlsx");
                }
            }
        }
    }
}
