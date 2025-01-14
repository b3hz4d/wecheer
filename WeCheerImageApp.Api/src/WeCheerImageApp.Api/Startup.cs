using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WeCheerImageApp.Api.Services;
using Amazon.Kinesis;

namespace WeCheerImageApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            // CORS configuration
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "WeCheer Image Event API", 
                    Version = "v1",
                    Description = "API for handling image events"
                });
            });

            // AWS service configuration
            services.AddAWSService<IAmazonKinesis>();

            // Service registration
            services.AddSingleton<IImageEventService, ImageEventService>();
            
            // Kinesis stream configuration
            services.AddSingleton<KinesisEventProcessor>(sp =>
            {
                var kinesisClient = sp.GetRequiredService<IAmazonKinesis>();
                var logger = sp.GetRequiredService<ILogger<KinesisEventProcessor>>();
                var streamName = Configuration["KinesisStreamName"] ?? 
                    Environment.GetEnvironmentVariable("KinesisStreamName") ?? 
                    throw new InvalidOperationException("KinesisStreamName is not configured");
                
                return new KinesisEventProcessor(kinesisClient, streamName, logger);
            });

            // Background service registration
            services.AddHostedService<KinesisBackgroundService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            // API documentation UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeCheer Image Event API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}