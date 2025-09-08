@echo off
echo ========================================
echo Plugin PPG Sage 50 ERP - Push vers GitHub
echo ========================================
echo.

echo [INFO] Repository local pret pour GitHub !
echo.

echo [STATISTIQUES DU PROJET]
git log --oneline | find /c "commit" > temp_count.txt
set /p COMMIT_COUNT=<temp_count.txt
del temp_count.txt
echo   - Commits: %COMMIT_COUNT%

for /f %%i in ('dir /b /s ^| find /c /v ""') do set FILE_COUNT=%%i
echo   - Fichiers: %FILE_COUNT%
echo   - Branche: main
echo   - Status: Pret pour GitHub
echo.

echo [ETAPE 1] Configuration du remote GitHub
echo.
set /p "GITHUB_USERNAME=Nom d'utilisateur GitHub: "

if "%GITHUB_USERNAME%"=="" (
    echo [ERREUR] Nom d'utilisateur requis
    echo.
    echo [SOLUTION] Executer manuellement:
    echo   git remote add origin https://github.com/VOTRE_USERNAME/ppgsage50-plugin.git
    echo   git push -u origin main
    echo.
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
echo [ETAPE 2] Push du code vers GitHub
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
    echo - GUIDE_FINAL_UTILISATEUR.md: Guide final
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
