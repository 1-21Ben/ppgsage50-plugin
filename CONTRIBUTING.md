# Guide de Contribution - Plugin PPG Sage 50 ERP

Merci de votre intérêt pour contribuer au plugin PPG Sage 50 ERP ! Ce guide vous aidera à comprendre comment participer au développement du projet.

## 🚀 Démarrage Rapide

### Prérequis
- **Visual Studio 2019** ou supérieur
- **.NET Framework 4.8**
- **Sage 50 ERP** (pour les tests)
- **Credentials API PPG Live** (pour les tests)

### Installation du Projet
1. **Fork le repository**
2. **Cloner votre fork** :
   ```bash
   git clone https://github.com/VOTRE_USERNAME/ppgsage50-plugin.git
   cd ppgsage50-plugin
   ```
3. **Restaurer les packages NuGet** :
   ```cmd
   nuget restore PPGSage50Plugin.sln
   ```
4. **Compiler le projet** :
   ```cmd
   msbuild PPGSage50Plugin.sln /p:Configuration=Debug
   ```

## 📋 Types de Contributions

### 🐛 Signalement de Bugs
- Utiliser les **Issues GitHub**
- Inclure les informations suivantes :
  - Version de Sage 50 ERP
  - Version de Windows
  - Logs d'erreur complets
  - Étapes pour reproduire le bug
  - Configuration App.config (sans credentials)

### ✨ Nouvelles Fonctionnalités
- Créer une **Issue** pour discuter de la fonctionnalité
- Attendre l'approbation avant de commencer le développement
- Suivre les conventions de code du projet

### 📚 Amélioration de la Documentation
- Corrections de typos
- Amélioration des guides d'installation
- Ajout d'exemples d'utilisation
- Traduction en d'autres langues

### 🔧 Améliorations Techniques
- Optimisation des performances
- Refactoring du code
- Amélioration de la gestion d'erreurs
- Ajout de tests unitaires

## 🛠️ Processus de Développement

### 1. Créer une Branche
```bash
git checkout -b feature/nom-de-la-fonctionnalite
# ou
git checkout -b bugfix/nom-du-bug
```

### 2. Développer
- Suivre les conventions de code
- Ajouter des commentaires en français
- Tester vos modifications
- Mettre à jour la documentation si nécessaire

### 3. Commiter
```bash
git add .
git commit -m "feat: ajout de la fonctionnalité X"
# ou
git commit -m "fix: correction du bug Y"
```

### 4. Pousser et Créer une Pull Request
```bash
git push origin feature/nom-de-la-fonctionnalite
```

## 📝 Conventions de Code

### Style de Code
- **Langage** : C# avec .NET Framework 4.8
- **Commentaires** : En français
- **Nommage** : PascalCase pour les classes, camelCase pour les variables
- **Indentation** : 4 espaces

### Structure des Commits
```
type(scope): description

feat: nouvelle fonctionnalité
fix: correction de bug
docs: documentation
style: formatage
refactor: refactoring
test: tests
chore: tâches de maintenance
```

### Exemples
```bash
feat(api): ajout de l'endpoint de synchronisation des produits
fix(auth): correction du timeout d'authentification
docs(readme): mise à jour du guide d'installation
```

## 🧪 Tests

### Tests Unitaires
- Créer des tests pour les nouvelles fonctionnalités
- Utiliser le framework de test de Visual Studio
- Couvrir au moins 80% du code critique

### Tests d'Intégration
- Tester avec une instance Sage 50 réelle
- Vérifier la compatibilité avec différentes versions
- Tester les scénarios d'erreur

### Tests Manuels
- Tester l'interface utilisateur
- Vérifier la synchronisation des données
- Tester la gestion des erreurs

## 📦 Build et Packaging

### Compilation Locale
```cmd
build.bat
```

### Vérification du Package
1. Compiler avec `build.bat`
2. Tester l'installation avec `install.bat`
3. Vérifier que tous les fichiers sont présents
4. Tester les fonctionnalités principales

## 🔍 Code Review

### Critères d'Acceptation
- ✅ Code fonctionnel et testé
- ✅ Documentation mise à jour
- ✅ Pas de régression
- ✅ Respect des conventions
- ✅ Gestion d'erreurs appropriée

### Processus de Review
1. **Automatique** : Vérification par GitHub Actions
2. **Manuel** : Review par les mainteneurs
3. **Tests** : Validation des fonctionnalités
4. **Merge** : Intégration dans la branche principale

## 🚫 Restrictions

### Limitations
- **Pas de modification** des endpoints API PPG Live
- **Pas de suppression** de fonctionnalités existantes sans discussion
- **Pas de changement** de l'architecture principale sans approbation

### Sécurité
- **Ne jamais** commiter de credentials ou secrets
- **Toujours** valider les entrées utilisateur
- **Respecter** les bonnes pratiques de sécurité

## 📞 Support et Communication

### Canaux de Communication
- **Issues GitHub** : Pour les bugs et fonctionnalités
- **Discussions GitHub** : Pour les questions générales
- **Email** : support.ppglive@ppg.com (pour les problèmes critiques)

### Réponse aux Contributions
- **Issues** : Réponse sous 48h
- **Pull Requests** : Review sous 72h
- **Questions** : Réponse sous 24h

## 🏆 Reconnaissance

### Contributeurs
- Ajout à la liste des contributeurs
- Mention dans le CHANGELOG
- Badge de contributeur sur le profil GitHub

### Types de Contributions
- **Code** : Développement de fonctionnalités
- **Documentation** : Amélioration de la documentation
- **Tests** : Ajout de tests et validation
- **Design** : Amélioration de l'interface utilisateur
- **Support** : Aide aux utilisateurs

## 📋 Checklist pour les Pull Requests

### Avant de Soumettre
- [ ] Code compilé sans erreur
- [ ] Tests passent
- [ ] Documentation mise à jour
- [ ] CHANGELOG.md mis à jour
- [ ] Pas de credentials dans le code
- [ ] Commit messages conformes

### Template de Pull Request
```markdown
## Description
Brève description des changements

## Type de Changement
- [ ] Bug fix
- [ ] Nouvelle fonctionnalité
- [ ] Breaking change
- [ ] Documentation

## Tests
- [ ] Tests unitaires ajoutés/mis à jour
- [ ] Tests d'intégration passent
- [ ] Tests manuels effectués

## Checklist
- [ ] Code reviewé
- [ ] Documentation mise à jour
- [ ] CHANGELOG.md mis à jour
```

## 🎯 Roadmap des Contributions

### Priorités Actuelles
1. **Tests unitaires** : Couverture de code
2. **Performance** : Optimisation des appels API
3. **Interface** : Amélioration de l'UX
4. **Documentation** : Guides utilisateur

### Idées Futures
- Support multi-langues
- Interface mobile
- Intégration avec d'autres ERP
- Dashboard analytique

---

**Merci de contribuer au plugin PPG Sage 50 ERP !** 🚀

Votre contribution aide à améliorer l'intégration entre Sage 50 ERP et PPG Live pour tous les utilisateurs.
