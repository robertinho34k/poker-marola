namespace poker_marola;

/// <summary>
/// Classe contendo todas as configuracoes do servidor.
/// </summary>
public struct ServerConfiguration {
    
    /// <summary>
    /// O id do dono do servidor que pode mudar as configuracoes.
    /// </summary>
    public Guid Owner { get; set; }
    
    /// <summary>
    /// Se vai usar NAT Hole Punching para permitir conexoes de outros jogadores.
    /// </summary>
    public bool UseNat { get; set; }
}