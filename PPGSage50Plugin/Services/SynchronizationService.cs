using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Configuration;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service de synchronisation entre Sage 50 et PPG Live
    /// </summary>
    public class SynchronizationService
    {
        private readonly AuthenticationService _authService;
        private readonly CustomerApiService _customerService;
        private readonly ProductApiService _productService;
        private readonly OrderApiService _orderService;
        private readonly InvoiceApiService _invoiceService;
        private readonly DeliveryApiService _deliveryService;
        private readonly QuotationApiService _quotationService;
        private readonly BackOrderApiService _backOrderService;

        public SynchronizationService()
        {
            _authService = new AuthenticationService();
            _customerService = new CustomerApiService(_authService);
            _productService = new ProductApiService(_authService);
            _orderService = new OrderApiService(_authService);
            _invoiceService = new InvoiceApiService(_authService);
            _deliveryService = new DeliveryApiService(_authService);
            _quotationService = new QuotationApiService(_authService);
            _backOrderService = new BackOrderApiService(_authService);
        }

        /// <summary>
        /// Synchronise toutes les données depuis PPG Live vers Sage 50
        /// </summary>
        /// <returns>Résultat de la synchronisation complète</returns>
        public async Task<SyncResult> SynchronizeAllAsync()
        {
            Logger.Info("Début de la synchronisation complète avec PPG Live");
            var startTime = DateTime.UtcNow;
            var result = new SyncResult
            {
                SyncDate = startTime,
                Status = "InProgress"
            };

            try
            {
                // Vérifier l'authentification
                var isAuthenticated = await _authService.ValidateCredentialsAsync();
                if (!isAuthenticated)
                {
                    result.Status = "Failed";
                    result.Errors.Add("Échec de l'authentification avec PPG Live");
                    return result;
                }

                // Synchroniser les clients
                if (AppConfig.AutoSyncCustomers)
                {
                    Logger.Info("Synchronisation des clients");
                    var customerSyncResult = await SynchronizeCustomersAsync();
                    result.ProcessedRecords += customerSyncResult.ProcessedRecords;
                    result.SuccessRecords += customerSyncResult.SuccessRecords;
                    result.ErrorRecords += customerSyncResult.ErrorRecords;
                    result.Errors.AddRange(customerSyncResult.Errors);
                }

                // Synchroniser les produits
                if (AppConfig.AutoSyncProducts)
                {
                    Logger.Info("Synchronisation des produits");
                    var productSyncResult = await SynchronizeProductsAsync();
                    result.ProcessedRecords += productSyncResult.ProcessedRecords;
                    result.SuccessRecords += productSyncResult.SuccessRecords;
                    result.ErrorRecords += productSyncResult.ErrorRecords;
                    result.Errors.AddRange(productSyncResult.Errors);
                }

                // Mettre à jour la date de dernière synchronisation
                AppConfig.LastSyncDate = DateTime.UtcNow;

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                result.Status = result.ErrorRecords > 0 ? "CompletedWithErrors" : "Completed";
                result.TotalRecords = result.ProcessedRecords;

                Logger.LogSync("All", result.TotalRecords, result.Status == "Completed", (long)duration);
            }
            catch (Exception ex)
            {
                Logger.Error($"Erreur lors de la synchronisation complète: {ex.Message}", ex);
                result.Status = "Failed";
                result.Errors.Add($"Erreur inattendue: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Synchronise les clients depuis PPG Live
        /// </summary>
        /// <returns>Résultat de la synchronisation des clients</returns>
        public async Task<SyncResult> SynchronizeCustomersAsync()
        {
            Logger.Info("Début de la synchronisation des clients");
            var startTime = DateTime.UtcNow;

            try
            {
                var lastSyncDate = AppConfig.LastSyncDate == DateTime.MinValue ? null : AppConfig.LastSyncDate;
                var response = await _customerService.SyncCustomersAsync(lastSyncDate);

                if (response.Success)
                {
                    var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    Logger.LogSync("Customers", response.Data.TotalRecords, true, (long)duration);
                    return response.Data;
                }
                else
                {
                    Logger.Error($"Échec de la synchronisation des clients: {response.Message}");
                    return new SyncResult
                    {
                        Status = "Failed",
                        Errors = new List<string> { response.Message }
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Erreur lors de la synchronisation des clients: {ex.Message}", ex);
                return new SyncResult
                {
                    Status = "Failed",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Synchronise les produits depuis PPG Live
        /// </summary>
        /// <returns>Résultat de la synchronisation des produits</returns>
        public async Task<SyncResult> SynchronizeProductsAsync()
        {
            Logger.Info("Début de la synchronisation des produits");
            var startTime = DateTime.UtcNow;

            try
            {
                var lastSyncDate = AppConfig.LastSyncDate == DateTime.MinValue ? null : AppConfig.LastSyncDate;
                var response = await _productService.SyncProductsAsync(lastSyncDate);

                if (response.Success)
                {
                    var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    Logger.LogSync("Products", response.Data.TotalRecords, true, (long)duration);
                    return response.Data;
                }
                else
                {
                    Logger.Error($"Échec de la synchronisation des produits: {response.Message}");
                    return new SyncResult
                    {
                        Status = "Failed",
                        Errors = new List<string> { response.Message }
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Erreur lors de la synchronisation des produits: {ex.Message}", ex);
                return new SyncResult
                {
                    Status = "Failed",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Synchronise les commandes depuis Sage 50 vers PPG Live
        /// </summary>
        /// <param name="orders">Commandes à synchroniser</param>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<SyncResult> SynchronizeOrdersToPPGLiveAsync(List<Order> orders)
        {
            Logger.Info($"Début de la synchronisation de {orders.Count} commandes vers PPG Live");
            var startTime = DateTime.UtcNow;
            var result = new SyncResult
            {
                SyncDate = startTime,
                Status = "InProgress",
                TotalRecords = orders.Count
            };

            foreach (var order in orders)
            {
                try
                {
                    // Simuler la commande d'abord
                    var simulation = await _orderService.SimulateOrderAsync(order);
                    if (!simulation.Success || !simulation.Data.IsValid)
                    {
                        result.ErrorRecords++;
                        result.Errors.Add($"Échec de la simulation pour la commande {order.OrderNumber}: {simulation.Message}");
                        continue;
                    }

                    // Envoyer la commande réelle
                    var orderResponse = await _orderService.SendOrderV2Async(order);
                    if (orderResponse.Success)
                    {
                        result.SuccessRecords++;
                        Logger.Info($"Commande {order.OrderNumber} synchronisée avec succès");
                    }
                    else
                    {
                        result.ErrorRecords++;
                        result.Errors.Add($"Échec de l'envoi de la commande {order.OrderNumber}: {orderResponse.Message}");
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorRecords++;
                    result.Errors.Add($"Erreur lors de la synchronisation de la commande {order.OrderNumber}: {ex.Message}");
                    Logger.Error($"Erreur lors de la synchronisation de la commande {order.OrderNumber}: {ex.Message}", ex);
                }

                result.ProcessedRecords++;
            }

            var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
            result.Status = result.ErrorRecords > 0 ? "CompletedWithErrors" : "Completed";
            Logger.LogSync("Orders", result.TotalRecords, result.Status == "Completed", (long)duration);

            return result;
        }

        /// <summary>
        /// Synchronise les devis depuis Sage 50 vers PPG Live
        /// </summary>
        /// <param name="quotations">Devis à synchroniser</param>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<SyncResult> SynchronizeQuotationsToPPGLiveAsync(List<Quotation> quotations)
        {
            Logger.Info($"Début de la synchronisation de {quotations.Count} devis vers PPG Live");
            var startTime = DateTime.UtcNow;
            var result = new SyncResult
            {
                SyncDate = startTime,
                Status = "InProgress",
                TotalRecords = quotations.Count
            };

            foreach (var quotation in quotations)
            {
                try
                {
                    var response = await _quotationService.CreateQuotationAsync(quotation);
                    if (response.Success)
                    {
                        result.SuccessRecords++;
                        Logger.Info($"Devis {quotation.QuotationNumber} synchronisé avec succès");
                    }
                    else
                    {
                        result.ErrorRecords++;
                        result.Errors.Add($"Échec de la synchronisation du devis {quotation.QuotationNumber}: {response.Message}");
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorRecords++;
                    result.Errors.Add($"Erreur lors de la synchronisation du devis {quotation.QuotationNumber}: {ex.Message}");
                    Logger.Error($"Erreur lors de la synchronisation du devis {quotation.QuotationNumber}: {ex.Message}", ex);
                }

                result.ProcessedRecords++;
            }

            var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
            result.Status = result.ErrorRecords > 0 ? "CompletedWithErrors" : "Completed";
            Logger.LogSync("Quotations", result.TotalRecords, result.Status == "Completed", (long)duration);

            return result;
        }

        /// <summary>
        /// Récupère les données mises à jour depuis PPG Live
        /// </summary>
        /// <param name="entityType">Type d'entité à récupérer</param>
        /// <param name="lastSyncDate">Date de dernière synchronisation</param>
        /// <returns>Résultat de la récupération</returns>
        public async Task<SyncResult> GetUpdatedDataFromPPGLiveAsync(string entityType, DateTime? lastSyncDate = null)
        {
            Logger.Info($"Récupération des données mises à jour pour {entityType}");
            var startTime = DateTime.UtcNow;

            try
            {
                SyncResult result = null;

                switch (entityType.ToLower())
                {
                    case "customers":
                        result = await SynchronizeCustomersAsync();
                        break;
                    case "products":
                        result = await SynchronizeProductsAsync();
                        break;
                    default:
                        throw new ArgumentException($"Type d'entité non supporté: {entityType}");
                }

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                Logger.LogSync(entityType, result.TotalRecords, result.Status == "Completed", (long)duration);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Erreur lors de la récupération des données {entityType}: {ex.Message}", ex);
                return new SyncResult
                {
                    Status = "Failed",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Valide la configuration de synchronisation
        /// </summary>
        /// <returns>True si la configuration est valide</returns>
        public bool ValidateSyncConfiguration()
        {
            return AppConfig.ValidateConfiguration();
        }

        /// <summary>
        /// Dispose des ressources
        /// </summary>
        public void Dispose()
        {
            _authService?.Dispose();
        }
    }
}
