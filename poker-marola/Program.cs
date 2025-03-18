using System.Reflection;
using System.Text;
using System.Text.Json;

namespace poker_marola;

internal static class Program
{
    private static void Main(string[] args) {
        DisplaySplashScreen();
        ShowMenu();
    }

    /// <summary>
    /// Mostra a arte de capa do jogo
    /// </summary>
    private static void DisplaySplashScreen() {
        Console.WriteLine(
            """

             _____      _                   
            |  __ \    | |                  
            | |__) |__ | | _____ _ __       
            |  ___/ _ \| |/ / _ \ '__|      
            | |  | (_) |   <  __/ |         
            |_|  _\___/|_|\_\___|_| _       
            |  \/  |               | |      
            | \  / | __ _ _ __ ___ | | __ _ 
            | |\/| |/ _` | '__/ _ \| |/ _` |
            | |  | | (_| | | | (_) | | (_| |
            |_|  |_|\__,_|_|  \___/|_|\__,_|

            """);
        string[] authors = [
            "Autores: ",
            "- João Vítor Guterres Giovelli",
            "- Rodrigo Appelt"
        ];
        Console.WriteLine(Tui.BuildMessageBox(authors));
        Version version = Assembly.GetExecutingAssembly().GetName().Version!;
        Console.WriteLine(Tui.BuildMessageBox([ $"Versão: {version.ToString(3)}" ]));
    }
    
    private static void ShowMenu() {
        // mostra opcoes de jogo
        Tui.Option[] options = [
            new("Iniciar servidor"),
            new("Entrar em uma partida"),
            new("Configurações"),
        ];
        int opt = Tui.ShowMenu("Selecione uma opção", options);
        if (opt == -1) {
            Console.WriteLine("Erro brabo");
            return;
        }
        if (opt == 0) {
            Server server = new(new() {
                UseNat = false
            });
            // aqui o server comeca a rodar
            _ = server.StartAsync();
            
            // e aqui vamos no fluxo normal para entrar em um servidor
        }else if (opt == 1) {
            // pedir ip e porta
        }else if (opt == 2) {
            ShowConfiguration();
        }
    }

    private static void ShowConfiguration() {
        string dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "poker-marola");
        string path = Path.Combine(dir, "user.json");
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        if (!File.Exists(path)) {
            var id = Guid.NewGuid();
            var u = new User {
                Username = User.DeriveName(id),
                Id = id
            };
            File.WriteAllText(path, JsonSerializer.Serialize(u));
        }
        var user = JsonSerializer.Deserialize<User>(File.ReadAllText(path));
        
        if (user == null) {
            var id = Guid.NewGuid();
            user = new User {
                Username = User.DeriveName(id),
                Id = id
            };
            
            File.WriteAllText(path, JsonSerializer.Serialize(user));
        }

        Console.WriteLine(Tui.BuildMessageBox([
            "Configurações",
            $"Nome: {user.Username}",
            $"Id: {user.Id}",
        ]));
        int opt;
        do {
            opt = Tui.ShowMenu("Selecione uma opção:", [
                new Tui.Option("Alterar nome"),
                new Tui.Option("Criar novo Id"),
                new Tui.Option("Voltar")
            ]);
            if (opt == 0) {
                Console.Write("Digite o novo nome: ");
                user.Username = Console.ReadLine();
            }else if (opt == 1) {
                user.Id = Guid.NewGuid();
            }

            File.WriteAllText(path, JsonSerializer.Serialize(user));

        }while(opt != -1 && opt != 2);
    }
}
