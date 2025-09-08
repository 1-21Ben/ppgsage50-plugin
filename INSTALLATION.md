# Guide d'Installation - Plugin Sage 50 ERP PPG Live

## Prérequis Système

### Logiciels Requis
- **Sage 50 ERP** (version 2020 ou supérieure)
- **.NET Framework 4.8** ou supérieur
- **Windows 10** ou supérieur
- **Visual Studio 2019** ou supérieur (pour la compilation)

### Accès Requis
- **Credentials API PPG Live** (clé API et secret)
- **Accès réseau** à `https://ppglive.fr/api`
- **Permissions d'écriture** dans le répertoire d'installation

## Installation Pas à Pas

### Étape 1 : Préparation de l'Environnement

1. **Vérifier la version de Sage 50**
   ```
   Sage 50 → Aide → À propos de Sage 50
   ```

2. **Installer .NET Framework 4.8** (si nécessaire)
   - Télécharger depuis le site Microsoft
   - Redémarrer l'ordinateur après installation

3. **Créer le répertoire d'installation**
   ```cmd
   mkdir C:\PPGSage50Plugin
   mkdir C:\PPGSage50Plugin\Logs
   ```

### Étape 2 : Compilation du Plugin

1. **Cloner le repository**
   ```bash
   git clone https://github.com/ppg/ppgsage50-plugin.git
   cd ppgsage50-plugin
   ```

2. **Compiler le projet**
   ```cmd
   msbuild PPGSage50Plugin.sln /p:Configuration=Release /p:Platform="Any CPU"
   ```

3. **Vérifier la compilation**
   - Vérifier que `PPGSage50Plugin.dll` est généré
   - Vérifier qu'aucune erreur n'apparaît

### Étape 3 : Installation dans Sage 50

1. **Localiser le répertoire Sage 50**
   ```
   C:\Program Files\Sage\Sage 50\
   ```

2. **Copier les fichiers**
   ```cmd
   copy PPGSage50Plugin.dll "C:\Program Files\Sage\Sage 50\Plugins\"
   copy PPGSage50Plugin.exe "C:\Program Files\Sage\Sage 50\Plugins\"
   copy App.config "C:\Program Files\Sage\Sage 50\Plugins\"
   copy log4net.config "C:\Program Files\Sage\Sage 50\Plugins\"
   ```

3. **Copier les dépendances**
   ```cmd
   copy Newtonsoft.Json.dll "C:\Program Files\Sage\Sage 50\Plugins\"
   copy log4net.dll "C:\Program Files\Sage\Sage 50\Plugins\"
   ```

### Étape 4 : Configuration Initiale

1. **Configurer App.config**
   ```xml
   <appSettings>
     <add key="PPGLiveApiBaseUrl" value="https://ppglive.fr/api" />
     <add key="PPGLiveApiKey" value="VOTRE_CLE_API" />
     <add key="PPGLiveApiSecret" value="VOTRE_SECRET_API" />
     <add key="Sage50DatabasePath" value="C:\Sage50\Data" />
     <add key="Sage50CompanyCode" value="VOTRE_CODE_COMPAGNIE" />
   </appSettings>
   ```

2. **Configurer les permissions**
   - Clic droit sur le dossier `C:\PPGSage50Plugin`
   - Propriétés → Sécurité → Modifier
   - Ajouter "Utilisateurs" avec contrôle total

### Étape 5 : Test de l'Installation

1. **Démarrer Sage 50**
   - Ouvrir Sage 50 ERP
   - Se connecter avec vos credentials

2. **Lancer le plugin**
   - Aller dans Outils → Plugins
   - Sélectionner "Plugin PPG Sage 50"
   - Cliquer sur "Exécuter"

3. **Tester la connexion**
   - Aller dans l'onglet "Paramètres"
   - Cliquer sur "Tester la connexion"
   - Vérifier que le test réussit

## Configuration Avancée

### Paramètres de Synchronisation

