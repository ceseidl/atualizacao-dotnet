using System.ComponentModel.DataAnnotations;

namespace GuiaApi;

public record NovoPedido(
    [Required, MinLength(3)] string Cliente,
    [Range(1, 100)] int Quantidade);

public record PedidoDto(int Id, string Cliente, int Quantidade);

public class PedidoRepositorio
{
    private readonly List<PedidoDto> _lista = [];

    public PedidoDto Adicionar(NovoPedido novo)
    {
        var dto = new PedidoDto(
            _lista.Count + 1, novo.Cliente, novo.Quantidade);
        _lista.Add(dto);
        return dto;
    }

    public PedidoDto? Obter(int id) =>
        _lista.FirstOrDefault(p => p.Id == id);
}
