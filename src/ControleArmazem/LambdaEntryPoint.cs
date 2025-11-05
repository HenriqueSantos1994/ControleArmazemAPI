using Amazon.Lambda.AspNetCoreServer;

namespace ControleArmazem;

public class LambdaEntryPoint : APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseStartup<Startup>();
    }
}