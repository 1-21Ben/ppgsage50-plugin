using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using PPGSage50Plugin.Configuration;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service de journalisation pour le plugin PPG Sage 50
    /// </summary>
    public static class Logger
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Logger));
        private static bool _isInitialized = false;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Initialise le système de logging
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            lock (_lockObject)
            {
                if (_isInitialized) return;

                try
                {
                    // Créer le répertoire de logs s'il n'existe pas
                    var logDirectory = AppConfig.LogFilePath;
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    // Configuration log4net
                    var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
                    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                    _isInitialized = true;
                    Info("Système de logging initialisé");
                }
                catch (Exception ex)
                {
                    // Fallback vers la console si la configuration échoue
                    Console.WriteLine($"Erreur lors de l'initialisation du logging: {ex.Message}");
                    _isInitialized = true;
                }
            }
        }

        /// <summary>
        /// Log un message de niveau Debug
        /// </summary>
        /// <param name="message">Message à logger</param>
        /// <param name="exception">Exception optionnelle</param>
        public static void Debug(string message, Exception exception = null)
        {
            Initialize();
            if (exception != null)
                _logger.Debug(message, exception);
            else
                _logger.Debug(message);
        }

        /// <summary>
        /// Log un message de niveau Info
        /// </summary>
        /// <param name="message">Message à logger</param>
        /// <param name="exception">Exception optionnelle</param>
        public static void Info(string message, Exception exception = null)
        {
            Initialize();
            if (exception != null)
                _logger.Info(message, exception);
            else
                _logger.Info(message);
        }

        /// <summary>
        /// Log un message de niveau Warning
        /// </summary>
        /// <param name="message">Message à logger</param>
        /// <param name="exception">Exception optionnelle</param>
        public static void Warning(string message, Exception exception = null)
        {
            Initialize();
            if (exception != null)
                _logger.Warn(message, exception);
            else
                _logger.Warn(message);
        }

        /// <summary>
        /// Log un message de niveau Error
        /// </summary>
        /// <param name="message">Message à logger</param>
        /// <param name="exception">Exception optionnelle</param>
        public static void Error(string message, Exception exception = null)
        {
            Initialize();
            if (exception != null)
                _logger.Error(message, exception);
            else
                _logger.Error(message);
        }

        /// <summary>
        /// Log un message de niveau Fatal
        /// </summary>
        /// <param name="message">Message à logger</param>
        /// <param name="exception">Exception optionnelle</param>
        public static void Fatal(string message, Exception exception = null)
        {
            Initialize();
            if (exception != null)
                _logger.Fatal(message, exception);
            else
                _logger.Fatal(message);
        }

        /// <summary>
        /// Log une activité API
        /// </summary>
        /// <param name="endpoint">Endpoint appelé</param>
        /// <param name="method">Méthode HTTP</param>
        /// <param name="success">Succès de l'appel</param>
        /// <param name="duration">Durée de l'appel en ms</param>
        /// <param name="requestId">ID de la requête</param>
        public static void LogApiCall(string endpoint, string method, bool success, long duration, string requestId = null)
        {
            var message = $"API Call - {method} {endpoint} - Success: {success} - Duration: {duration}ms";
            if (!string.IsNullOrEmpty(requestId))
                message += $" - RequestId: {requestId}";

            if (success)
                Info(message);
            else
                Warning(message);
        }

        /// <summary>
        /// Log une erreur API
        /// </summary>
        /// <param name="endpoint">Endpoint appelé</param>
        /// <param name="method">Méthode HTTP</param>
        /// <param name="statusCode">Code de statut HTTP</param>
        /// <param name="errorMessage">Message d'erreur</param>
        /// <param name="requestId">ID de la requête</param>
        public static void LogApiError(string endpoint, string method, int statusCode, string errorMessage, string requestId = null)
        {
            var message = $"API Error - {method} {endpoint} - Status: {statusCode} - Error: {errorMessage}";
            if (!string.IsNullOrEmpty(requestId))
                message += $" - RequestId: {requestId}";

            Error(message);
        }

        /// <summary>
        /// Log une synchronisation
        /// </summary>
        /// <param name="entityType">Type d'entité synchronisée</param>
        /// <param name="recordsCount">Nombre d'enregistrements</param>
        /// <param name="success">Succès de la synchronisation</param>
        /// <param name="duration">Durée en ms</param>
        public static void LogSync(string entityType, int recordsCount, bool success, long duration)
        {
            var message = $"Sync - {entityType} - Records: {recordsCount} - Success: {success} - Duration: {duration}ms";
            
            if (success)
                Info(message);
            else
                Error(message);
        }
    }
}
