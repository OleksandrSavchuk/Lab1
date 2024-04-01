using DocumentFormat.OpenXml.Vml.Office;
using LibraryDomain.Model;

namespace LibraryInfrastructure.Services
{
    public interface IDataPortServiceFactory<TEntity>
    {
        IImportService<TEntity> GetImportService(string contentType);
        IExportService<TEntity> GetExportService(string contentType);
    }

}
