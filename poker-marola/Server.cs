using System.Net;
using System.Net.Sockets;
using Mono.Nat;

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
    public async Task StartAsync() {
        ServerPort  = Random.Shared.Next(49152, 65535+1);
        tcpListener = new TcpListener(IPAddress.Any, ServerPort);
        bool natOpen = await TryOpenNatAsync(ServerPort);
        if (!natOpen) {
            Console.WriteLine("NAT não foi aberto. O servidor só poderá ser acessado por LAN(local, hamachi, radmin, etc).");
        }
        tcpListener.Start();
        Console.WriteLine($"Servidor iniciado em {ServerAddress}:{ServerPort}");
        TcpClient client = await tcpListener.AcceptTcpClientAsync();
        Console.WriteLine("Cliente conectado: " + client.Client.RemoteEndPoint);
        await client.Client.SendAsync(new byte[] { 1, 2, 3, 4 }, SocketFlags.None);
        tcpListener?.Stop();
        if (natOpen) {
            await natDevice!.DeletePortMapAsync(new Mapping(Protocol.Tcp, ServerPort, ServerPort));
            Console.WriteLine($"Porta {ServerPort} fechada no roteador.");
        }
    }
    
    public IPAddress ServerAddress { get; private set; } = IPAddress.None;
    public int ServerPort { get; private set; } = -1;
    
    private TcpListener tcpListener;
    private INatDevice? natDevice;

    private async Task<bool> TryOpenNatAsync(int port) {
        INatDevice? device = null;
        NatUtility.DeviceFound += async (sender, args) =>
        {
            device = args.Device;
            Console.WriteLine("Roteador UPnP encontrado: " + device.GetExternalIP());
            
            await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port));
            Console.WriteLine($"Porta {port} aberta no roteador.");
        };
        NatUtility.StartDiscovery();
        await Task.Delay(3000); // Dá tempo para a descoberta
        natDevice = device;
        return device != null;
    }
    
    /// <summary>
    /// Libera todos os recursos utilizados pelo servidor. Fecha conexões com jogadores
    /// se necessário.
    /// </summary>
    public void Dispose() {
        
    }
}