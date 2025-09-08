# 🚀 Guide de Démarrage Rapide - Plugin PPG Sage 50 ERP

## Installation Express (5 minutes)

### 📥 Téléchargement
1. **Télécharger le plugin** depuis GitHub :
   ```
   https://github.com/ppg/ppgsage50-plugin/releases/latest
   ```
2. **Extraire l'archive** `PPGSage50Plugin-v1.0.0.zip`

### ⚡ Installation Automatique
1. **Exécuter en tant qu'administrateur** :
   ```cmd
   Clic droit sur install.bat → "Exécuter en tant qu'administrateur"
   ```
2. **Attendre la fin de l'installation** (2-3 minutes)
3. **Le plugin est maintenant installé !** ✅

### 🔧 Configuration Initiale
1. **Ouvrir Sage 50 ERP**
2. **Lancer le plugin** via le raccourci sur le bureau :
   ```
   "Plugin PPG Sage 50"
   ```
3. **Aller dans l'onglet "Paramètres"**
4. **Configurer vos credentials API PPG Live** :
   - URL API : `https://ppglive.fr/api`
   - Clé API : `VOTRE_CLE_API`
   - Secret API : `VOTRE_SECRET_API`
5. **Cliquer sur "Tester la connexion"**
6. **Si le test réussit, vous êtes prêt !** 🎉

## 🎯 Première Utilisation

### Synchronisation des Données
1. **Onglet "Synchronisation"**
2. **Cliquer sur "Synchronisation complète"**
3. **Attendre la fin** (peut prendre quelques minutes)
4. **Vérifier les logs** pour s'assurer du succès

### Création d'une Commande
1. **Onglet "Commandes"**
2. **Cliquer sur "Nouvelle commande"**
3. **Sélectionner un client**
4. **Ajouter des lignes de commande**
5. **Cliquer sur "Simuler"** pour vérifier
6. **Cliquer sur "Envoyer"** vers PPG Live

### Consultation du Stock
1. **Onglet "Produits & Stock"**
2. **Rechercher un produit**
3. **Voir les informations de stock** en temps réel

## 🔍 Vérification de l'Installation

### ✅ Checklist Rapide
- [ ] Plugin visible dans Sage 50
- [ ] Interface s'ouvre sans erreur
- [ ] Test de connexion API réussi
- [ ] Synchronisation clients fonctionnelle
- [ ] Synchronisation produits fonctionnelle
- [ ] Logs générés dans `C:\PPGSage50Plugin\Logs\`

### 🚨 Problèmes Courants

#### "Plugin non trouvé"
**Solution** : Réinstaller avec `install.bat` en tant qu'administrateur

#### "Configuration invalide"
**Solution** : Vérifier les credentials API dans les paramètres

#### "Timeout API"
**Solution** : Augmenter le timeout dans les paramètres (60 secondes)

#### "Erreur d'authentification"
**Solution** : Vérifier la clé API et le secret PPG Live

## 📞 Support Rapide

### 🆘 Besoin d'Aide ?
- **Email** : support.ppglive@ppg.com
- **Documentation** : [README.md](README.md)
- **Installation détaillée** : [INSTALLATION.md](INSTALLATION.md)
- **Logs** : `C:\PPGSage50Plugin\Logs\PPGSage50Plugin.log`

### 🔧 Dépannage Express
1. **Consulter les logs** dans `C:\PPGSage50Plugin\Logs\`
2. **Tester la connexion** dans les paramètres
3. **Vérifier la connectivité** : `ping ppglive.fr`
4. **Contacter le support** avec les logs d'erreur

## 🎉 Félicitations !

Vous avez maintenant :
- ✅ Plugin PPG Sage 50 ERP installé
- ✅ Connexion API PPG Live configurée
- ✅ Synchronisation des données active
- ✅ Interface complète pour gérer vos commandes

**Vous pouvez maintenant profiter de l'intégration complète entre Sage 50 ERP et PPG Live !** 🚀

---

**Temps d'installation total** : ~5 minutes  
**Configuration** : ~2 minutes  
**Première synchronisation** : ~3-5 minutes  

**Total** : ~10 minutes pour être opérationnel ! ⚡
