using System;
using ControleArmazem;

var builder = WebApplication.CreateBuilder(args);

// Delegar configuração de serviços para Startup
var startup = new Startup();
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Delegar configuração do pipeline para Startup
startup.Configure(app, app.Environment);

// Não iniciar Kestrel quando executando dentro do Lambda (ou do Lambda Test Tool).
var runningInLambda = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME"))
    || string.Equals(Environment.GetEnvironmentVariable("LAMBDA_TEST_TOOL"), "true", StringComparison.OrdinalIgnoreCase);

if (!runningInLambda)
{
    app.Run();
}
