using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WeCheerImageApp.Api.Services
{
    public class KinesisBackgroundService : BackgroundService
    {
        private readonly KinesisEventProcessor _processor;
        private readonly ILogger<KinesisBackgroundService> _logger;

        public KinesisBackgroundService(
            KinesisEventProcessor processor,
            ILogger<KinesisBackgroundService> logger)
        {
            _processor = processor;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Starting Kinesis stream processing");
                await _processor.ProcessStreamAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing Kinesis stream");
                throw;
            }
        }
    }
} 