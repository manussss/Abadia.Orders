using Abadia.Orders.Application.Contracts;
using Abadia.Orders.Shared.IntegrationEvents;
using Abadia.Orders.Worker.XlsConverter.Application;
using Abadia.Orders.Worker.XlsConverter.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Abadia.Orders.Worker.XlsConverter;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ISubscriber _subscriberCommand;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, 
        IServiceScopeFactory serviceScopeFactory, 
        ISubscriber subscriberCommand, 
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _subscriberCommand = subscriberCommand;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var queue = _configuration.GetSection("RabbitMq:XlsQueueName").Value;
            var channel = _subscriberCommand.Channel;
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ProcessMessageAsync;

            //TODO autoack = false
            //TODO deadletter
            channel.BasicConsume(queue, true, consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs args)
    {
        try
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());

            _logger.LogInformation("Processing message: {message}", message);

            var integrationEvent = JsonSerializer.Deserialize<MessageContract<UploadXlsFinishedIntegrationEvent>>(message);

            if (integrationEvent != null)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new ConversorCommand(integrationEvent.Data.OrderUploadId));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred.");
            throw;
        }
    }
}