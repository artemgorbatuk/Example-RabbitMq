using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Example_RabbitMq.Services;

public class RabbitService : BackgroundService
{
    private IConnection connection;
    private IModel model;
    private string queue = "TestQueue";
    public RabbitService()
    {
        var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
        connection = factory.CreateConnection();
        model = connection.CreateModel();
        model.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(model);
        consumer.Received += Consume;
        model.BasicConsume(queue, false, consumer);
        return Task.CompletedTask;
    }

    private void Consume(object? sender, BasicDeliverEventArgs e)
    {
        var body = Encoding.UTF8.GetString(e.Body.ToArray());
        model.BasicAck(e.DeliveryTag, false);    
    }

    public override void Dispose()
    {
        model.Close();
        connection.Close();
        base.Dispose();
    }
}