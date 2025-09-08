@echo off
echo ========================================
echo Plugin PPG Sage 50 ERP - Setup GitHub
echo ========================================
echo.

echo [INFO] Repository local pret pour GitHub !
echo.
echo Fichiers inclus:
echo   - 50 fichiers de code source et documentation
echo   - 8714 lignes de code et documentation
echo   - Structure complete: Models, Services, UI, Configuration
echo   - Scripts d'installation: install.bat, build.bat
echo   - Documentation complete: README, guides, securite
echo   - GitHub Actions: Build et Release automatiques
echo   - Templates: Issues, PR, Code owners
echo   - Configuration: Dependabot, securite, labels
echo.

echo [ETAPE 1] Creer le repository sur GitHub
echo.
echo 1. Aller sur: https://github.com/new
echo 2. Nom du repository: ppgsage50-plugin
echo 3. Description: Plugin Sage 50 ERP pour l'integration complete avec PPG Live
echo 4. Visibilite: Public
echo 5. Ne PAS cocher "Initialize with README"
echo 6. Cliquer sur "Create repository"
echo.

set /p "REPO_CREATED=Repository cree sur GitHub ? (y/N): "
if /i not "%REPO_CREATED%"=="y" (
    echo [INFO] Veuillez creer le repository sur GitHub d'abord
    echo Voir GITHUB_SETUP_GUIDE.md pour les instructions detaillees
    pause
    exit /b 1
)

echo.
echo [ETAPE 2] Configuration du remote
echo.
set /p "GITHUB_USERNAME=Nom d'utilisateur GitHub: "

if "%GITHUB_USERNAME%"=="" (
    echo [ERREUR] Nom d'utilisateur requis
    pause
    exit /b 1
)

echo [INFO] Configuration du remote origin...
git remote add origin https://github.com/%GITHUB_USERNAME%/ppgsage50-plugin.git

if %errorLevel% == 0 (
    echo [OK] Remote origin configure
) else (
    echo [ATTENTION] Remote deja configure ou erreur
)

echo.
echo [ETAPE 3] Push du code vers GitHub
echo.
echo [INFO] Push du code vers GitHub...
git push -u origin main

if %errorLevel% == 0 (
    echo [OK] Code pousse avec succes !
    echo.
    echo ========================================
    echo Repository GitHub cree avec succes !
    echo ========================================
    echo.
    echo URL du repository: https://github.com/%GITHUB_USERNAME%/ppgsage50-plugin
    echo.
    echo [ETAPES SUIVANTES]
    echo 1. Aller sur le repository GitHub
    echo 2. Configurer les topics dans "About"
    echo 3. Ajouter les labels dans "Issues - Labels"
    echo 4. Creer la premiere release v1.0.0
    echo 5. Partager avec la communaute Sage 50 ERP
    echo.
    echo [CONFIGURATION RECOMMANDEE]
    echo Topics: sage50, erp, ppg-live, integration, plugin, csharp, dotnet, api, synchronization
    echo Labels: bug, enhancement, documentation, good first issue, help wanted, priority: high/medium/low, security, dependencies, api, ui, sync, installation
    echo.
    echo [DOCUMENTATION]
    echo - README.md: Documentation principale
    echo - QUICK_START.md: Guide de demarrage rapide
    echo - INSTALLATION.md: Guide d'installation detaille
    echo - GITHUB_SETUP_GUIDE.md: Guide de configuration GitHub
    echo.
    echo [SUPPORT]
    echo Email: support.ppglive@ppg.com
    echo Documentation API: https://ppglive.fr/api/docs
    echo.
) else (
    echo [ERREUR] Echec du push vers GitHub
    echo.
    echo [SOLUTIONS POSSIBLES]
    echo 1. Verifier que le repository existe sur GitHub
    echo 2. Verifier les credentials GitHub
    echo 3. Executer manuellement: git push -u origin main
    echo.
)

echo.
echo [INFO] Script termine
echo.
pause
