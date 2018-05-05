using System.Threading.Tasks;

namespace ProductImagesApi.Controllers
{
    public class InMemoryImagesRepository : IImagesRepository
    {
        public Task<string[]> GetImagesFor(string productCode)
        {
            var images = new[]
            {
                "https://www.adidas.es/dis/dw/image/v2/aagl_prd/on/demandware.static/-/Sites-adidas-products/default/dw68222d48/zoom/CP9248_01_standard.jpg?sh=840&strip=false&sw=840",
                "https://www.adidas.es/dis/dw/image/v2/aagl_prd/on/demandware.static/-/Sites-adidas-products/default/dw68222d48/zoom/CP9248_01_standard.jpg?sh=60&strip=false&sw=60&q=97",
                "https://www.adidas.es/dis/dw/image/v2/aagl_prd/on/demandware.static/-/Sites-adidas-products/default/dwa20eb5e2/zoom/CP9248_02_hover_frv.jpg?sh=60&strip=false&sw=60&q=97",
                "https://www.adidas.es/dis/dw/image/v2/aagl_prd/on/demandware.static/-/Sites-adidas-products/default/dw90fb684f/zoom/CP9248_02_standard.jpg?sh=60&strip=false&sw=60&q=97"
            };

            return Task.FromResult(images);
        }
    }
}