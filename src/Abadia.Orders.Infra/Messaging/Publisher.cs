using Abadia.Orders.Domain.Messaging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Abadia.Orders.Infra.Messaging;

public class Publisher : IPublisher
{
    private readonly IConnection _connection;
    private readonly IAsyncConnectionFactory _connectionFactory;
    private readonly IModel _channel;
    private readonly IConfiguration _configuration;
    private string? _exchange;
    private string? _routingKey;

    public Publisher(IConfiguration configuration)
    {
        _configuration = configuration;

        _connectionFactory = new ConnectionFactory
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

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        ConfigureChannel();
    }


    public void Publish<T>(T message) where T : class
    {
        var serializedMessage = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(serializedMessage);

        _channel.BasicPublish(_exchange, _routingKey, null, messageBytes);
    }

    private void ConfigureChannel()
    {
        var queue = _configuration.GetSection("RabbitMq:XlsQueueName").Value;

        _exchange = _configuration.GetSection("RabbitMq:XlsExchangeName").Value;
        _routingKey = _configuration.GetSection("RabbitMq:XlsRoutingKey").Value;

        _channel.QueueDeclare(queue, true, false, false, null);
        _channel.ExchangeDeclare(_exchange, ExchangeType.Topic, true, false, null);
        _channel.QueueBind(queue, _exchange, _routingKey);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}