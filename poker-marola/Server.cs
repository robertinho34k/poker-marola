namespace poker_marola;

/// <summary>
/// Representa o servidor central do jogo. Normalmente, o host do jogo vai abrir um servidor em
/// outra thread ou async task para que os jogadores possam se conectar e jogar.
/// </summary>
public sealed class Server : IDisposable {
    // estudar NAT Hole Punching e UPnP?
    // como outros jogadores vao se conectar?
    // talvez um servidor centra acessivel a todos com 'matchmaking' ou redirection
    // com UPnP nao eh necessario server central

    /*
     * ChatGPT solucoes:
     * - UPnP
     * - Se roteador nao suporta, STUN
     * - TURN
     * - Relay server
     *
     * ou a gente obriga a usar hamachi/radmin e faz apenas na LAN
     */
    public Server() {
        
    }

    /// <summary>
    /// Inicia o servidor, abrindo portas e aceitando conexões de jogadores.
    /// </summary>
    public void Start() {
        
    }
    
    /// <summary>
    /// Libera todos os recursos utilizados pelo servidor. Fecha conexões com jogadores
    /// se necessário.
    /// </summary>
    public void Dispose() {
        
    }
}