
using SeventySevenDiamondsBackend.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var options = new WebApplicationOptions {
    WebRootPath = "wwwroot"
};

var builder = WebApplication.CreateBuilder(options);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options => {

    options.AddPolicy("AllowNuxtFrontend", builder => builder
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c => {

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.OperationFilter<FileUploadFilter>();
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });

});

builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

var app = builder.Build();

app.UseCors("AllowNuxtFrontend");
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run("http://localhost:5047");
