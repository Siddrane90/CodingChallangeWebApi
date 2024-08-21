using CodingChallangeWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var allowSpecificOriginsPolicy = "allowSpecificOriginsPolicy";
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOriginsPolicy,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                option.ReportApiVersions = true;
            });
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

var app = builder.Build();
app.UseCors(allowSpecificOriginsPolicy);

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
