using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Services.TaskServices;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.Repository.Common;
using TaskManagement.Infrastructure.Repository.TaskRepository;
using TaskManagement.API.Converters;
using TaskManagement.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TaskManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManagementDB"));
});

builder.Services.AddSingleton<IReactiveTaskQueue, ReactiveTaskQueue>();
builder.Services.AddHostedService<ReactiveTaskProcessorService>();

builder.Services.AddScoped<ICommonProcess<Tareas>, TaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/*
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskManagementContext>();
    context.Database.Migrate();
}*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
