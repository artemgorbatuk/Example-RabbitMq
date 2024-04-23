using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Example_RabbitMq.Controllers;
[ApiController]
[Route("[controller]")]
public class RabbitController : ControllerBase
{
    [HttpPost("send")]
    public IActionResult Send(object message)
    {

        var exchange = string.Empty;

        var queue = "TestQueue";

        var routingKey = "TestQueue";

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));


        var factory = new ConnectionFactory { HostName = "localhost", Port = 5762 };

        using var connection = factory.CreateConnection();

        using var model = connection.CreateModel();

        model.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

        model.BasicPublish(exchange, routingKey, basicProperties: null, body);

        return Ok();
    }
}