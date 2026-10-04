using System.ComponentModel.DataAnnotations;

namespace Panda.Application.Validators;

public static class NhsNumberValidator
{
    public static void EnsureValid(string nhs)
    {
        if (!IsValid(nhs))
            throw new ValidationException("Invalid NHS number.");
    }

    public static bool IsValid(string nhs)
    {
        if (string.IsNullOrWhiteSpace(nhs))
            return false;

        nhs = nhs.Trim();

        if (nhs.Length != 10 || !nhs.All(char.IsDigit))
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int digit = nhs[i] - '0';
            int weight = 10 - i;
            sum += digit * weight;
        }

        // Divide the total by 11 and get the remainder
        int remainder = sum % 11;

        // Subtract remainder from 11 to get check digit
        int checkDigit = 11 - remainder;

        // Special NHS rules
        if (checkDigit == 11)
            checkDigit = 0;

        // NHS number is invalid
        if (checkDigit == 10)
            return false;

        // Compare calculated check digit with actual last digit
        int lastDigit = nhs[9] - '0';
        return checkDigit == lastDigit;
    }
}