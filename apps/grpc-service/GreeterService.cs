using Grpc.Core;
namespace grpc_service;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override async Task KeepTalking(
        HelloRequest request, 
        IServerStreamWriter<HelloReply> responseStream,
        ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Connected");

            var response = new HelloReply
            {
                Message = "Hello " + request.Name
            };

            await responseStream.WriteAsync(response);

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), context.CancellationToken);
                response.Message = $"I will keep talking to you {request.Name} - {DateTime.Now}";

                await responseStream.WriteAsync(response);
            }
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case TaskCanceledException: break;
                default:
                    _logger.LogError(ex, "Unknown error happened");
                    break;
            }
        }
        finally
        {
            _logger.LogInformation("Disconnected.");
        }
    }
}
