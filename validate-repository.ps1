# Script de validation du repository GitHub
# Plugin PPG Sage 50 ERP

Write-Host "🔍 Validation du Repository GitHub - Plugin PPG Sage 50 ERP" -ForegroundColor Green
Write-Host ""

$errors = @()
$warnings = @()

# Vérifier la structure des fichiers
Write-Host "📁 Vérification de la structure des fichiers..." -ForegroundColor Yellow

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

foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Fichier manquant: $file"
    }
}

# Vérifier les modèles de données
Write-Host ""
Write-Host "📊 Vérification des modèles de données..." -ForegroundColor Yellow

$modelFiles = @(
    "PPGSage50Plugin/Models/Customer.cs",
    "PPGSage50Plugin/Models/Product.cs",
    "PPGSage50Plugin/Models/Order.cs",
    "PPGSage50Plugin/Models/Invoice.cs",
    "PPGSage50Plugin/Models/DeliveryNote.cs",
    "PPGSage50Plugin/Models/Quotation.cs",
    "PPGSage50Plugin/Models/ApiResponse.cs"
)

foreach ($file in $modelFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Modèle manquant: $file"
    }
}

# Vérifier les services API
Write-Host ""
Write-Host "🔧 Vérification des services API..." -ForegroundColor Yellow

$serviceFiles = @(
    "PPGSage50Plugin/Services/AuthenticationService.cs",
    "PPGSage50Plugin/Services/BaseApiService.cs",
    "PPGSage50Plugin/Services/CustomerApiService.cs",
    "PPGSage50Plugin/Services/ProductApiService.cs",
    "PPGSage50Plugin/Services/OrderApiService.cs",
    "PPGSage50Plugin/Services/InvoiceApiService.cs",
    "PPGSage50Plugin/Services/DeliveryApiService.cs",
    "PPGSage50Plugin/Services/QuotationApiService.cs",
    "PPGSage50Plugin/Services/BackOrderApiService.cs",
    "PPGSage50Plugin/Services/PricingApiService.cs",
    "PPGSage50Plugin/Services/SynchronizationService.cs",
    "PPGSage50Plugin/Services/Logger.cs"
)

foreach ($file in $serviceFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Service manquant: $file"
    }
}

# Vérifier les interfaces utilisateur
Write-Host ""
Write-Host "🖥️ Vérification des interfaces utilisateur..." -ForegroundColor Yellow

$uiFiles = @(
    "PPGSage50Plugin/UI/MainForm.cs",
    "PPGSage50Plugin/UI/OrderForm.cs"
)

foreach ($file in $uiFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Interface manquante: $file"
    }
}

# Vérifier la configuration
Write-Host ""
Write-Host "⚙️ Vérification de la configuration..." -ForegroundColor Yellow

$configFiles = @(
    "PPGSage50Plugin/App.config",
    "PPGSage50Plugin/log4net.config",
    "PPGSage50Plugin/Configuration/AppConfig.cs",
    "packages.config"
)

foreach ($file in $configFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Configuration manquante: $file"
    }
}

# Vérifier les scripts
Write-Host ""
Write-Host "📜 Vérification des scripts..." -ForegroundColor Yellow

$scriptFiles = @(
    "install.bat",
    "build.bat",
    "create-repository.ps1"
)

foreach ($file in $scriptFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Script manquant: $file"
    }
}

# Vérifier la documentation
Write-Host ""
Write-Host "📚 Vérification de la documentation..." -ForegroundColor Yellow

$docFiles = @(
    "README.md",
    "QUICK_START.md",
    "INSTALLATION.md",
    "CONTRIBUTING.md",
    "SECURITY.md",
    "CHANGELOG.md",
    "create-github-repo.md"
)

foreach ($file in $docFiles) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw
        if ($content.Length -gt 100) {
            Write-Host "   ✅ $file (${content.Length} caractères)" -ForegroundColor Green
        } else {
            Write-Host "   ⚠️ $file (trop court: ${content.Length} caractères)" -ForegroundColor Yellow
            $warnings += "Documentation trop courte: $file"
        }
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Documentation manquante: $file"
    }
}

# Vérifier les workflows GitHub
Write-Host ""
Write-Host "🔄 Vérification des workflows GitHub..." -ForegroundColor Yellow

$workflowFiles = @(
    ".github/workflows/build.yml",
    ".github/workflows/release.yml"
)

foreach ($file in $workflowFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Workflow manquant: $file"
    }
}

# Vérifier les templates
Write-Host ""
Write-Host "📋 Vérification des templates..." -ForegroundColor Yellow

$templateFiles = @(
    ".github/ISSUE_TEMPLATE/bug_report.md",
    ".github/ISSUE_TEMPLATE/feature_request.md",
    ".github/ISSUE_TEMPLATE/question.md",
    ".github/PULL_REQUEST_TEMPLATE.md"
)

foreach ($file in $templateFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $file" -ForegroundColor Red
        $errors += "Template manquant: $file"
    }
}

# Résumé
Write-Host ""
Write-Host "📊 Résumé de la validation :" -ForegroundColor Yellow

if ($errors.Count -eq 0) {
    Write-Host "✅ Aucune erreur critique détectée !" -ForegroundColor Green
} else {
    Write-Host "❌ $($errors.Count) erreur(s) critique(s) détectée(s) :" -ForegroundColor Red
    foreach ($error in $errors) {
        Write-Host "   - $error" -ForegroundColor Red
    }
}

if ($warnings.Count -gt 0) {
    Write-Host "⚠️ $($warnings.Count) avertissement(s) :" -ForegroundColor Yellow
    foreach ($warning in $warnings) {
        Write-Host "   - $warning" -ForegroundColor Yellow
    }
}

# Recommandations
Write-Host ""
Write-Host "💡 Recommandations :" -ForegroundColor Cyan
Write-Host "   1. Exécuter 'build.bat' pour tester la compilation"
Write-Host "   2. Vérifier que tous les fichiers sont dans le bon répertoire"
Write-Host "   3. Tester l'installation avec 'install.bat'"
Write-Host "   4. Créer le repository GitHub avec 'create-repository.ps1'"
Write-Host "   5. Configurer les paramètres selon 'create-github-repo.md'"

Write-Host ""
if ($errors.Count -eq 0) {
    Write-Host "🎉 Repository pret pour GitHub !" -ForegroundColor Green
} else {
    Write-Host "🔧 Veuillez corriger les erreurs avant de creer le repository" -ForegroundColor Red
}
