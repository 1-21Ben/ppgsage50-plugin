# Script simple de verification des fichiers
# Plugin PPG Sage 50 ERP

Write-Host "Verification des fichiers du repository..." -ForegroundColor Green
Write-Host ""

$requiredFiles = @(
    "README.md",
    "QUICK_START.md", 
    "INSTALLATION.md",
    "CONTRIBUTING.md",
    "SECURITY.md",
    "CHANGELOG.md",
    "LICENSE",
    "install.bat",
    "build.bat",
    "PPGSage50Plugin.sln",
    "PPGSage50Plugin/PPGSage50Plugin.csproj",
    "PPGSage50Plugin/Program.cs",
    "PPGSage50Plugin/PPGSage50Plugin.cs",
    ".github/workflows/build.yml",
    ".github/workflows/release.yml",
    ".github/ISSUE_TEMPLATE/bug_report.md",
    ".github/ISSUE_TEMPLATE/feature_request.md",
    ".github/ISSUE_TEMPLATE/question.md",
    ".github/PULL_REQUEST_TEMPLATE.md",
    ".github/CODEOWNERS",
    ".github/dependabot.yml",
    ".gitignore"
)

$missingFiles = @()

foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-Host "OK: $file" -ForegroundColor Green
    } else {
        Write-Host "MANQUANT: $file" -ForegroundColor Red
        $missingFiles += $file
    }
}

Write-Host ""
if ($missingFiles.Count -eq 0) {
    Write-Host "Tous les fichiers requis sont presents !" -ForegroundColor Green
    Write-Host "Le repository est pret pour GitHub." -ForegroundColor Green
} else {
    Write-Host "Fichiers manquants: $($missingFiles.Count)" -ForegroundColor Red
    foreach ($file in $missingFiles) {
        Write-Host "  - $file" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Prochaines etapes:" -ForegroundColor Yellow
Write-Host "1. Creer le repository sur GitHub" -ForegroundColor White
Write-Host "2. Executer: git remote add origin https://github.com/USERNAME/ppgsage50-plugin.git" -ForegroundColor White
Write-Host "3. Executer: git push -u origin main" -ForegroundColor White
Write-Host "4. Configurer les parametres du repository" -ForegroundColor White
