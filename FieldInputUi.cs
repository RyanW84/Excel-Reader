using ExcelReader.RyanW84.Helpers;

using Spectre.Console;

namespace ExcelReader.RyanW84.UserInterface;

/// <summary>
/// Unified field input UI for both Excel and PDF form field editing.
/// Consolidates common functionality while supporting format-specific features.
/// </summary>
public class FieldInputUi
{
	private readonly UserNotifier _userNotifier;

	public FieldInputUi(UserNotifier userNotifier)
	{
		_userNotifier = userNotifier;
	}

	/// <summary>
	/// Asynchronous method to gather updated field values from user input.
	/// Works for both Excel and PDF scenarios.
	/// </summary>
	/// <param name="existingFields">Dictionary of current field values</param>
	/// <param name="fileType">Type of file being processed (for display messages)</param>
	/// <param name="cancellationToken">Cancellation token for async operations</param>
	/// <returns>Dictionary of updated field values</returns>
	public async Task<Dictionary<string , string>> GatherUpdatedFieldsAsync(
		Dictionary<string , string> existingFields ,
		FileType fileType = FileType.Generic ,
		CancellationToken cancellationToken = default)
	{
		return await Task.Run(( ) => GatherUpdatedFields(existingFields , fileType) , cancellationToken);
	}

	/// <summary>
	/// Synchronous method to gather updated field values from user input.
	/// Maintained for backward compatibility.
	/// </summary>
	/// <param name="existingFields">Dictionary of current field values</param>
	/// <param name="fileType">Type of file being processed (for display messages)</param>
	/// <returns>Dictionary of updated field values</returns>
	public Dictionary<string , string> GatherUpdatedFields(
		Dictionary<string , string> existingFields ,
		FileType fileType = FileType.Generic)
	{
		var fieldValues = new Dictionary<string , string>();
		string? dobValue = existingFields.TryGetValue("DOB" , out string? value) ? value : null;
		string? ageFieldName = null;

		var fileTypeName = fileType switch
		{
			FileType.Excel => "Excel",
			FileType.PDF => "PDF form",
			_ => "form"
		};

		AnsiConsole.MarkupLine($"[yellow]Review and update {fileTypeName} fields:[/]");

		foreach (var (fieldName, currentValue) in existingFields)
		{
			string newValue = currentValue;

			if (!AnsiConsole.Confirm(
				$"Field: [green]{fieldName}[/] | Current Value: [yellow]{currentValue}[/] | Update?"))
			{
				fieldValues[fieldName] = newValue;
				continue;
			}

			newValue = fieldName.ToLowerInvariant() switch
			{
				"name" => PromptForName(currentValue),
				"surname" => PromptForSurname(currentValue),
				"age" => HandleAgeField(ref ageFieldName , fieldName , currentValue),
				"sex" => PromptForSex(),
				"colour" => PromptForColour(),
				"wanted" => PromptForWanted(), // PDF-specific field
				_ when fieldName.Contains("dob" , StringComparison.OrdinalIgnoreCase) =>
					dobValue = PromptForDob(currentValue),
				_ => PromptForGeneric(currentValue , fieldName),
			};

			fieldValues[fieldName] = newValue ?? string.Empty;
		}

		// Recalculate age from DOB if possible
		RecalculateAge(fieldValues , ageFieldName , dobValue , existingFields);

		return fieldValues;
	}

	/// <summary>
	/// Asynchronous version of GetFilePath for non-blocking file path input
	/// </summary>
	/// <param name="defaultPath">Default file path</param>
	/// <param name="fileType">Type of file for appropriate prompting</param>
	/// <param name="cancellationToken">Cancellation token for async operations</param>
	/// <returns>Selected file path</returns>
	public async Task<string> GetFilePathAsync(
		string defaultPath ,
		FileType fileType = FileType.Generic ,
		CancellationToken cancellationToken = default)
	{
		return await Task.Run(( ) => GetFilePath(defaultPath , fileType) , cancellationToken);
	}

	/// <summary>
	/// Synchronous version maintained for backward compatibility
	/// </summary>
	/// <param name="defaultPath">Default file path</param>
	/// <param name="fileType">Type of file for appropriate prompting</param>
	/// <returns>Selected file path</returns>
	public string GetFilePath(string defaultPath , FileType fileType = FileType.Generic)
	{
		var promptText = fileType switch
		{
			FileType.Excel => "\nEnter the path to the Excel file (or press Enter for default):",
			FileType.PDF => "\nEnter the path to the PDF form file (or press Enter for default):",
			_ => "\nEnter the file path (or press Enter for default):"
		};

		return AnsiConsole.Ask<string>(promptText , defaultPath);
	}

	/// <summary>
	/// Asynchronous field validation with potential external validation services
	/// </summary>
	/// <param name="fieldName">Name of the field to validate</param>
	/// <param name="fieldValue">Value to validate</param>
	/// <param name="cancellationToken">Cancellation token for async operations</param>
	/// <returns>Validation result</returns>
	public async Task<bool> ValidateFieldAsync(
		string fieldName ,
		string fieldValue ,
		CancellationToken cancellationToken = default)
	{
		return await Task.Run(( ) =>
		{
			return fieldName.ToLowerInvariant() switch
			{
				"dob" or var name when name.Contains("dob" , StringComparison.OrdinalIgnoreCase) =>
					FieldValidator.IsValidDate(fieldValue),
				"age" => int.TryParse(fieldValue , out var age) && age >= 0 && age <= 150,
				"name" or "surname" => !string.IsNullOrWhiteSpace(fieldValue),
				"sex" => new[] { "male" , "female" , "other" }.Contains(fieldValue.ToLowerInvariant()),
				"wanted" => new[] { "yes" , "no" }.Contains(fieldValue.ToLowerInvariant()),
				_ => !string.IsNullOrEmpty(fieldValue) // Generic validation
			};
		} , cancellationToken);
	}

