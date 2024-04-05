namespace Data.Services
{
    public static class AssetService
    {
        public static void CopyFonts()
        {
            var destinationDirectory = Environment.SpecialFolder.Fonts.ToString();
            var appFonts = Directory.GetFiles(Path.Combine("Assets", "Fonts"), "*");
            foreach (var file in appFonts)
            {
                // Create the path to where the file will be copied.
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDirectory, fileName);

                // Copy the file.
                File.Copy(file, destFile, true);
            }
        }
    }
}
