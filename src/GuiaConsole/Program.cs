using GuiaConsole;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("1. Fundamentos de C# 14");
var relogio = new Relogio();
Pedido[] pedidos =
[
    new(1, Status.Pago, [new("Suco", 2, 8m)]),
    new(2, Status.Criado, [new("Mesa", 1, 300m)]),
    new(3, Status.Entregue, [new("Cadeira", 4, 90m)]),
];
Console.WriteLine(new PedidoServico(relogio).Carimbo(pedidos[0]));
Console.WriteLine($"  Faturamento: {pedidos.Faturamento:C}");
Console.WriteLine($"  Pagos: {pedidos.Com(Status.Pago).Count()}");

Cliente? cliente = new() { Nome = "  Ana  " };
cliente?.Nome = "  Beto ";
Console.WriteLine($"  Cliente: [{cliente?.Nome}]");

Console.WriteLine("2. Configuracao e DI");
var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddInMemoryCollection(
    new Dictionary<string, string?>
    {
        ["Loja:Nome"] = "Loja Exemplo",
        ["Loja:DescontoMaximo"] = "15",
    });
builder.Services
    .AddOptions<LojaOptions>()
    .BindConfiguration(LojaOptions.Secao)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<DescontoServico>();
builder.Services.AddSingleton<IChatClient, ChatFalso>();
builder.Services.AddSingleton<Resumidor>();

using var host = builder.Build();
var sp = host.Services;
var desconto = sp.GetRequiredService<DescontoServico>();
Console.WriteLine($"  100 com 30%: {desconto.Aplicar(100, 30)}");

Console.WriteLine("3. Dados com EF Core 10 e Dapper");
await DadosDemo.ExecutarAsync();

Console.WriteLine("4. IA com Microsoft.Extensions.AI");
var resumidor = sp.GetRequiredService<Resumidor>();
var resumo = await resumidor.ResumirAsync("Texto longo");
Console.WriteLine($"  {resumo}");
