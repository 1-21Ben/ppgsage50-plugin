# Plugin Sage 50 ERP - Intégration PPG Live

## Description

Ce plugin permet une intégration complète et fluide entre Sage 50 ERP et la plateforme e-commerce PPG Live via leurs API REST. Il automatise et facilite la gestion commerciale, logistique, et financière directement depuis Sage 50, sans avoir à basculer entre les systèmes.

## Fonctionnalités

### 🔄 Synchronisation Bidirectionnelle
- **Clients** : Synchronisation automatique des fiches clients entre Sage 50 et PPG Live
- **Produits** : Mise à jour des catalogues produits et informations de stock
- **Commandes** : Envoi des commandes depuis Sage 50 vers PPG Live
- **Devis** : Création et gestion des devis commerciaux
- **Factures** : Consultation et téléchargement des factures PPG Live

### 💰 Gestion des Tarifs
- Consultation des tarifs personnalisés par client
- Calcul automatique des prix avec remises
- Validation des conditions commerciales

### 📦 Gestion du Stock
- Consultation en temps réel des niveaux de stock
- Vérification de disponibilité des produits
- Gestion des back-orders pour produits en rupture

### 🚚 Suivi des Livraisons
- Récupération des bons de livraison
- Suivi des expéditions avec numéros de tracking
- Téléchargement des documents PDF

### 📊 Tableaux de Bord
- Interface utilisateur intuitive avec onglets spécialisés
- Logs détaillés des opérations de synchronisation
- Gestion des erreurs et notifications

## Installation

### Prérequis
- Sage 50 ERP (version compatible)
- .NET Framework 4.8
- Accès à l'API PPG Live avec credentials valides
- Windows 10 ou supérieur

### Étapes d'installation

1. **Télécharger le plugin**
   ```bash
   git clone https://github.com/ppg/ppgsage50-plugin.git
   ```

2. **Compiler le projet**
   ```bash
   cd ppgsage50-plugin
   msbuild PPGSage50Plugin.sln /p:Configuration=Release
   ```

3. **Installer dans Sage 50**
   - Copier les fichiers compilés dans le répertoire des plugins Sage 50
   - Configurer les paramètres dans `App.config`

4. **Configuration initiale**
   - Ouvrir l'interface du plugin
   - Aller dans l'onglet "Paramètres"
   - Saisir les credentials API PPG Live
   - Tester la connexion

## Configuration

### Paramètres API PPG Live

```xml
<appSettings>
  <add key="PPGLiveApiBaseUrl" value="https://ppglive.fr/api" />
  <add key="PPGLiveApiKey" value="VOTRE_CLE_API" />
  <add key="PPGLiveApiSecret" value="VOTRE_SECRET_API" />
  <add key="ApiTimeoutSeconds" value="30" />
</appSettings>
```

### Paramètres Sage 50

```xml
<appSettings>
  <add key="Sage50DatabasePath" value="C:\Sage50\Data" />
  <add key="Sage50CompanyCode" value="VOTRE_CODE_COMPAGNIE" />
  <add key="Sage50UserId" value="VOTRE_USER_ID" />
</appSettings>
```

### Configuration de la Synchronisation

```xml
<appSettings>
  <add key="AutoSyncCustomers" value="true" />
  <add key="AutoSyncProducts" value="true" />
  <add key="SyncIntervalMinutes" value="60" />
</appSettings>
```

## Utilisation

### Interface Principale

Le plugin s'ouvre dans une fenêtre principale avec plusieurs onglets :

#### 📋 Onglet Clients
- Recherche et consultation des clients PPG Live
- Synchronisation des données clients
- Affichage des détails complets

#### 📦 Onglet Produits & Stock
- Consultation du catalogue produits
- Vérification des niveaux de stock
- Synchronisation des données produits

#### 🛒 Onglet Commandes
- Création de nouvelles commandes
- Simulation avant envoi
- Envoi vers PPG Live
- Suivi des commandes existantes

#### 📄 Onglet Factures
- Consultation des factures par client
- Téléchargement des PDF
- Suivi des paiements

#### 💼 Onglet Devis
- Création de nouveaux devis
- Conversion en commandes
- Gestion des validités

