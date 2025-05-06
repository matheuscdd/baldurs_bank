using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace Api.Controllers;

public class Worker: BackgroundService
{
    private readonly string QUEUE_NAME = "rcp_queue";
    private IConnection? _connection;
    private IChannel? _channel;


    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory{HostName = "localhost"};
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        await _channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false
        );

        await base.StartAsync(cancellationToken);

    }

    protected  override async Task ExecuteAsync(CancellationToken stopingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (object sender, BasicDeliverEventArgs ea) =>
        {
            var cons = (AsyncEventingBasicConsumer)sender;
            var ch = cons.Channel;
            var response = string.Empty;

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;

            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            try
            {
                var message = Encoding.UTF8.GetString(body);
                var n = int.Parse(message);
                Console.WriteLine($" [.] Fib({message})");
                response = Fib(n).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine($" [.] {e.Message}");
                response = string.Empty;
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await ch.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: props.ReplyTo,
                    mandatory: true,
                    basicProperties: replyProps,
                    body: responseBytes
                );
                await ch.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        };

        await _channel!.BasicConsumeAsync(QUEUE_NAME, false, consumer);

        while (!stopingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stopingToken);
        }
    } 

    public static int Fib(int n)
    {
        if (n is 0 or 1)
        {
            return n;
        }
        return Fib(n - 1) + Fib(n - 2);
    }

    public override async void Dispose()
    {
        if (_channel?.IsOpen == true)
            await _channel.CloseAsync();
        
        if (_connection?.IsOpen == true)
            await _connection.CloseAsync();
        
        base.Dispose();
    }
}


