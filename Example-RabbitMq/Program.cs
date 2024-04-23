using Example_RabbitMq.Services;

namespace Example_RabbitMq;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // 
        builder.Services.AddHostedService<RabbitService>();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
