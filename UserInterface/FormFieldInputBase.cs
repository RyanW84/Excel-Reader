using Spectre.Console;

namespace ExcelReader.RyanW84.UserInterface;

public abstract class FormFieldInputBase
{
    protected virtual string PromptForName(string currentValue) =>
        AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the updated Name:")
                .DefaultValue(currentValue)
                .AllowEmpty()
        );

    protected virtual string PromptForSurname(string currentValue) =>
        AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the updated Surname:")
                .DefaultValue(currentValue)
                .AllowEmpty()
        );

    protected virtual string PromptForSex() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select sex:")
                .AddChoices("Male", "Female", "Other")
        );

    protected virtual string PromptForColour() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select colour:")
                .AddChoices("White", "Black", "Asian", "African", "Other")
        );

    // Abstract: Each derived class must provide its own DOB prompt (validator differs)
    protected abstract string PromptForDob(string currentValue);
}
