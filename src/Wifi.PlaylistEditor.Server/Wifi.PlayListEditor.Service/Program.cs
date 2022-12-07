using Wifi.PlaylistEditor.Factories;
using Wifi.PlaylistEditor.Types;
using Wifi.PlayListEditor.Service.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPlaylistFactory, PlaylistFactory>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IPlaylistItemFactory, PlaylistItemFactory>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
