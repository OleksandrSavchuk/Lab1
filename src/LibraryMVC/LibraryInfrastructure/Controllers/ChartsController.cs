using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LibraryInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private record CountByYearResponseItem(string Year, int Count);

        private readonly DblibraryContext libraryContext;

        public ChartsController(DblibraryContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        [HttpGet("countByYear")]
        public async Task<JsonResult> GetCountByYearAsync(CancellationToken cancellationToken)
        {
            var responseItems = await libraryContext
                .Books
                .GroupBy(book => book.PublishedYear)
                .Select(group => new CountByYearResponseItem(group.Key.ToString(), group.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }

        private record CountByAuthorResponseItem(string Author, int Count);

        [HttpGet("countByAuthor")]
        public async Task<JsonResult> GetCountByAuthorsAsync(CancellationToken cancellationToken)
        {
            var responseItems = await libraryContext
                .Books
                .SelectMany(book => book.Authors)
                .GroupBy(author => author)
                .Select(group => new CountByAuthorResponseItem(group.Key.LastName, group.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }


    }
}
