namespace Shared.Services
{
    public static class TextService
    {
        public static string GetChechenString(string rawStr)
        {
            if (string.IsNullOrWhiteSpace(rawStr))
            {
                return string.Empty;
            }
            return rawStr.Replace("1", "Ӏ")
                .Replace("I", "Ӏ")
                .Trim();
        }

    }
}
