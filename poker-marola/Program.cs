using System.Reflection;

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
        Version version = Assembly.GetExecutingAssembly().GetName().Version!;
        string versionString = $"{version.Major}.{version.Minor}.{version.Build}";
        int trailingSpaces = 51 - 10 - versionString.Length - 1;
        Console.WriteLine(
            $"""
             #-------------------------------------------------#
             | Feito por: João Vitor Guterres e Rodrigo Appelt |
             | Versão: {versionString}{new string(' ', trailingSpaces)}|
             #-------------------------------------------------#
             """);
    }

    private static void ShowMenu() {
        // mostra opcoes de jogo
    }
}
