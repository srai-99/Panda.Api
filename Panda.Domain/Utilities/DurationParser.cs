using System.ComponentModel.DataAnnotations;

namespace Panda.Domain.Utilities;

public static class DurationParser
{
    public static int ParseMinutes(string duration)
    {
        if (string.IsNullOrWhiteSpace(duration))
            throw new ValidationException("Duration cannot be empty.");

        duration = duration.Trim().ToLowerInvariant();

        if (duration.EndsWith("m"))
        {
            var number = duration[..^1];
            if (int.TryParse(number, out var minutes))
                return minutes;

            throw new ValidationException("Invalid duration format. Use '15m' or '1h'.");
        }

        if (duration.EndsWith("h"))
        {
            var number = duration[..^1];
            if (int.TryParse(number, out var hours))
                return hours * 60;

            throw new ValidationException("Invalid duration format. Use '15m' or '1h'.");
        }

        throw new ValidationException("Invalid duration format. Use '15m' or '1h'.");
    }
}