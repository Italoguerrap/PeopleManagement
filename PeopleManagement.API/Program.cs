using FluentValidation;
using FluentValidation.AspNetCore;
using PeopleManagement.API.Middlewares;
using PeopleManagement.API.PipelineExtensions;
using PeopleManagement.API.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddVersioning()
       .AddInfrastructure(builder.Configuration)
       .AddApplication()
       .AddAutoMapper(Assembly.GetExecutingAssembly())
       .AddCorsPolicy(builder.Configuration)
       .AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<GetPeopleQueryRequestValidator>();

var app = builder.Build();

app.UseCors("AllowLocalhostFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();