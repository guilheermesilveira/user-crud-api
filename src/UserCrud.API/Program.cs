using UserCrud.API.Configuration;
using UserCrud.Application;
using UserCrud.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationConfig(builder);

builder.Services.AddInfraDataConfig(builder.Configuration);

builder.Services.AddSwaggerConfig();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("default");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();