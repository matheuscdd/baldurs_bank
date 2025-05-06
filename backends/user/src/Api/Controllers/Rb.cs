using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace Api.Controllers;

public class RabbitMqService
{
    private readonly string _host = "localhost";
    private readonly string _queueName = "fila";
    private readonly string _userName = "guest";
    private readonly string _password = "guest";

    public RabbitMqService()
    {

    }

    public async Task Read()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _host,
            UserName = _userName,
            Password = _password
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var msg = Encoding.UTF8.GetString(body);
            Console.WriteLine(msg);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(_queueName, autoAck: true, consumer: consumer);
    }

    public async Task Send(string msg)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _host,
            UserName = _userName,
            Password = _password
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(msg);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueName,
            body: body
        );

        Console.WriteLine("Foi?");
    }
}
