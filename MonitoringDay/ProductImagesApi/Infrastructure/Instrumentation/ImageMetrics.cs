using Nexogen.Libraries.Metrics;

namespace ProductImagesApi.Infrastructure.Instrumentation
{
    public class ImageMetrics
    {
        public ImageMetrics(IMetrics metrics)
        {
            ProcessedImagesCounter = metrics.Counter()
                .Name("processed_images_total")
                .Help("Total number of processed images.")
                .Register();
        }

        public ICounter ProcessedImagesCounter { get; protected set; }
    }
}
