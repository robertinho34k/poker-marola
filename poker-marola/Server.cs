using System.Net;
using System.Net.Sockets;
using Mono.Nat;

namespace poker_marola;

/// <summary>
/// Representa o servidor central do jogo. Normalmente, o host do jogo vai abrir um servidor em
/// outra thread ou async task para que os jogadores possam se conectar e jogar.
/// </summary>
public sealed class Server : IDisposable, IAsyncDisposable {
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
    public Server(ServerConfiguration config) {
        this.config = config;
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
            ExternalServerAddress = await GetLocalIpAddress();
        }
        tcpListener.Start();
        Console.WriteLine($"Servidor iniciado em {ExternalServerAddress}:{ServerPort}");
        
        acceptingTask = AcceptConnectionsAsync(cts.Token);
    }

    public async Task StopAsync() {
        await CloseNatAsync();
        tcpListener.Stop();
        await cts.CancelAsync();
        ExternalServerAddress = IPAddress.None;
        ServerPort = -1;
        cts = new CancellationTokenSource();
        acceptingTask = null;
    }

    private async Task AcceptConnectionsAsync(CancellationToken cancellationToken) {

        while (!cancellationToken.IsCancellationRequested) {
            TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync(cancellationToken);
            _ = HandleClient(tcpClient, cancellationToken);
        }
    }

    private async Task HandleClient(TcpClient tcpClient, CancellationToken cancellationToken) {
        Console.WriteLine($"Recebi cliente! {tcpClient.Client.RemoteEndPoint}");
        tcpClient.NoDelay = true;
        NetworkStream ns = tcpClient.GetStream();
        StreamWriter sw = new StreamWriter(ns);
        
        tcpClient.Close();
    }
    
    public IPAddress ExternalServerAddress { get; private set; } = IPAddress.None;
    
    public int ServerPort { get; private set; } = -1;

    private ServerConfiguration config;
    private TcpListener tcpListener = null!;
    private INatDevice? natDevice;
    private CancellationTokenSource cts = new();
    private Task? acceptingTask = null;

    private async Task<bool> TryOpenNatAsync(int port) {
        INatDevice? device = null;
        NatUtility.DeviceFound += async (sender, args) =>
        {
            device = args.Device;
            ExternalServerAddress = await device.GetExternalIPAsync();
            
            if (config.UseNat) {
                await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port));
            }
        };
        NatUtility.StartDiscovery();
        await Task.Delay(3000); // Dá tempo para a descoberta
        natDevice = device;
        return device != null;
    }

    private async Task CloseNatAsync() {
        if (natDevice is not null) {
            if (config.UseNat) {
                await natDevice.DeletePortMapAsync(new Mapping(Protocol.Tcp, ServerPort, ServerPort));
            }
        }
    }

    private async Task<IPAddress> GetLocalIpAddress() {
        using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
        await socket.ConnectAsync("8.8.8.8", 65530);
        if (socket.LocalEndPoint is null) {
            return IPAddress.None;
        }
        var endPoint = (IPEndPoint)socket.LocalEndPoint;
        return endPoint.Address;
    }
    
    /// <summary>
    /// Libera todos os recursos utilizados pelo servidor. Fecha conexões com jogadores
    /// se necessário.
    /// </summary>
    public void Dispose() {
        tcpListener.Stop();
        tcpListener.Dispose();
        if (natDevice is not null && config.UseNat) {
            natDevice.DeletePortMap(new Mapping(Protocol.Tcp, ServerPort, ServerPort));
        }
    }

    /// <summary>
    /// Libera todos os recursos utilizados pelo servidor. Fecha conexões com jogadores
    /// se necessário.
    /// </summary>
    public async ValueTask DisposeAsync() {
        tcpListener.Dispose();
        await CloseNatAsync();   
    }
}