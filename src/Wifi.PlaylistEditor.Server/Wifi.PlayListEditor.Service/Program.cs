using Wifi.PlayList.Editor.DbRepositories.MongoDbEntities;
using Wifi.PlayList.Editor.DbRepositories;
using Wifi.PlaylistEditor.Factories;
using Wifi.PlaylistEditor.Types;
using Wifi.PlayListEditor.Service.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PlaylistDbSettings>(
    builder.Configuration.GetSection("PlaylistDbSettings"));

// Add services to the container.
builder.Services.AddScoped<IPlaylistFactory, PlaylistFactory>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IPlaylistItemFactory, PlaylistItemFactory>();
builder.Services.AddScoped<IDatabaseRepository<PlaylistEntity, PlaylistItemEntity>, MongoDbRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();


