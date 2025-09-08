# Script PowerShell pour créer le repository GitHub
# Plugin PPG Sage 50 ERP

Write-Host "🚀 Création du Repository GitHub - Plugin PPG Sage 50 ERP" -ForegroundColor Green
Write-Host ""

# Vérifier si GitHub CLI est installé
try {
    $ghVersion = gh --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ GitHub CLI détecté" -ForegroundColor Green
        
        # Demander les informations
        $username = Read-Host "Nom d'utilisateur GitHub"
        $repoName = "ppgsage50-plugin"
        
        Write-Host ""
        Write-Host "📋 Configuration du repository :" -ForegroundColor Yellow
        Write-Host "   Nom : $repoName"
        Write-Host "   Description : Plugin Sage 50 ERP pour l'intégration complète avec PPG Live"
        Write-Host "   Visibilité : Public"
        Write-Host ""
        
        $confirm = Read-Host "Créer le repository ? (y/N)"
        if ($confirm -eq "y" -or $confirm -eq "Y") {
            Write-Host "🔄 Création du repository..." -ForegroundColor Yellow
            
            # Créer le repository
            gh repo create "$username/$repoName" --public --description "Plugin Sage 50 ERP pour l'intégration complète avec PPG Live - Synchronisation bidirectionnelle, gestion des commandes, devis, factures et stock"
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✅ Repository créé avec succès !" -ForegroundColor Green
                
                # Ajouter le remote et pousser
                Write-Host "🔄 Configuration du remote..." -ForegroundColor Yellow
                git remote add origin "https://github.com/$username/$repoName.git"
                
                Write-Host "🔄 Push du code..." -ForegroundColor Yellow
                git push -u origin main
                
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "✅ Code poussé avec succès !" -ForegroundColor Green
                    Write-Host ""
                    Write-Host "🎉 Repository GitHub créé et configuré !" -ForegroundColor Green
                    Write-Host "📍 URL : https://github.com/$username/$repoName" -ForegroundColor Cyan
                    Write-Host ""
                    Write-Host "📋 Prochaines étapes :" -ForegroundColor Yellow
                    Write-Host "   1. Aller sur https://github.com/$username/$repoName"
                    Write-Host "   2. Configurer les paramètres du repository"
                    Write-Host "   3. Ajouter les topics et labels"
                    Write-Host "   4. Créer la première release v1.0.0"
                    Write-Host "   5. Partager avec la communauté Sage 50 ERP"
                } else {
                    Write-Host "❌ Erreur lors du push du code" -ForegroundColor Red
                }
            } else {
                Write-Host "❌ Erreur lors de la création du repository" -ForegroundColor Red
            }
        } else {
            Write-Host "❌ Création annulée" -ForegroundColor Red
        }
    }
} catch {
    Write-Host "❌ GitHub CLI non trouvé" -ForegroundColor Red
    Write-Host ""
    Write-Host "📋 Instructions manuelles :" -ForegroundColor Yellow
    Write-Host "   1. Aller sur https://github.com/new"
    Write-Host "   2. Créer un repository nommé 'ppgsage50-plugin'"
    Write-Host "   3. Exécuter les commandes suivantes :"
    Write-Host ""
    Write-Host "   git remote add origin https://github.com/USERNAME/ppgsage50-plugin.git" -ForegroundColor Cyan
    Write-Host "   git push -u origin main" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "📖 Voir create-github-repo.md pour les instructions détaillées" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "📚 Documentation disponible :" -ForegroundColor Yellow
Write-Host "   - README.md : Documentation principale"
Write-Host "   - QUICK_START.md : Guide de démarrage rapide"
Write-Host "   - INSTALLATION.md : Guide d'installation détaillé"
Write-Host "   - create-github-repo.md : Instructions de création du repository"
Write-Host ""
Write-Host "🆘 Support : support.ppglive@ppg.com" -ForegroundColor Cyan
