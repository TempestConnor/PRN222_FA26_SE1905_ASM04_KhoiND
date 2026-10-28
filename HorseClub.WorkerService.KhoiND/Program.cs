using HorseClub.Services.KhoiND;
using HorseClub.WorkerService.KhoiND;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection("Worker"));
builder.Services.AddScoped<IHorseServiceKhoiND, HorseServiceKhoiND>();
builder.Services.AddScoped<IOwnerServiceKhoiND, OwnerServiceKhoiND>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