	/// <summary>
	/// Batch field validation for multiple fields
	/// </summary>
	/// <param name="fields">Dictionary of field names and values to validate</param>
	/// <param name="cancellationToken">Cancellation token for async operations</param>
	/// <returns>Dictionary of field names and their validation status</returns>
	public async Task<Dictionary<string , bool>> ValidateFieldsAsync(
		Dictionary<string , string> fields ,
		CancellationToken cancellationToken = default)
	{
		var validationTasks = fields.Select(async field =>
			new KeyValuePair<string , bool>(
				field.Key ,
				await ValidateFieldAsync(field.Key , field.Value , cancellationToken)
			)
		);

		var results = await Task.WhenAll(validationTasks);
		return results.ToDictionary(r => r.Key , r => r.Value);
	}

	private void RecalculateAge(
		Dictionary<string , string> fieldValues ,
		string? ageFieldName ,
		string? dobValue ,
		Dictionary<string , string> existingFields)
	{
		ageFieldName ??= existingFields.Keys.FirstOrDefault(k =>
			k.Equals("age" , StringComparison.OrdinalIgnoreCase));

		if (!string.IsNullOrEmpty(ageFieldName) && dobValue != null)
		{
			int? calculatedAge = FieldValidator.CalculateAge(dobValue);
			if (calculatedAge.HasValue)
			{
				fieldValues[ageFieldName] = calculatedAge.Value.ToString();
				_userNotifier.ShowSuccess($"Calculated age from DOB: {calculatedAge}");
			}
		}
	}

	private string HandleAgeField(ref string? ageFieldName , string fieldName , string currentValue)
	{
		_userNotifier.ShowInfo("Age is autocalculated");
		ageFieldName = fieldName;
		return currentValue;
	}

	private string PromptForName(string currentValue) =>
		AnsiConsole.Prompt(
			new TextPrompt<string>("Enter the updated Name:")
				.DefaultValue(currentValue)
				.AllowEmpty()
		);

	private string PromptForSurname(string currentValue) =>
		AnsiConsole.Prompt(
			new TextPrompt<string>("Enter the updated Surname:")
				.DefaultValue(currentValue)
				.AllowEmpty()
		);

	private string PromptForDob(string currentValue) =>
		AnsiConsole.Prompt(
			new TextPrompt<string>("Enter Date of Birth (dd-MM-yyyy):")
				.DefaultValue(currentValue)
				.Validate(date =>
					FieldValidator.IsValidDate(date)
						? ValidationResult.Success()
						: ValidationResult.Error("Invalid date format. Use dd-MM-yyyy.")
				)
		);

	private string PromptForSex( ) =>
		AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Select sex:")
				.AddChoices("Male" , "Female" , "Other")
		);

	private string PromptForColour( ) =>
		AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Select colour:")
				.AddChoices("White" , "Black" , "Asian" , "African" , "Other")
		);

	private string PromptForWanted( ) =>
		AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Is wanted?")
				.AddChoices("Yes" , "No")
		);

	private string PromptForGeneric(string currentValue , string fieldName) =>
		AnsiConsole.Prompt(
			new TextPrompt<string>($"Enter the updated value for {fieldName}:")
				.DefaultValue(currentValue)
				.AllowEmpty()
		);

	/// <summary>
	/// Display methods for backward compatibility with ExcelUserInputUi
	/// </summary>
	public void DisplayMessage(string message)
	{
		_userNotifier.ShowInfo(message);
	}

	public void DisplayError(string message)
	{
		_userNotifier.ShowError(message);
	}

	public void DisplaySuccess(string message)
	{
		_userNotifier.ShowSuccess(message);
	}

	public void DisplayWarning(string message)
	{
		_userNotifier.ShowWarning(message);
	}

	public void DisplayErrorMessage( )
	{
		_userNotifier.ShowError("An error occurred while processing the file.");
	}

	/// <summary>
	/// Compatibility method for Excel workflows
	/// Maps to GatherUpdatedFields with Excel file type
	/// </summary>
	public Dictionary<string , string> UpdateFieldValues(Dictionary<string , string> existingFields)
	{
		return GatherUpdatedFields(existingFields , FileType.Excel);
	}

	/// <summary>
	/// Async compatibility method for Excel workflows
	/// </summary>
	/// <param name="existingFields">Existing field values</param>
	/// <param name="cancellationToken">Cancellation token for async operations</param>
	/// <returns>Updated field values</returns>
	public async Task<Dictionary<string , string>> UpdateFieldValuesAsync(
		Dictionary<string , string> existingFields ,
		CancellationToken cancellationToken = default)
	{
		return await GatherUpdatedFieldsAsync(existingFields , FileType.Excel , cancellationToken);
	}

	/// <summary>
	/// File type enumeration for context-aware messaging
	/// </summary>
	public enum FileType
	{
		Generic,
		Excel,
		PDF
	}
}