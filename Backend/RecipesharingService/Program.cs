using MediatR;
using RecipesharingService.Contracts;
using RecipesharingService.MongoDB;
using RecipesharingService.Repositories;
using RecipesharingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// Register Repository and Service
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<RecipeRepository>();
builder.Services.AddSingleton<LikeRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<CommentRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<LikeService>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

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
