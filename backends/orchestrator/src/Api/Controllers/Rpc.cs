using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Collections.Concurrent;


namespace Api.Controllers;

public class RpcClient: IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly AsyncEventingBasicConsumer _consumer;
    private readonly string _replyQueueName;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

    public RpcClient(
        string replyQueueName,
        IConnection connection, 
        IChannel channel, 
        AsyncEventingBasicConsumer consumer, 
        ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper
    )
    {
        _replyQueueName = replyQueueName;
        _connection = connection;
        _channel = channel;
        _consumer = consumer;
        _callbackMapper = callbackMapper;
    }

    public static async Task<RpcClient> InitAsync()
    {
        // TODO - depois pegar isso de variáveis de ambiente
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        // fila exclusiva e temporária para a resposta
        var replyQueue = await channel.QueueDeclareAsync(
            queue: string.Empty,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        var replyQueueName = replyQueue.QueueName;

        var consumer = new AsyncEventingBasicConsumer(channel);
        var callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>{};

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);

            if (callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
            {
                tcs.SetResult(response);
            }

            await Task.Yield(); 
        };

        await channel.BasicConsumeAsync(replyQueueName, autoAck: true, consumer: consumer);

        return new RpcClient(replyQueueName, connection, channel, consumer, callbackMapper);
    }

    public async Task CallAsync(string msg)
    {
        var correlationId = Guid.NewGuid().ToString();
        var tcs = new TaskCompletionSource<string>();

        _callbackMapper[correlationId] = tcs;

        var props = new BasicProperties
        {
            CorrelationId = correlationId,
            ReplyTo = _replyQueueName
        };

        var msgBin = Encoding.UTF8.GetBytes(msg);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "fila",
            mandatory: false,
            basicProperties: props,
            body: msgBin
        );

    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}