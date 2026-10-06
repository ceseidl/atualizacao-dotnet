using System.Text;
using GuiaApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
if (args.Contains("--demo"))
    builder.Logging.ClearProviders();

builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddSingleton<PedidoRepositorio>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new()
    {
        ValidIssuer = Demo.Emissor,
        ValidAudience = Demo.Audiencia,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Demo.ChaveDeTeste)),
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Gerente", p => p.RequireRole("gerente"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapOpenApi();

var pedidos = app.MapGroup("/pedidos").RequireAuthorization();

pedidos.MapPost("/", (NovoPedido novo, PedidoRepositorio repo) =>
{
    var criado = repo.Adicionar(novo);
    return TypedResults.Created($"/pedidos/{criado.Id}", criado);
});

pedidos.MapGet("/{id:int}", (int id, PedidoRepositorio repo) =>
    repo.Obter(id) is { } p
        ? Results.Ok(p)
        : Results.NotFound());

pedidos.MapDelete("/{id:int}", (int id) => Results.NoContent())
    .RequireAuthorization("Gerente");

if (args.Contains("--demo"))
    await Demo.ExecutarAsync(app);
else
    app.Run();
