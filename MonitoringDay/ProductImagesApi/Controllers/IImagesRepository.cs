using System.Threading.Tasks;

namespace ProductImagesApi.Controllers
{
    public interface IImagesRepository
    {
        Task<string[]> GetImagesFor(string productCode);
    }
}