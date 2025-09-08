@echo off
echo ========================================
echo Plugin PPG Sage 50 ERP - Compilation
echo ========================================
echo.

REM Vérifier si MSBuild est disponible
where msbuild >nul 2>&1
if %errorLevel% neq 0 (
    echo [ERREUR] MSBuild non trouvé
    echo Veuillez installer Visual Studio ou Build Tools
    echo Télécharger: https://visualstudio.microsoft.com/downloads/
    pause
    exit /b 1
)

echo [OK] MSBuild trouvé

REM Vérifier si .NET Framework 4.8 est installé
reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" /v Release >nul 2>&1
if %errorLevel% neq 0 (
    echo [ERREUR] .NET Framework 4.8 non trouvé
    echo Veuillez installer .NET Framework 4.8
    echo Télécharger: https://dotnet.microsoft.com/download/dotnet-framework/net48
    pause
    exit /b 1
)

echo [OK] .NET Framework 4.8 détecté

REM Créer le répertoire de sortie
if not exist "bin" mkdir "bin"
if not exist "bin\Release" mkdir "bin\Release"

echo [INFO] Début de la compilation...

REM Restaurer les packages NuGet
echo [INFO] Restauration des packages NuGet...
nuget restore PPGSage50Plugin.sln >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] Packages NuGet restaurés
) else (
    echo [ATTENTION] NuGet non trouvé, compilation sans restauration
)

REM Compiler le projet
echo [INFO] Compilation du projet...
msbuild PPGSage50Plugin.sln /p:Configuration=Release /p:Platform="Any CPU" /p:OutputPath=bin\Release\ /verbosity:minimal

if %errorLevel% == 0 (
    echo [OK] Compilation réussie
) else (
    echo [ERREUR] Échec de la compilation
    pause
    exit /b 1
)

REM Copier les fichiers de configuration
echo [INFO] Copie des fichiers de configuration...
copy "PPGSage50Plugin\App.config" "bin\Release\" >nul 2>&1
copy "PPGSage50Plugin\log4net.config" "bin\Release\" >nul 2>&1

REM Copier les dépendances NuGet
echo [INFO] Copie des dépendances...
if exist "packages\Newtonsoft.Json.13.0.3\lib\net45\Newtonsoft.Json.dll" (
    copy "packages\Newtonsoft.Json.13.0.3\lib\net45\Newtonsoft.Json.dll" "bin\Release\" >nul 2>&1
    echo [OK] Newtonsoft.Json.dll copié
)

if exist "packages\log4net.2.0.15\lib\net45\log4net.dll" (
    copy "packages\log4net.2.0.15\lib\net45\log4net.dll" "bin\Release\" >nul 2>&1
    echo [OK] log4net.dll copié
)

if exist "packages\System.Net.Http.4.3.4\lib\net46\System.Net.Http.dll" (
    copy "packages\System.Net.Http.4.3.4\lib\net46\System.Net.Http.dll" "bin\Release\" >nul 2>&1
    echo [OK] System.Net.Http.dll copié
)

if exist "packages\System.Configuration.ConfigurationManager.7.0.0\lib\net48\System.Configuration.ConfigurationManager.dll" (
    copy "packages\System.Configuration.ConfigurationManager.7.0.0\lib\net48\System.Configuration.ConfigurationManager.dll" "bin\Release\" >nul 2>&1
    echo [OK] System.Configuration.ConfigurationManager.dll copié
)

REM Créer le package de distribution
echo [INFO] Création du package de distribution...
if not exist "dist" mkdir "dist"

REM Copier tous les fichiers nécessaires
copy "bin\Release\PPGSage50Plugin.dll" "dist\" >nul 2>&1
copy "bin\Release\PPGSage50Plugin.exe" "dist\" >nul 2>&1
copy "bin\Release\App.config" "dist\" >nul 2>&1
copy "bin\Release\log4net.config" "dist\" >nul 2>&1
copy "bin\Release\Newtonsoft.Json.dll" "dist\" >nul 2>&1
copy "bin\Release\log4net.dll" "dist\" >nul 2>&1
copy "bin\Release\System.Net.Http.dll" "dist\" >nul 2>&1
copy "bin\Release\System.Configuration.ConfigurationManager.dll" "dist\" >nul 2>&1

REM Copier les fichiers d'installation
copy "install.bat" "dist\" >nul 2>&1
copy "README.md" "dist\" >nul 2>&1
copy "INSTALLATION.md" "dist\" >nul 2>&1
copy "LICENSE" "dist\" >nul 2>&1

REM Créer un fichier de version
echo [INFO] Création du fichier de version...
(
echo Plugin PPG Sage 50 ERP - Version 1.0.0
echo Compilé le: %DATE% %TIME%
echo Build: %COMPUTERNAME%
echo.
echo Fichiers inclus:
echo - PPGSage50Plugin.dll ^(Plugin principal^)
echo - PPGSage50Plugin.exe ^(Exécutable^)
echo - App.config ^(Configuration^)
echo - log4net.config ^(Configuration des logs^)
echo - Newtonsoft.Json.dll ^(Dépendance^)
echo - log4net.dll ^(Dépendance^)
echo - System.Net.Http.dll ^(Dépendance^)
echo - System.Configuration.ConfigurationManager.dll ^(Dépendance^)
echo - install.bat ^(Script d'installation^)
echo - README.md ^(Documentation^)
echo - INSTALLATION.md ^(Guide d'installation^)
echo - LICENSE ^(Licence^)
echo.
echo Installation:
echo 1. Exécuter install.bat en tant qu'administrateur
echo 2. Configurer les credentials API dans App.config
echo 3. Lancer le plugin depuis Sage 50
echo.
echo Support: support.ppglive@ppg.com
) > "dist\VERSION.txt"

echo [OK] Package de distribution créé dans le dossier 'dist'

REM Créer une archive ZIP
echo [INFO] Création de l'archive ZIP...
powershell -command "Compress-Archive -Path 'dist\*' -DestinationPath 'PPGSage50Plugin-v1.0.0.zip' -Force" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] Archive ZIP créée: PPGSage50Plugin-v1.0.0.zip
) else (
    echo [ATTENTION] Impossible de créer l'archive ZIP
)

echo.
echo ========================================
echo Compilation terminée avec succès !
echo ========================================
echo.
echo Fichiers générés:
echo   - bin\Release\ : Fichiers compilés
echo   - dist\ : Package de distribution
echo   - PPGSage50Plugin-v1.0.0.zip : Archive d'installation
echo.
echo Pour installer le plugin:
echo 1. Copier le contenu du dossier 'dist' sur l'ordinateur cible
echo 2. Exécuter install.bat en tant qu'administrateur
echo.
pause
