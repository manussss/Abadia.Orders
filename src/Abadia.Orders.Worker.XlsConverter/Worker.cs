using Abadia.Orders.Application.Contracts;
using Abadia.Orders.Shared.IntegrationEvents;
using Abadia.Orders.Worker.XlsConverter.Application;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Abadia.Orders.Worker.XlsConverter;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            // TODO criar uma classe que tenha isso e utilizar em ambos producer e consumer, passando
            // a IConfiguration e recebendo um IModel
            IAsyncConnectionFactory factory = new ConnectionFactory
            {
                UserName = _configuration.GetSection("RabbitMq:UserName").Value,
                Password = _configuration.GetSection("RabbitMq:Password").Value,
                HostName = _configuration.GetSection("RabbitMq:HostName").Value,
                Port = int.Parse(_configuration.GetSection("RabbitMq:Port").Value),
                VirtualHost = _configuration.GetSection("RabbitMq:VirtualHost").Value,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(300),
                DispatchConsumersAsync = true
            };

            var queue = _configuration.GetSection("RabbitMq:XlsQueueName").Value;
            //var exchange = _configuration.GetSection("RabbitMq:XlsExchangeName").Value;

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //channel.ExchangeDeclare(exchange, ExchangeType.Topic, true, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ProcessMessageAsync;
            
            channel.BasicConsume(queue, true, consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs args)
    {
        try
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
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