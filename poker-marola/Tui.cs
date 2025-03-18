using System.Text;

namespace poker_marola;

/// <summary>
/// Classe para funcoes uteis para um TUI (Text User Interface).
/// </summary>
public static class Tui {
    
    public static string BuildMessageBox(string[] lines) {
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

    public static int ShowMenu(string prompt, Option[] options) {
        if (options.Length <= 0) {
            return -1;
        }
        if (options.Length == 1) {
            return 0;
        }
        
        string ident = "  ";
        Console.WriteLine(prompt);
        int index = 1;
        foreach (Option option in options) {
            Console.Write(ident);
            Console.Write($"{index}. ");
            Console.WriteLine(option.Text);
            index++;
        }
        bool selected = false;
        int maxLength = 10;
        int choice = 0;
        while (!selected) { // loop de verificacao
            Console.Write("> ");
            ConsoleKeyInfo key = default;
            StringBuilder sb = new();
            do {
                key = Console.ReadKey(true);
                
                // n digitou texto
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0) {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                    continue;
                }
                if (key.KeyChar is < '0' or > '9') {
                    continue;
                }
                if(sb.Length >= maxLength) {
                    continue;
                }
                sb.Append(key.KeyChar);
                Console.Write(key.KeyChar);
                if (key.Key == ConsoleKey.Enter) {
                    break;
                }
            }while (key.Key != ConsoleKey.Enter || sb.Length > maxLength);

            string input = sb.ToString(); 
            if(!int.TryParse(input, out choice)) {
                Console.CursorLeft = 0;
                Console.Write(new string(' ', maxLength+1));
                Console.CursorLeft = 0;
                continue;
            }
            if (choice < 1 || choice > options.Length) {
                Console.CursorLeft = 0;
                Console.Write(new string(' ', maxLength+1));
                Console.CursorLeft = 0;
                continue;
            }
            selected = true;
            Console.Write('\n');
        }

        return selected ? choice-1 : -1;
    } 

    public class Option {
        public string Text { get; set; } = "";

        public Option(string text) {
            Text = text;
        }
    }
}