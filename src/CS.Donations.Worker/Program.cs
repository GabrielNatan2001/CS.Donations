using CS.Donations.Application;
using CS.Donations.Infrastructure;
using CS.Donations.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.Configure<DoacaoRecebidaWorkerConfig>(builder.Configuration.GetSection("Workers:DoacaoRecebida"));
builder.Services.AddHostedService<DoacaoRecebidaWorker>();

var host = builder.Build();
host.Run();
