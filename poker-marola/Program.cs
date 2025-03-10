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
        Console.WriteLine(BuildMessageBox(authors));
        Version version = Assembly.GetExecutingAssembly().GetName().Version!;
        Console.WriteLine(BuildMessageBox([ $"Versão: {version.ToString(3)}" ]));
    }
    
    private static string BuildMessageBox(string[] lines) {
        const int padding = 1;
        int width = lines.Max(x => x.Length) + padding*2 + 2;
        string separator = "#" + new string('-', width-2) + "#";
        StringBuilder sb = new();
        sb.AppendLine(separator);
        foreach (string line in lines) {
            sb.AppendLine($"| {line}{new string(' ', width - line.Length - 2 - padding*2)} |");
        }
        sb.AppendLine(separator);
        return sb.ToString();
    }

    private static void ShowMenu() {
        // mostra opcoes de jogo
    }
}
