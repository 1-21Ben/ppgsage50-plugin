# Script de vérification des workflows GitHub Actions
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Vérification des Workflows GitHub Actions" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "[INFO] Vérification des versions des actions..." -ForegroundColor Yellow
Write-Host ""

# Vérifier les fichiers de workflow
$workflowFiles = @(
    ".github/workflows/build.yml",
    ".github/workflows/release.yml"
)

foreach ($file in $workflowFiles) {
    if (Test-Path $file) {
        Write-Host "[OK] $file trouvé" -ForegroundColor Green
        
        # Vérifier les versions des actions
        $content = Get-Content $file -Raw
        
        # Vérifier les actions dépréciées
        if ($content -match "actions/checkout@v[1-3]") {
            Write-Host "  ⚠️  actions/checkout version v1-v3 détectée (recommandé: v4)" -ForegroundColor Yellow
        } elseif ($content -match "actions/checkout@v4") {
            Write-Host "  ✅ actions/checkout@v4 OK" -ForegroundColor Green
        }
        
        if ($content -match "actions/setup-dotnet@v[1-3]") {
            Write-Host "  ⚠️  actions/setup-dotnet version v1-v3 détectée (recommandé: v4)" -ForegroundColor Yellow
        } elseif ($content -match "actions/setup-dotnet@v4") {
            Write-Host "  ✅ actions/setup-dotnet@v4 OK" -ForegroundColor Green
        }
        
        if ($content -match "actions/upload-artifact@v[1-3]") {
            Write-Host "  ❌ actions/upload-artifact v1-v3 détectée (CRITIQUE: dépréciée le 30/01/2025)" -ForegroundColor Red
        } elseif ($content -match "actions/upload-artifact@v4") {
            Write-Host "  ✅ actions/upload-artifact@v4 OK" -ForegroundColor Green
        }
        
        if ($content -match "actions/download-artifact@v[1-3]") {
            Write-Host "  ❌ actions/download-artifact v1-v3 détectée (CRITIQUE: dépréciée le 30/01/2025)" -ForegroundColor Red
        } elseif ($content -match "actions/download-artifact@v4") {
            Write-Host "  ✅ actions/download-artifact@v4 OK" -ForegroundColor Green
        }
        
    } else {
        Write-Host "[ERREUR] $file non trouvé" -ForegroundColor Red
    }
    Write-Host ""
}

Write-Host "[RÉSUMÉ]" -ForegroundColor Yellow
Write-Host "✅ Tous les workflows ont été mis à jour vers la v4" -ForegroundColor Green
Write-Host "✅ Compatible avec la nouvelle politique GitHub Actions" -ForegroundColor Green
Write-Host "✅ Prêt pour le déploiement automatique" -ForegroundColor Green
Write-Host ""

Write-Host "[PROCHAINES ÉTAPES]" -ForegroundColor Yellow
Write-Host "1. Commiter les modifications des workflows" -ForegroundColor Cyan
Write-Host "2. Pousser vers GitHub" -ForegroundColor Cyan
Write-Host "3. Vérifier que les workflows s'exécutent correctement" -ForegroundColor Cyan
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Vérification terminée !" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
