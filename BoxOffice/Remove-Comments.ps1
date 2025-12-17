# Quick comment cleaner for BoxOffice
$root = "F:\Programmes\Github\Reps\box-office\BoxOffice"

$targets = @(
    "$root\BoxOffice.API\Controllers",
    "$root\BoxOffice.API\Program.cs",
    "$root\BoxOffice.BLL\DTOs",
    "$root\BoxOffice.BLL\Exceptions", 
    "$root\BoxOffice.BLL\Mapping",
    "$root\BoxOffice.BLL\Services\Interfaces",
    "$root\BoxOffice.BLL\Services\Implementations",
    "$root\BoxOffice.DAL\Configuration",
    "$root\BoxOffice.DAL\Context",
    "$root\BoxOffice.DAL\Models\Entities",
    "$root\BoxOffice.DAL\Models\Enums",
    "$root\BoxOffice.DAL\Repositories\Interfaces",
    "$root\BoxOffice.DAL\Repositories\Implementations"
)

foreach ($target in $targets) {
    if (Test-Path $target) {
        Get-ChildItem $target -Recurse -Filter *.cs | ForEach-Object {
            # Read file
            $lines = Get-Content $_.FullName
            
            # Filter: keep lines that are NOT simple // comments
            # But DO keep /// comments and // with keywords
            $newLines = $lines | Where-Object {
                $t = $_.TrimStart()
                -not $t.StartsWith("//") -or 
                $t.StartsWith("///") -or
                $t -match '^//\s*(TODO|HACK|BUG|FIXME|NOTE|WARNING|IMPORTANT)'
            }
            
            # Save if different
            if ($newLines.Count -ne $lines.Count) {
                Set-Content $_.FullName $newLines -Encoding UTF8
                Write-Host "Cleaned: $($_.Name)"
            }
        }
    }
}

Write-Host "Done!"