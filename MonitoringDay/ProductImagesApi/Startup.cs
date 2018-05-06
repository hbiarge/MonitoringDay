using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nexogen.Libraries.Metrics.Prometheus.AspCore;
using ProductImagesApi.Controllers;
using ProductImagesApi.Infrastructure.Instrumentation;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace ProductImagesApi
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }

        public ILoggerFactory LoggerFactory { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Health checks services
            services.AddBeatPulse();

            // Prometheus services
            services.AddPrometheus();
            services.AddSingleton<ImageMetrics>();

            services.AddScoped<IImagesRepository, InMemoryImagesRepository>();

            services.AddMvcCore()
                .AddJsonFormatters()
                .AddDataAnnotations();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Register zipkin tracer hooks
            appLifetime.ApplicationStarted.Register(StartZipkinTracer);
            appLifetime.ApplicationStopped.Register(StopZipkinTracer);

            // Prometheus middleware: listen at /metrics endpoint
            app.UsePrometheus(options => options.CollectHttpMetrics());

            app.UseTracing("Product-Images-Service");

            app.UseMvc();
        }

        private void StartZipkinTracer()
        {
            var zipkinCollector = Configuration.GetValue<string>("Zipkin:Collector");

            var logger = new TracingLogger(LoggerFactory, "zipkin4net");
            var httpSender = new HttpZipkinSender(zipkinCollector, "application/json");
            var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());

            TraceManager.SamplingRate = 1.0f;
            TraceManager.RegisterTracer(tracer);

            TraceManager.Start(logger);
        }

        private void StopZipkinTracer()
        {
            TraceManager.Stop();
        }
    }
}
