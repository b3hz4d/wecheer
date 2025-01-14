using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System.Text.Json;
using WeCheerImageApp.Api.Models;

namespace WeCheerImageApp.Api.Services
{
    /// <summary>
    /// Processes events from a Kinesis stream
    /// </summary>
    public class KinesisEventProcessor
    {
        private readonly IAmazonKinesis _kinesisClient;
        private readonly string _streamName;
        private readonly ILogger<KinesisEventProcessor> _logger;

        public KinesisEventProcessor(IAmazonKinesis kinesisClient, string streamName, ILogger<KinesisEventProcessor> logger)
        {
            _kinesisClient = kinesisClient;
            _streamName = streamName;
            _logger = logger;
        }

        public async Task ProcessStreamAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var shardIteratorRequest = new GetShardIteratorRequest
                {
                    StreamName = _streamName,
                    ShardId = "shardId-000000000000", // Default shard ID for single-shard streams
                    ShardIteratorType = ShardIteratorType.LATEST
                };

                var shardIteratorResponse = await _kinesisClient.GetShardIteratorAsync(shardIteratorRequest, cancellationToken);
                var shardIterator = shardIteratorResponse.ShardIterator;

                while (!cancellationToken.IsCancellationRequested)
                {
                    var recordsRequest = new GetRecordsRequest
                    {
                        ShardIterator = shardIterator,
                        Limit = 100
                    };

                    var recordsResponse = await _kinesisClient.GetRecordsAsync(recordsRequest, cancellationToken);
                    
                    foreach (var record in recordsResponse.Records)
                    {
                        try
                        {
                            using var memoryStream = new MemoryStream(record.Data.ToArray());
                            var imageEvent = await JsonSerializer.DeserializeAsync<ImageEvent>(memoryStream, cancellationToken: cancellationToken);
                            
                            if (imageEvent != null)
                            {
                                _logger.LogInformation("Processed image event: {ImageUrl}", imageEvent.ImageUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing Kinesis record");
                        }
                    }

                    shardIterator = recordsResponse.NextShardIterator;
                    await Task.Delay(1000, cancellationToken); // Rate limiting
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Kinesis stream");
                throw;
            }
        }
    }
} 