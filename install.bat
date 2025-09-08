@echo off
echo ========================================
echo Plugin PPG Sage 50 ERP - Installation
echo ========================================
echo.

REM Vérifier les privilèges administrateur
net session >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] Privilèges administrateur détectés
) else (
    echo [ERREUR] Ce script nécessite des privilèges administrateur
    echo Veuillez exécuter en tant qu'administrateur
    pause
    exit /b 1
)

echo.
echo [INFO] Début de l'installation du plugin PPG Sage 50...

REM Créer les répertoires nécessaires
echo [INFO] Création des répertoires...
if not exist "C:\PPGSage50Plugin" mkdir "C:\PPGSage50Plugin"
if not exist "C:\PPGSage50Plugin\Logs" mkdir "C:\PPGSage50Plugin\Logs"
if not exist "C:\PPGSage50Plugin\Config" mkdir "C:\PPGSage50Plugin\Config"

REM Vérifier si Sage 50 est installé
echo [INFO] Vérification de l'installation Sage 50...
set SAGE_PATH=""
if exist "C:\Program Files\Sage\Sage 50" set SAGE_PATH="C:\Program Files\Sage\Sage 50"
if exist "C:\Program Files (x86)\Sage\Sage 50" set SAGE_PATH="C:\Program Files (x86)\Sage\Sage 50"

if %SAGE_PATH%=="" (
    echo [ERREUR] Sage 50 non trouvé dans les emplacements standards
    echo Veuillez installer Sage 50 avant de continuer
    pause
    exit /b 1
)

echo [OK] Sage 50 trouvé dans: %SAGE_PATH%

REM Créer le répertoire plugins s'il n'existe pas
if not exist "%SAGE_PATH%\Plugins" mkdir "%SAGE_PATH%\Plugins"

REM Copier les fichiers du plugin
echo [INFO] Installation des fichiers du plugin...
copy "PPGSage50Plugin.dll" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] PPGSage50Plugin.dll installé
) else (
    echo [ERREUR] Échec de l'installation de PPGSage50Plugin.dll
    pause
    exit /b 1
)

copy "PPGSage50Plugin.exe" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] PPGSage50Plugin.exe installé
) else (
    echo [ERREUR] Échec de l'installation de PPGSage50Plugin.exe
    pause
    exit /b 1
)

copy "App.config" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] App.config installé
) else (
    echo [ERREUR] Échec de l'installation de App.config
    pause
    exit /b 1
)

copy "log4net.config" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] log4net.config installé
) else (
    echo [ERREUR] Échec de l'installation de log4net.config
    pause
    exit /b 1
)

REM Copier les dépendances
echo [INFO] Installation des dépendances...
copy "Newtonsoft.Json.dll" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] Newtonsoft.Json.dll installé
) else (
    echo [ERREUR] Échec de l'installation de Newtonsoft.Json.dll
    pause
    exit /b 1
)

copy "log4net.dll" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] log4net.dll installé
) else (
    echo [ERREUR] Échec de l'installation de log4net.dll
    pause
    exit /b 1
)

copy "System.Net.Http.dll" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] System.Net.Http.dll installé
) else (
    echo [ERREUR] Échec de l'installation de System.Net.Http.dll
    pause
    exit /b 1
)

copy "System.Configuration.ConfigurationManager.dll" "%SAGE_PATH%\Plugins\" >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] System.Configuration.ConfigurationManager.dll installé
) else (
    echo [ERREUR] Échec de l'installation de System.Configuration.ConfigurationManager.dll
    pause
    exit /b 1
)

REM Configurer les permissions
echo [INFO] Configuration des permissions...
icacls "C:\PPGSage50Plugin" /grant "Users:(OI)(CI)F" /T >nul 2>&1
icacls "%SAGE_PATH%\Plugins" /grant "Users:(OI)(CI)F" /T >nul 2>&1

REM Créer le fichier de configuration par défaut
echo [INFO] Création de la configuration par défaut...
(
echo ^<?xml version="1.0" encoding="utf-8"?^>
echo ^<configuration^>
echo   ^<appSettings^>
echo     ^<!-- Configuration API PPG Live --^>
echo     ^<add key="PPGLiveApiBaseUrl" value="https://ppglive.fr/api" /^>
echo     ^<add key="PPGLiveApiKey" value="" /^>
echo     ^<add key="PPGLiveApiSecret" value="" /^>
echo     ^<add key="ApiTimeoutSeconds" value="30" /^>
echo     ^<add key="MaxRetryAttempts" value="3" /^>
echo     ^<add key="RetryDelayMs" value="1000" /^>
echo.
echo     ^<!-- Configuration Sage 50 --^>
echo     ^<add key="Sage50DatabasePath" value="" /^>
echo     ^<add key="Sage50CompanyCode" value="" /^>
echo     ^<add key="Sage50UserId" value="" /^>
echo.
echo     ^<!-- Configuration Logging --^>
echo     ^<add key="LogLevel" value="INFO" /^>
echo     ^<add key="LogFilePath" value="C:\PPGSage50Plugin\Logs\" /^>
echo     ^<add key="MaxLogFileSizeMB" value="10" /^>
echo     ^<add key="MaxLogFiles" value="5" /^>
echo.
echo     ^<!-- Configuration Synchronisation --^>
echo     ^<add key="AutoSyncCustomers" value="true" /^>
echo     ^<add key="AutoSyncProducts" value="true" /^>
echo     ^<add key="SyncIntervalMinutes" value="60" /^>
echo     ^<add key="LastSyncDate" value="" /^>
echo.
echo     ^<!-- Configuration Sécurité --^>
echo     ^<add key="EnableDataEncryption" value="true" /^>
echo     ^<add key="EncryptionKey" value="" /^>
echo     ^<add key="ValidateSSLCertificates" value="true" /^>
echo.
echo     ^<!-- Configuration Interface Utilisateur --^>
echo     ^<add key="DefaultLanguage" value="fr-FR" /^>
echo     ^<add key="ShowDebugInfo" value="false" /^>
echo     ^<add key="DefaultPageSize" value="50" /^>
echo   ^</appSettings^>
echo ^</configuration^>
) > "%SAGE_PATH%\Plugins\App.config"

