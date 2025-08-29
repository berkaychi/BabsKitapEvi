using System.Text.RegularExpressions;

namespace BabsKitapEvi.Business.Utilities
{
    public static class SlugGenerator
    {
        private static readonly Dictionary<char, string> TurkishCharacterMap = new()
        {
            {'ç', "c"}, {'Ç', "c"},
            {'ğ', "g"}, {'Ğ', "g"},
            {'ı', "i"}, {'İ', "i"},
            {'ö', "o"}, {'Ö', "o"},
            {'ş', "s"}, {'Ş', "s"},
            {'ü', "u"}, {'Ü', "u"}
        };

        public static string GenerateSlug(string title, string author)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
                return GenerateSlug(title ?? author ?? "");

            var combinedTitle = $"{title.Trim()} {author.Trim()}";

            var normalized = NormalizeTurkishCharacters(combinedTitle);

            normalized = normalized.ToLowerInvariant();

            normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", "");

            normalized = Regex.Replace(normalized, @"\s+", " ").Trim();

            normalized = normalized.Replace(" ", "-");

            normalized = Regex.Replace(normalized, @"-+", "-");

            normalized = normalized.Trim('-');

            return normalized;
        }

        public static string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            var normalized = NormalizeTurkishCharacters(title);

            normalized = normalized.ToLowerInvariant();

            normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", "");

            normalized = Regex.Replace(normalized, @"\s+", " ").Trim();

            normalized = normalized.Replace(" ", "-");

            normalized = Regex.Replace(normalized, @"-+", "-");

            normalized = normalized.Trim('-');

            return normalized;
        }

        public static async Task<string> GenerateUniqueSlug(string baseSlug, Func<string, Task<bool>> slugExistsCheck)
        {
            var slug = baseSlug;
            var counter = 1;

            while (await slugExistsCheck(slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        public static bool IsValidSlug(string slug)
        {
            if (string.IsNullOrEmpty(slug) || slug.Length < 3 || slug.Length > 200)
                return false;

            var slugPattern = @"^[a-z0-9]+(?:-[a-z0-9]+)*$";
            return Regex.IsMatch(slug, slugPattern);
        }

        private static string NormalizeTurkishCharacters(string text)
        {
            return text
                .Replace('ç', 'c').Replace('Ç', 'c')
                .Replace('ğ', 'g').Replace('Ğ', 'g')
                .Replace('ı', 'i').Replace('İ', 'i')
                .Replace('ö', 'o').Replace('Ö', 'o')
                .Replace('ş', 's').Replace('Ş', 's')
                .Replace('ü', 'u').Replace('Ü', 'u');
        }
    }
}