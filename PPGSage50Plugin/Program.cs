using System;
using System.Windows.Forms;
using PPGSage50Plugin.UI;
using PPGSage50Plugin.Services;

namespace PPGSage50Plugin
{
    /// <summary>
    /// Point d'entrée principal de l'application
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Initialiser le système de logging
                Logger.Initialize();
                Logger.Info("Démarrage du plugin PPG Sage 50");

                // Configuration de l'application Windows Forms
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Vérifier la configuration
                if (!AppConfig.ValidateConfiguration())
                {
                    Logger.Error("Configuration invalide détectée");
                    MessageBox.Show(
                        "La configuration du plugin est invalide.\nVeuillez vérifier les paramètres dans App.config.",
                        "Erreur de Configuration",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // Créer et afficher le formulaire principal
                using (var mainForm = new MainForm())
                {
                    Logger.Info("Interface utilisateur initialisée");
                    Application.Run(mainForm);
                }

                Logger.Info("Arrêt du plugin PPG Sage 50");
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Erreur critique lors du démarrage: {ex.Message}", ex);
                MessageBox.Show(
                    $"Une erreur critique s'est produite:\n{ex.Message}\n\nVeuillez consulter les logs pour plus de détails.",
                    "Erreur Critique",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