```xml
<appSettings>
  <!-- Synchronisation automatique -->
  <add key="AutoSyncCustomers" value="true" />
  <add key="AutoSyncProducts" value="true" />
  <add key="SyncIntervalMinutes" value="60" />
  
  <!-- Gestion des erreurs -->
  <add key="MaxRetryAttempts" value="3" />
  <add key="RetryDelayMs" value="1000" />
  <add key="ApiTimeoutSeconds" value="30" />
</appSettings>
```

### Configuration des Logs

```xml
<appSettings>
  <!-- Niveau de logging -->
  <add key="LogLevel" value="INFO" />
  
  <!-- Répertoire des logs -->
  <add key="LogFilePath" value="C:\PPGSage50Plugin\Logs\" />
  
  <!-- Rotation des logs -->
  <add key="MaxLogFileSizeMB" value="10" />
  <add key="MaxLogFiles" value="5" />
</appSettings>
```

## Vérification Post-Installation

### Checklist de Vérification

- [ ] Plugin visible dans Sage 50
- [ ] Interface utilisateur s'ouvre correctement
- [ ] Test de connexion API réussi
- [ ] Logs générés dans `C:\PPGSage50Plugin\Logs\`
- [ ] Synchronisation des clients fonctionnelle
- [ ] Synchronisation des produits fonctionnelle

### Tests Fonctionnels

1. **Test de Synchronisation Clients**
   ```
   Onglet Synchronisation → Synchroniser clients
   Vérifier les logs de succès
   ```

2. **Test de Création Commande**
   ```
   Onglet Commandes → Nouvelle commande
   Ajouter lignes → Simuler → Envoyer
   ```

3. **Test de Consultation Stock**
   ```
   Onglet Produits → Rechercher produit
   Vérifier informations de stock
   ```

## Dépannage

### Problèmes Courants

#### Erreur "Plugin non trouvé"
```
Solution : Vérifier le chemin d'installation et les permissions
```

#### Erreur "Configuration invalide"
```
Solution : Vérifier App.config et les credentials API
```

#### Erreur "Timeout API"
```
Solution : Augmenter ApiTimeoutSeconds dans App.config
```

#### Erreur "Accès refusé aux logs"
```
Solution : Vérifier les permissions sur C:\PPGSage50Plugin\Logs\
```

### Logs de Dépannage

1. **Consulter les logs**
   ```
   C:\PPGSage50Plugin\Logs\PPGSage50Plugin.log
   ```

2. **Activer le mode DEBUG**
   ```xml
   <root>
     <level value="DEBUG" />
   </root>
   ```

3. **Tester la connectivité**
   ```cmd
   ping ppglive.fr
   telnet ppglive.fr 443
   ```

## Désinstallation

### Étapes de Désinstallation

1. **Arrêter Sage 50**
   - Fermer toutes les instances de Sage 50
   - Arrêter le service Sage 50 (si applicable)

2. **Supprimer les fichiers**
   ```cmd
   del "C:\Program Files\Sage\Sage 50\Plugins\PPGSage50Plugin.*"
   ```

3. **Supprimer les logs** (optionnel)
   ```cmd
   rmdir /s C:\PPGSage50Plugin\Logs
   ```

4. **Nettoyer la configuration**
   - Supprimer les entrées dans App.config
   - Supprimer les références dans Sage 50

## Support Technique

### Ressources de Support

- **Documentation API** : [PPG Live API Docs](https://ppglive.fr/api/docs)
- **Collection Postman** : [Lien vers la collection](https://extapi-ppg-live.postman.co/workspace/extAPI-PPG-Live-Workspace~a1a38643-c7f9-4880-b1a3-fc6effd40782/collection/22734418-911a2a62-5103-486b-a3cb-dd41c15f4944?action=share&creator=22734418)
- **Support Email** : support.ppglive@ppg.com

### Informations de Diagnostic

En cas de problème, fournir :
- Version de Sage 50
- Version de Windows
- Logs d'erreur complets
- Configuration App.config (sans les credentials)
- Description détaillée du problème

---

**Note** : Cette installation doit être effectuée par un administrateur système avec les permissions appropriées.
