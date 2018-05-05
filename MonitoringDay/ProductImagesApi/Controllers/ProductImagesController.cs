using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductImagesApi.Infrastructure.Instrumentation;

namespace ProductImagesApi.Controllers
{
    [Route("products/{productCode}/images")]
    public class ProductImagesController : Controller
    {
        private readonly ImageMetrics _imageMetrics;
        private readonly IImagesRepository _imagesRepository;

        public ProductImagesController(
            ImageMetrics imageMetrics,
            IImagesRepository imagesRepository)
        {
            _imageMetrics = imageMetrics;
            _imagesRepository = imagesRepository;
        }

        // GET product/1234/images
        [HttpGet]
        public async Task<IActionResult> Get(string productCode)
        {
            var images = await _imagesRepository.GetImagesFor(productCode);

            // Increment the custom counter
            _imageMetrics.ProcessedImagesCounter.Increment();

            return Ok(new
            {
                ProductCode = productCode,
                Images = images
            });
        }
    }
}
