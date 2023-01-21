using Abadia.Orders.Application.Contracts;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Abadia.Orders.Application.MessageProducer;

public class ProducerClient<T> : IProducerClient<T>
{
    private readonly IAsyncConnectionFactory _connectionFactory;
    private readonly IConfiguration _configuration;

    public ProducerClient(IConfiguration configuration)
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
    }

    //TODO ASYNC
    //TODO REFACTOR
    public void SendXlsMessage(MessageContract<T> messageContract)
    {
        var serializedMessage = JsonSerializer.Serialize(messageContract);
        var messageBytes = Encoding.UTF8.GetBytes(serializedMessage);

        var queue = _configuration.GetSection("RabbitMq:XlsQueueName").Value;
        var exchange = _configuration.GetSection("RabbitMq:XlsExchangeName").Value;
        var routingKey = _configuration.GetSection("RabbitMq:XlsRoutingKey").Value;

        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue, true, false, false, null);
        channel.ExchangeDeclare(exchange, ExchangeType.Topic, true, false, null);
        channel.QueueBind(queue, exchange, routingKey);

        channel.BasicPublish(exchange, routingKey, null, messageBytes);
    }
}