using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Configuration;
using PPGSage50Plugin.Models;
using PPGSage50Plugin.Services;

namespace PPGSage50Plugin
{
    /// <summary>
    /// Plugin principal pour l'intégration Sage 50 ERP avec PPG Live
    /// </summary>
    public class PPGSage50Plugin
    {
        private readonly SynchronizationService _syncService;
        private readonly AuthenticationService _authService;
        private readonly CustomerApiService _customerService;
        private readonly ProductApiService _productService;
        private readonly OrderApiService _orderService;
        private readonly InvoiceApiService _invoiceService;
        private readonly DeliveryApiService _deliveryService;
        private readonly QuotationApiService _quotationService;
        private readonly BackOrderApiService _backOrderService;
        private readonly PricingApiService _pricingService;

        public PPGSage50Plugin()
        {
            Logger.Initialize();
            Logger.Info("Initialisation du plugin PPG Sage 50");

            try
            {
                // Initialiser les services
                _authService = new AuthenticationService();
                _customerService = new CustomerApiService(_authService);
                _productService = new ProductApiService(_authService);
                _orderService = new OrderApiService(_authService);
                _invoiceService = new InvoiceApiService(_authService);
                _deliveryService = new DeliveryApiService(_authService);
                _quotationService = new QuotationApiService(_authService);
                _backOrderService = new BackOrderApiService(_authService);
                _pricingService = new PricingApiService(_authService);
                _syncService = new SynchronizationService();

                Logger.Info("Plugin PPG Sage 50 initialisé avec succès");
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Erreur lors de l'initialisation du plugin: {ex.Message}", ex);
                throw;
            }
        }

        #region Gestion des Clients

        /// <summary>
        /// Récupère les données d'un client depuis PPG Live
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <returns>Données du client</returns>
        public async Task<Customer> GetCustomerDataAsync(string customerId)
        {
            Logger.Info($"Récupération des données du client: {customerId}");
            
            var response = await _customerService.GetCustomerDataAsync(customerId);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la récupération du client {customerId}: {response.Message}");
                throw new Exception($"Échec de la récupération du client: {response.Message}");
            }
        }

        /// <summary>
        /// Synchronise les clients depuis PPG Live vers Sage 50
        /// </summary>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<SyncResult> SyncCustomersAsync()
        {
            return await _syncService.SynchronizeCustomersAsync();
        }

        /// <summary>
        /// Recherche des clients par critères
        /// </summary>
        /// <param name="searchCriteria">Critères de recherche</param>
        /// <returns>Liste des clients correspondants</returns>
        public async Task<List<Customer>> SearchCustomersAsync(CustomerSearchCriteria searchCriteria)
        {
            Logger.Info($"Recherche de clients avec critères");
            
            var response = await _customerService.SearchCustomersAsync(searchCriteria);
            if (response.Success)
            {
                return response.Data.Data;
            }
            else
            {
                Logger.Error($"Échec de la recherche de clients: {response.Message}");
                throw new Exception($"Échec de la recherche de clients: {response.Message}");
            }
        }

        #endregion

        #region Gestion des Tarifs

