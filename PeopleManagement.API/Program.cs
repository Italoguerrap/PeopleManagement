using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PeopleManagement.API.Middlewares;
using PeopleManagement.API.PipelineExtensions;
using PeopleManagement.API.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "People Management API", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "People Management API", Version = "v2" });

    // Filtra controllers com base no grupo
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (apiDesc.GroupName == null)
            return false;

        return apiDesc.GroupName.Equals(docName, StringComparison.OrdinalIgnoreCase);
    });

});


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.AddVersioning()
       .AddInfrastructure(builder.Configuration)
       .AddApplication()
       .AddAutoMapper(Assembly.GetExecutingAssembly())
       .AddCorsPolicy(builder.Configuration)
       .AddFluentValidationAutoValidation()
       .AddJwtAutentication(builder.Configuration)
       ;

builder.Services.AddValidatorsFromAssemblyContaining<GetPeopleQueryRequestValidator>();

var app = builder.Build();

app.UseCors("AllowLocalhostFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "People API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "People API V2");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();