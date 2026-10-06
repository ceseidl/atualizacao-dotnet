using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;

namespace GuiaConsole;

// Cliente de teste: troque por um provedor real (Azure OpenAI,
// Ollama...) sem mudar o codigo que usa IChatClient.
public class ChatFalso : IChatClient
{
    public Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> mensagens,
        ChatOptions? opcoes = null,
        CancellationToken ct = default)
    {
        var ultima = mensagens.Last().Text;
        var resposta = new ChatMessage(
            ChatRole.Assistant, $"Resumo: {ultima.Length} chars");
        return Task.FromResult(new ChatResponse(resposta));
    }

    public async IAsyncEnumerable<ChatResponseUpdate>
        GetStreamingResponseAsync(
            IEnumerable<ChatMessage> mensagens,
            ChatOptions? opcoes = null,
            [EnumeratorCancellation]
            CancellationToken ct = default)
    {
        var r = await GetResponseAsync(mensagens, opcoes, ct);
        yield return new ChatResponseUpdate(
            ChatRole.Assistant, r.Text);
    }

    public object? GetService(Type tipo, object? chave = null)
        => null;

    public void Dispose() { }
}

public class Resumidor(IChatClient chat)
{
    public async Task<string> ResumirAsync(string texto)
    {
        var r = await chat.GetResponseAsync(
            $"Resuma em uma frase: {texto}");
        return r.Text;
    }
}
