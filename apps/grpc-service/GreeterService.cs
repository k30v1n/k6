using Grpc.Core;
namespace grpc_service;
public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override async Task KeepTalking(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        var response = new HelloReply
        {
            Message = "Hello " + request.Name
        };

        await responseStream.WriteAsync(response);
        
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            
            response.Message = $"I will keep talking to you {request.Name} - {DateTime.Now}";
            await responseStream.WriteAsync(response);
        }
    }
}
