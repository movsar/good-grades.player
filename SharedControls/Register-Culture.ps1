param (
    [string]$CultureName = "ce",
    [string]$ParentCultureName = "ru"
)

# Check if the culture is already registered
if (![System.Globalization.CultureInfo]::GetCultures([System.Globalization.CultureTypes]::AllCultures).Name -contains $CultureName) {
    try {
        # Load the necessary .NET assembly for CultureAndRegionInfoBuilder
        Add-Type -AssemblyName System.Globalization

        # Create and register the new culture
        $parentCulture = New-Object System.Globalization.CultureInfo($ParentCultureName)
        $builder = New-Object System.Globalization.CultureAndRegionInfoBuilder $CultureName, [System.Globalization.CultureAndRegionModifiers]::None

        # Copy data from the parent culture
        $builder.LoadDataFromCultureInfo($parentCulture)
        $builder.LoadDataFromRegionInfo((New-Object System.Globalization.RegionInfo($ParentCultureName)))
        $builder.CultureEnglishName = "Chechen"
        $builder.CultureNativeName = "Нохчийн"  # Chechen name in native script

        # Register the culture
        $builder.Register()
        Write-Output "Culture '$CultureName' registered successfully."
    } catch {
        Write-Output "Failed to register culture '$CultureName'. Error: $_"
    }
} else {
    Write-Output "Culture '$CultureName' is already registered."
}