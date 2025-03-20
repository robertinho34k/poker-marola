namespace poker_marola;

/// <summary>
/// Representa um usuario do sistema. 
/// </summary>
/// <remarks>
/// Eh diferente de um jogador, pois o jogador existe no contexto de uma
/// partida de poker, e um usuario eh atrelado a uma conta.
/// </remarks>
public class User {

    public string Username { get; set; } = "";
    
    public Guid Id { get; set; } = Guid.Empty;

    public static string DeriveName(Guid id) {
        byte byteId1 = id.ToByteArray()[0];
        byte byteId2 = id.ToByteArray()[1];
        byte i1 = (byte)(byteId1 & 0b111);
        byte i2 = (byte)((byteId1 & 0b11_1000) >> 3);
        byte i3 = (byte)(((byteId1 & 0b1100_0000) >> 6) | ((byteId2 & 0b1) << 2));
        string s1 = i1 switch {
            0 => "Feroz",
            1 => "Rápido",
            2 => "Valente",
            3 => "Perspicaz",
            4 => "Astuto",
            5 => "Corajoso",
            6 => "Cauteloso",
            7 => "Inteligente",
            _ => throw new InvalidOperationException("Value out of range")
        };
        string s2 = i2 switch {
            0 => "Leão",
            1 => "Tigre",
            2 => "Urso",
            3 => "Lobo",
            4 => "Águia",
            5 => "Cobra",
            6 => "Gato",
            7 => "Rato",
            _ => throw new InvalidOperationException("Value out of range")
        };
        string s3 = i3 switch {
            0 => "de Fogo",
            1 => "de Gelo",
            2 => "de Pedra",
            3 => "de Água",
            4 => "de Ar",
            5 => "de Luz",
            6 => "das Trevas",
            7 => "de Terra",
            _ => throw new InvalidOperationException("Value out of range")
        };
        return $"{s1} {s2} {s3}";
    }
}