#### 🔄 Onglet Synchronisation
- Synchronisation complète
- Synchronisation par type d'entité
- Logs détaillés des opérations

#### ⚙️ Onglet Paramètres
- Configuration des credentials API
- Paramètres de synchronisation
- Test de connexion

### Création d'une Commande

1. **Ouvrir l'onglet Commandes**
2. **Cliquer sur "Nouvelle commande"**
3. **Sélectionner le client**
4. **Ajouter les lignes de commande**
5. **Simuler la commande** (vérification prix, stock, etc.)
6. **Envoyer vers PPG Live**

### Synchronisation

#### Synchronisation Automatique
- Configurée dans les paramètres
- Exécutée selon l'intervalle défini
- Logs automatiques des opérations

#### Synchronisation Manuelle
- Via l'onglet Synchronisation
- Choix des entités à synchroniser
- Suivi en temps réel

## API Endpoints Supportés

| Fonctionnalité | Endpoint | Description |
|----------------|----------|-------------|
| Gestion clients | `POST /get-customer-data` | Récupération des fiches clients |
| Tarifs client | `POST /get-customer-specific-price` | Consultation des tarifs personnalisés |
| Simulation commande | `POST /simulate-order` | Simulation avant envoi |
| Envoi commande | `POST /send-order-v2` | Envoi de commande réelle |
| Consultation stock | `POST /get-stock-info` | Vérification des quantités |
| Bon de livraison | `POST /get-delivery-note` | Récupération des BL |
| Liste factures | `POST /get-invoice-list` | Liste des factures |
| Détail facture | `POST /get-invoice` | Détails d'une facture |
| Création devis | `POST /create-quotation` | Création de devis |
| Demande back-order | `POST /back-order-request` | Demande de commande différée |

## Logs et Monitoring

### Fichiers de Logs
- `PPGSage50Plugin.log` : Logs généraux
- `PPGSage50Plugin_Errors.log` : Erreurs uniquement
- `PPGSage50Plugin_API.log` : Appels API détaillés
- `PPGSage50Plugin_Sync.log` : Opérations de synchronisation

### Niveaux de Log
- **DEBUG** : Informations détaillées
- **INFO** : Informations générales
- **WARN** : Avertissements
- **ERROR** : Erreurs
- **FATAL** : Erreurs critiques

## Dépannage

### Problèmes Courants

#### Erreur d'Authentification
```
Erreur: Échec de l'authentification avec PPG Live
```
**Solution** : Vérifier les credentials API dans les paramètres

#### Timeout des Appels API
```
Erreur: Timeout de la requête
```
**Solution** : Augmenter le timeout dans les paramètres

#### Erreur de Synchronisation
```
Erreur: Échec de la synchronisation des clients
```
**Solution** : Vérifier la connectivité réseau et les logs détaillés

### Logs de Dépannage

1. **Activer le mode DEBUG** dans `log4net.config`
2. **Consulter les logs** dans `C:\PPGSage50Plugin\Logs\`
3. **Tester la connexion** via l'onglet Paramètres

## Support Technique

### Documentation API PPG Live
- [Collection Postman](https://extapi-ppg-live.postman.co/workspace/extAPI-PPG-Live-Workspace~a1a38643-c7f9-4880-b1a3-fc6effd40782/collection/22734418-911a2a62-5103-486b-a3cb-dd41c15f4944?action=share&creator=22734418)

### Contact Support
- **Email** : support.ppglive@ppg.com
- **Documentation** : [PPG Live API Docs](https://ppglive.fr/api/docs)

## Changelog

### Version 1.0.0
- Intégration complète avec PPG Live API
- Synchronisation bidirectionnelle
- Interface utilisateur intuitive
- Gestion des erreurs robuste
- Logs détaillés

## Licence

Copyright © 2024 PPG Industries. Tous droits réservés.

## Contribution

Les contributions sont les bienvenues ! Merci de suivre les guidelines de contribution du projet.

---

**Note** : Ce plugin est développé spécifiquement pour l'intégration entre Sage 50 ERP et PPG Live. Assurez-vous d'avoir les bonnes versions et configurations avant utilisation.
