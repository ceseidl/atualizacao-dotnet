using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace GuiaConsole;

public class LojaOptions
{
    public const string Secao = "Loja";

    [Required, MinLength(3)]
    public string Nome { get; set; } = "";

    [Range(0, 100)]
    public int DescontoMaximo { get; set; }
}

public class DescontoServico(IOptions<LojaOptions> opcoes)
{
    private readonly LojaOptions _loja = opcoes.Value;

    public decimal Aplicar(decimal valor, int percentual)
    {
        var p = Math.Min(percentual, _loja.DescontoMaximo);
        return valor - valor * p / 100m;
    }
}
