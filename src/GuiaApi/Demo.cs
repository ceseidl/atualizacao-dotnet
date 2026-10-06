using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace GuiaApi;

public static class Demo
{
    public const string Emissor = "guia-api";
    public const string Audiencia = "guia-clientes";

    // Valor de teste. Em producao: user-secrets ou cofre.
    public const string ChaveDeTeste =
        "chave-de-teste-com-pelo-menos-32-bytes!!";

    public static string Token(string papel) =>
        new JsonWebTokenHandler().CreateToken(
            new SecurityTokenDescriptor
            {
                Issuer = Emissor,
                Audience = Audiencia,
                Subject = new ClaimsIdentity(
                    [new Claim(ClaimTypes.Role, papel)]),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(ChaveDeTeste)),
                    SecurityAlgorithms.HmacSha256),
            });

    public static async Task ExecutarAsync(WebApplication app)
    {
        app.Urls.Add("http://127.0.0.1:0");
        await app.StartAsync();
        using var http = new HttpClient
        {
            BaseAddress = new(app.Urls.First()),
        };

        async Task Chamar(string rotulo, HttpMethod metodo,
            string rota, string? papel, object? corpo = null)
        {
            var req = new HttpRequestMessage(metodo, rota);
            if (papel is not null)
                req.Headers.Authorization =
                    new("Bearer", Token(papel));
            if (corpo is not null)
                req.Content = JsonContent.Create(corpo);
            var resp = await http.SendAsync(req);
            var codigo = (int)resp.StatusCode;
            Console.WriteLine($"{rotulo,-18} {codigo}");
        }

        var ok = new { Cliente = "Ana", Quantidade = 2 };
        var ruim = new { Cliente = "A", Quantidade = 0 };
        var (get, post, del) = (HttpMethod.Get, HttpMethod.Post,
            HttpMethod.Delete);

        await Chamar("Sem token", get, "/pedidos/1", null);
        await Chamar("POST valido", post, "/pedidos", "v", ok);
        await Chamar("POST invalido", post, "/pedidos",
            "v", ruim);
        await Chamar("GET existente", get, "/pedidos/1", "v");
        await Chamar("DELETE vendedor", del, "/pedidos/1", "v");
        await Chamar("DELETE gerente", del, "/pedidos/1",
            "gerente");

        await app.StopAsync();
    }
}
