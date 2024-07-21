using Microsoft.Extensions.Options;
using util.API.Repository;
using util.API.Service;
using util.API.Utility.Configuration;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //set config
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    var configuration = config.Build();
    builder.Services.Configure<MongoDbConfig>(configuration.GetSection("MongoDbConfig"));

    // Add services to the container.
    builder.Services.AddScoped<ILedgerService, LedgerService>();
    builder.Services.AddScoped<ILedgerRepository, LedgerRepository>();


    builder.Services.AddHttpClient(Options.DefaultName, c =>
    {
    }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) => true
        };
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddMvc().AddNewtonsoftJson();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI();
    //}

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
    );

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    //log
}
finally
{
    //log
}