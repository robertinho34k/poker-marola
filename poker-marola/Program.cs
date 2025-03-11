using System.Reflection;
using System.Text;

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
        Tui.Option? opt = Tui.ShowMenu("Selecione uma opção", options);
        if (opt == null) {
            Console.WriteLine("Erro brabo");
            return;
        }
        if (opt == options[0]) {
            Console.WriteLine("iniciar");
        }else if (opt == options[1]) {
            Console.WriteLine("entrar");
        }else if (opt == options[2]) {
            Console.WriteLine("config");
        }
    }
}
