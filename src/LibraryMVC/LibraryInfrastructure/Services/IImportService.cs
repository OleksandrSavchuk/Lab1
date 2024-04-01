using LibraryDomain.Model;

namespace LibraryInfrastructure.Services
{
    public interface IImportService<TEntity>
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }
}
