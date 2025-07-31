// Ignore Spelling: Validator

using System.Globalization;

namespace ExcelReader.RyanW84.Helpers;

public static class ExcelFieldValidator
{
    public static bool IsValidDate(string date)
    {
        return DateTime.TryParseExact(
            date,
            "dd-MM-yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
    }

    public static int? CalculateAge(string? dobValue)
    {
        if (!string.IsNullOrEmpty(dobValue) && DateTime.TryParseExact(
                dobValue,
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dob
            ))
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age))
                age--;
            return age;
        }
        return null;
    }
}