        /// <summary>
        /// Récupère les tarifs spécifiques d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="productCodes">Codes des produits</param>
        /// <returns>Tarifs spécifiques du client</returns>
        public async Task<List<CustomerPrice>> GetCustomerSpecificPriceAsync(string customerId, List<string> productCodes = null)
        {
            Logger.Info($"Récupération des tarifs spécifiques pour le client: {customerId}");
            
            var response = await _pricingService.GetCustomerSpecificPriceAsync(customerId, productCodes);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la récupération des tarifs pour le client {customerId}: {response.Message}");
                throw new Exception($"Échec de la récupération des tarifs: {response.Message}");
            }
        }

        /// <summary>
        /// Calcule le prix total d'une commande avec les tarifs spécifiques
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="orderLines">Lignes de commande</param>
        /// <returns>Calcul des prix</returns>
        public async Task<PriceCalculation> CalculateOrderPriceAsync(string customerId, List<OrderLine> orderLines)
        {
            Logger.Info($"Calcul des prix pour la commande du client: {customerId}");
            
            var response = await _pricingService.CalculateOrderPriceAsync(customerId, orderLines);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec du calcul des prix pour le client {customerId}: {response.Message}");
                throw new Exception($"Échec du calcul des prix: {response.Message}");
            }
        }

        #endregion

        #region Gestion des Commandes

        /// <summary>
        /// Simule une commande avant envoi
        /// </summary>
        /// <param name="order">Commande à simuler</param>
        /// <returns>Résultat de la simulation</returns>
        public async Task<OrderSimulation> SimulateOrderAsync(Order order)
        {
            Logger.Info($"Simulation de commande pour le client: {order.CustomerCode}");
            
            var response = await _orderService.SimulateOrderAsync(order);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la simulation de commande pour le client {order.CustomerCode}: {response.Message}");
                throw new Exception($"Échec de la simulation de commande: {response.Message}");
            }
        }

        /// <summary>
        /// Envoie une commande réelle vers PPG Live
        /// </summary>
        /// <param name="order">Commande à envoyer</param>
        /// <returns>Commande créée</returns>
        public async Task<Order> SendOrderV2Async(Order order)
        {
            Logger.Info($"Envoi de commande réelle pour le client: {order.CustomerCode}");
            
            var response = await _orderService.SendOrderV2Async(order);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de l'envoi de commande pour le client {order.CustomerCode}: {response.Message}");
                throw new Exception($"Échec de l'envoi de commande: {response.Message}");
            }
        }

        /// <summary>
        /// Synchronise les commandes depuis Sage 50 vers PPG Live
        /// </summary>
        /// <param name="orders">Commandes à synchroniser</param>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<SyncResult> SyncOrdersToPPGLiveAsync(List<Order> orders)
        {
            return await _syncService.SynchronizeOrdersToPPGLiveAsync(orders);
        }

        #endregion

        #region Gestion des Produits et Stock

        /// <summary>
        /// Récupère les informations de stock d'un produit
        /// </summary>
        /// <param name="productCode">Code du produit</param>
        /// <param name="warehouseId">ID de l'entrepôt</param>
        /// <returns>Informations de stock</returns>
        public async Task<ProductStock> GetStockInfoAsync(string productCode, string warehouseId = null)
        {
            Logger.Info($"Récupération des informations de stock pour le produit: {productCode}");
            
            var response = await _productService.GetStockInfoAsync(productCode, warehouseId);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la récupération du stock pour le produit {productCode}: {response.Message}");
                throw new Exception($"Échec de la récupération du stock: {response.Message}");
            }
        }

        /// <summary>
        /// Synchronise les produits depuis PPG Live vers Sage 50
        /// </summary>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<SyncResult> SyncProductsAsync()
        {
            return await _syncService.SynchronizeProductsAsync();
        }

        #endregion

        #region Gestion des Bons de Livraison

        /// <summary>
        /// Récupère un bon de livraison par son ID
        /// </summary>
        /// <param name="deliveryNoteId">ID du bon de livraison</param>
        /// <returns>Bon de livraison</returns>
        public async Task<DeliveryNote> GetDeliveryNoteAsync(string deliveryNoteId)
        {
            Logger.Info($"Récupération du bon de livraison: {deliveryNoteId}");
            
            var response = await _deliveryService.GetDeliveryNoteAsync(deliveryNoteId);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la récupération du bon de livraison {deliveryNoteId}: {response.Message}");
                throw new Exception($"Échec de la récupération du bon de livraison: {response.Message}");
            }
        }

        /// <summary>
        /// Récupère les informations de suivi d'une livraison
        /// </summary>
        /// <param name="trackingNumber">Numéro de suivi</param>
        /// <returns>Informations de suivi</returns>
        public async Task<TrackingInfo> GetTrackingInfoAsync(string trackingNumber)
        {
            Logger.Info($"Récupération des informations de suivi: {trackingNumber}");
            
            var response = await _deliveryService.GetTrackingInfoAsync(trackingNumber);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la récupération du suivi {trackingNumber}: {response.Message}");
                throw new Exception($"Échec de la récupération du suivi: {response.Message}");
            }
        }

        #endregion

        #region Gestion des Factures

        /// <summary>
        /// Récupère la liste des factures d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <returns>Liste des factures</returns>
        public async Task<List<InvoiceSummary>> GetInvoiceListAsync(string customerId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Logger.Info($"Récupération de la liste des factures du client: {customerId}");
            
            var response = await _invoiceService.GetInvoiceListAsync(customerId, fromDate, toDate);
            if (response.Success)
            {
                return response.Data.Data;
            }
            else
            {
                Logger.Error($"Échec de la récupération des factures pour le client {customerId}: {response.Message}");
                throw new Exception($"Échec de la récupération des factures: {response.Message}");
            }
        }

        /// <summary>
        /// Récupère les détails d'une facture spécifique
        /// </summary>
        /// <param name="invoiceId">ID de la facture</param>
        /// <returns>Détails de la facture</returns>
        public async Task<Invoice> GetInvoiceAsync(string invoiceId)
        {
            Logger.Info($"Récupération des détails de la facture: {invoiceId}");
            
            var response = await _invoiceService.GetInvoiceAsync(invoiceId);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la récupération de la facture {invoiceId}: {response.Message}");
                throw new Exception($"Échec de la récupération de la facture: {response.Message}");
            }
        }

        #endregion

        #region Gestion des Devis

        /// <summary>
        /// Crée un nouveau devis
        /// </summary>
        /// <param name="quotation">Données du devis</param>
        /// <returns>Devis créé</returns>
        public async Task<Quotation> CreateQuotationAsync(Quotation quotation)
        {
            Logger.Info($"Création d'un nouveau devis pour le client: {quotation.CustomerCode}");
            
            var response = await _quotationService.CreateQuotationAsync(quotation);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la création du devis pour le client {quotation.CustomerCode}: {response.Message}");
                throw new Exception($"Échec de la création du devis: {response.Message}");
            }
        }

        /// <summary>
        /// Synchronise les devis depuis Sage 50 vers PPG Live
        /// </summary>
        /// <param name="quotations">Devis à synchroniser</param>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<SyncResult> SyncQuotationsToPPGLiveAsync(List<Quotation> quotations)
        {
            return await _syncService.SynchronizeQuotationsToPPGLiveAsync(quotations);
        }

        #endregion

        #region Gestion des Back-Orders

        /// <summary>
        /// Soumet une demande de back-order
        /// </summary>
        /// <param name="backOrderRequest">Demande de back-order</param>
        /// <returns>Résultat de la demande</returns>
        public async Task<BackOrderResult> SubmitBackOrderRequestAsync(BackOrderRequest backOrderRequest)
        {
            Logger.Info($"Soumission d'une demande de back-order pour le client: {backOrderRequest.CustomerCode}");
            
            var response = await _backOrderService.SubmitBackOrderRequestAsync(backOrderRequest);
            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                Logger.Error($"Échec de la soumission du back-order pour le client {backOrderRequest.CustomerCode}: {response.Message}");
                throw new Exception($"Échec de la soumission du back-order: {response.Message}");
            }
        }

        #endregion

        #region Synchronisation Générale

        /// <summary>
        /// Synchronise toutes les données depuis PPG Live vers Sage 50
        /// </summary>
        /// <returns>Résultat de la synchronisation complète</returns>
        public async Task<SyncResult> SynchronizeAllAsync()
        {
            return await _syncService.SynchronizeAllAsync();
        }

        /// <summary>
        /// Valide la configuration du plugin
        /// </summary>
        /// <returns>True si la configuration est valide</returns>
        public bool ValidateConfiguration()
        {
            return AppConfig.ValidateConfiguration();
        }

        #endregion

        /// <summary>
        /// Dispose des ressources du plugin
        /// </summary>
        public void Dispose()
        {
            Logger.Info("Arrêt du plugin PPG Sage 50");
            
            _authService?.Dispose();
            _syncService?.Dispose();
            
            Logger.Info("Plugin PPG Sage 50 arrêté");
        }
    }
}
