using System;
using System.Configuration;

namespace PPGSage50Plugin.Configuration
{
    /// <summary>
    /// Configuration de l'application pour le plugin PPG Sage 50
    /// </summary>
    public static class AppConfig
    {
        // Configuration API PPG Live
        public static string PPGLiveApiBaseUrl => ConfigurationManager.AppSettings["PPGLiveApiBaseUrl"] ?? "https://ppglive.fr/api";
        public static string PPGLiveApiKey => ConfigurationManager.AppSettings["PPGLiveApiKey"] ?? "";
        public static string PPGLiveApiSecret => ConfigurationManager.AppSettings["PPGLiveApiSecret"] ?? "";
        public static int ApiTimeoutSeconds => int.Parse(ConfigurationManager.AppSettings["ApiTimeoutSeconds"] ?? "30");
        public static int MaxRetryAttempts => int.Parse(ConfigurationManager.AppSettings["MaxRetryAttempts"] ?? "3");
        public static int RetryDelayMs => int.Parse(ConfigurationManager.AppSettings["RetryDelayMs"] ?? "1000");

        // Configuration Sage 50
        public static string Sage50DatabasePath => ConfigurationManager.AppSettings["Sage50DatabasePath"] ?? "";
        public static string Sage50CompanyCode => ConfigurationManager.AppSettings["Sage50CompanyCode"] ?? "";
        public static string Sage50UserId => ConfigurationManager.AppSettings["Sage50UserId"] ?? "";

        // Configuration Logging
        public static string LogLevel => ConfigurationManager.AppSettings["LogLevel"] ?? "INFO";
        public static string LogFilePath => ConfigurationManager.AppSettings["LogFilePath"] ?? @"C:\PPGSage50Plugin\Logs\";
        public static int MaxLogFileSizeMB => int.Parse(ConfigurationManager.AppSettings["MaxLogFileSizeMB"] ?? "10");
        public static int MaxLogFiles => int.Parse(ConfigurationManager.AppSettings["MaxLogFiles"] ?? "5");

        // Configuration Synchronisation
        public static bool AutoSyncCustomers => bool.Parse(ConfigurationManager.AppSettings["AutoSyncCustomers"] ?? "true");
        public static bool AutoSyncProducts => bool.Parse(ConfigurationManager.AppSettings["AutoSyncProducts"] ?? "true");
        public static int SyncIntervalMinutes => int.Parse(ConfigurationManager.AppSettings["SyncIntervalMinutes"] ?? "60");
        public static DateTime LastSyncDate
        {
            get
            {
                var lastSync = ConfigurationManager.AppSettings["LastSyncDate"];
                return DateTime.TryParse(lastSync, out var date) ? date : DateTime.MinValue;
            }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LastSyncDate"].Value = value.ToString("yyyy-MM-dd HH:mm:ss");
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        // Configuration Sécurité
        public static bool EnableDataEncryption => bool.Parse(ConfigurationManager.AppSettings["EnableDataEncryption"] ?? "true");
        public static string EncryptionKey => ConfigurationManager.AppSettings["EncryptionKey"] ?? "";
        public static bool ValidateSSLCertificates => bool.Parse(ConfigurationManager.AppSettings["ValidateSSLCertificates"] ?? "true");

        // Configuration Interface Utilisateur
        public static string DefaultLanguage => ConfigurationManager.AppSettings["DefaultLanguage"] ?? "fr-FR";
        public static bool ShowDebugInfo => bool.Parse(ConfigurationManager.AppSettings["ShowDebugInfo"] ?? "false");
        public static int DefaultPageSize => int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"] ?? "50");

        /// <summary>
        /// Valide la configuration de l'application
        /// </summary>
        /// <returns>True si la configuration est valide</returns>
        public static bool ValidateConfiguration()
        {
            var isValid = true;
            var errors = new List<string>();

            if (string.IsNullOrEmpty(PPGLiveApiBaseUrl))
            {
                errors.Add("URL de base de l'API PPG Live manquante");
                isValid = false;
            }

            if (string.IsNullOrEmpty(PPGLiveApiKey))
            {
                errors.Add("Clé API PPG Live manquante");
                isValid = false;
            }

            if (string.IsNullOrEmpty(Sage50DatabasePath))
            {
                errors.Add("Chemin de la base de données Sage 50 manquant");
                isValid = false;
            }

            if (ApiTimeoutSeconds <= 0)
            {
                errors.Add("Timeout API invalide");
                isValid = false;
            }

            if (!isValid)
            {
                Logger.Error($"Configuration invalide: {string.Join(", ", errors)}");
            }

            return isValid;
        }
    }
}
