using LibraryDomain.Model;

namespace LibraryInfrastructure.Services
{
    public interface IExportService<TEntity>
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }

}
