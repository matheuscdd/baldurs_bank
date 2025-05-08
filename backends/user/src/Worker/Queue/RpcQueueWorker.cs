using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using System.Text;
using Newtonsoft.Json;
using Domain.Exceptions;
using System.Net;

namespace Worker.Queue;

public class RpcQueueWorker: BackgroundService
{
    private readonly string QUEUE_NAME = "rpc_queue";
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly QueueConsumer _queueConsumer;

    public RpcQueueWorker(QueueConsumer queueConsumer)
    {
        _queueConsumer = queueConsumer;
    }

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

    protected override async Task ExecuteAsync(CancellationToken stopingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (object sender, BasicDeliverEventArgs ea) =>
        {
            var cons = (AsyncEventingBasicConsumer)sender;
            var ch = cons.Channel;
            var response = string.Empty;
            var statusCode = 0;

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;

            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            var message = Encoding.UTF8.GetString(body);

            try
            {
                var (rawResponse, status) = await _queueConsumer.OnMessageReceived(message);
                statusCode = status;
                response = JsonConvert.SerializeObject(rawResponse);
            }
            catch (BaseException ex)
            {
                Console.WriteLine(message);
                Console.WriteLine($" [!] Erro ao processar mensagem: {ex.Message}");
                statusCode = ex.StatusCode;
                response = JsonConvert.SerializeObject(ex.ToResponse());
            }
            catch (Exception ex)
            {
                Console.WriteLine(message);
                Console.WriteLine($" [!] Erro ao processar mensagem: {ex.Message}");
                statusCode = (int) HttpStatusCode.InternalServerError;
                response = JsonConvert.SerializeObject(new {
                        type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
                        title = "Internal Server Error",
                        status = statusCode,
                    });
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                    new {
                        Status = statusCode,
                        Payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(response))
                        }
                    ));
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

    public override async void Dispose()
    {
        if (_channel?.IsOpen == true)
            await _channel.CloseAsync();
        
        if (_connection?.IsOpen == true)
            await _connection.CloseAsync();
        
        base.Dispose();
    }
}


