namespace GuiaConsole;

public enum Status { Criado, Pago, Entregue }

public record Item(string Produto, int Quantidade, decimal Preco)
{
    public decimal Subtotal => Quantidade * Preco;
}

public record Pedido(int Id, Status Status, List<Item> Itens)
{
    public decimal Total => Itens.Sum(i => i.Subtotal);
}

public interface IRelogio
{
    DateTime Agora { get; }
}

public class Relogio : IRelogio
{
    public DateTime Agora => DateTime.UtcNow;
}

// Antes: campo, construtor e atribuicao manual
public class PedidoServicoAntigo
{
    private readonly IRelogio _relogio;

    public PedidoServicoAntigo(IRelogio relogio)
    {
        _relogio = relogio;
    }

    public string Carimbo(Pedido p) =>
        $"#{p.Id} em {_relogio.Agora:HH:mm}";
}

// Depois: construtor primario (C# 12)
public class PedidoServico(IRelogio relogio)
{
    public string Carimbo(Pedido p) =>
        $"#{p.Id} em {relogio.Agora:HH:mm}";
}

public class Cliente
{
    // C# 14: palavra-chave field, sem campo declarado
    public string Nome
    {
        get;
        set => field = value?.Trim()
            ?? throw new ArgumentNullException(nameof(value));
    } = "";
}

public static class PedidoExtensoes
{
    // C# 14: blocos extension (propriedades de extensao)
    extension(IEnumerable<Pedido> pedidos)
    {
        public decimal Faturamento =>
            pedidos.Where(p => p.Status != Status.Criado)
                   .Sum(p => p.Total);

        public IEnumerable<Pedido> Com(Status status) =>
            pedidos.Where(p => p.Status == status);
    }
}
