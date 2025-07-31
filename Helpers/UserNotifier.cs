namespace ExcelReader.RyanW84.Helpers;

public class UserNotifier
{
    public void ShowSuccess(string message) => Spectre.Console.AnsiConsole.MarkupLine($"[green]{message}[/]");
    public void ShowError(string message) => Spectre.Console.AnsiConsole.MarkupLine($"[red]{message}[/]");
    public void ShowWarning(string message) => Spectre.Console.AnsiConsole.MarkupLine($"[yellow]{message}[/]");
    public void ShowInfo(string message) => Spectre.Console.AnsiConsole.MarkupLine(message);
}