echo [OK] Configuration par défaut créée

REM Créer un raccourci sur le bureau
echo [INFO] Création du raccourci sur le bureau...
set DESKTOP=%USERPROFILE%\Desktop
echo Set oWS = WScript.CreateObject("WScript.Shell") > "%TEMP%\CreateShortcut.vbs"
echo sLinkFile = "%DESKTOP%\Plugin PPG Sage 50.lnk" >> "%TEMP%\CreateShortcut.vbs"
echo Set oLink = oWS.CreateShortcut(sLinkFile) >> "%TEMP%\CreateShortcut.vbs"
echo oLink.TargetPath = "%SAGE_PATH%\Plugins\PPGSage50Plugin.exe" >> "%TEMP%\CreateShortcut.vbs"
echo oLink.WorkingDirectory = "%SAGE_PATH%\Plugins" >> "%TEMP%\CreateShortcut.vbs"
echo oLink.Description = "Plugin PPG Sage 50 ERP - Intégration PPG Live" >> "%TEMP%\CreateShortcut.vbs"
echo oLink.Save >> "%TEMP%\CreateShortcut.vbs"
cscript "%TEMP%\CreateShortcut.vbs" >nul 2>&1
del "%TEMP%\CreateShortcut.vbs" >nul 2>&1

echo [OK] Raccourci créé sur le bureau

REM Créer le fichier de désinstallation
echo [INFO] Création du script de désinstallation...
(
echo @echo off
echo echo ========================================
echo echo Plugin PPG Sage 50 ERP - Désinstallation
echo echo ========================================
echo echo.
echo echo [INFO] Suppression des fichiers du plugin...
echo del "%SAGE_PATH%\Plugins\PPGSage50Plugin.*" /Q
echo del "%SAGE_PATH%\Plugins\Newtonsoft.Json.dll" /Q
echo del "%SAGE_PATH%\Plugins\log4net.dll" /Q
echo del "%SAGE_PATH%\Plugins\System.Net.Http.dll" /Q
echo del "%SAGE_PATH%\Plugins\System.Configuration.ConfigurationManager.dll" /Q
echo del "%SAGE_PATH%\Plugins\App.config" /Q
echo del "%SAGE_PATH%\Plugins\log4net.config" /Q
echo echo [OK] Fichiers supprimés
echo echo.
echo echo [INFO] Suppression des logs ^(optionnel^)...
echo set /p "DELETE_LOGS=Supprimer les logs ? ^(O/N^): "
echo if /i "%%DELETE_LOGS%%"=="O" ^(
echo     rmdir "C:\PPGSage50Plugin\Logs" /S /Q
echo     echo [OK] Logs supprimés
echo ^)
echo echo.
echo echo [INFO] Suppression du raccourci...
echo del "%USERPROFILE%\Desktop\Plugin PPG Sage 50.lnk" /Q
echo echo [OK] Raccourci supprimé
echo echo.
echo echo [OK] Désinstallation terminée
echo pause
) > "uninstall.bat"

echo [OK] Script de désinstallation créé

echo.
echo ========================================
echo Installation terminée avec succès !
echo ========================================
echo.
echo Le plugin PPG Sage 50 ERP a été installé dans:
echo   - Fichiers: %SAGE_PATH%\Plugins\
echo   - Logs: C:\PPGSage50Plugin\Logs\
echo   - Configuration: %SAGE_PATH%\Plugins\App.config
echo.
echo PROCHAINES ÉTAPES:
echo 1. Configurer vos credentials API PPG Live dans App.config
echo 2. Lancer Sage 50 ERP
echo 3. Exécuter le plugin via le raccourci sur le bureau
echo 4. Tester la connexion dans l'onglet Paramètres
echo.
echo Pour désinstaller: exécuter uninstall.bat
echo.
echo Support: support.ppglive@ppg.com
echo Documentation: https://github.com/ppg/ppgsage50-plugin
echo.
pause
