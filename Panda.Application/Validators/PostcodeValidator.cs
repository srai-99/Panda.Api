using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Panda.Application.Validators;

public static class PostcodeValidator
{
    private static readonly Regex UkPostcodeRegex =
        new(@"^[A-Z]{1,2}\d[A-Z\d]?\s?\d[A-Z]{2}$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static string Normalise(string postcode)
    {
        if (string.IsNullOrWhiteSpace(postcode))
            throw new ValidationException("Postcode is required.");

        postcode = postcode.Trim().ToUpperInvariant();

        if (!UkPostcodeRegex.IsMatch(postcode))
            throw new ValidationException("Invalid UK postcode.");

        if (!postcode.Contains(' ') && postcode.Length > 3)
        {
            postcode = postcode.Insert(postcode.Length - 3, " ");
        }

        return postcode;
    }
}