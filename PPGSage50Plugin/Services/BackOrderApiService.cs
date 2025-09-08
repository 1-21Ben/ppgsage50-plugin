using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des back-orders PPG Live
    /// </summary>
    public class BackOrderApiService : BaseApiService
    {
        public BackOrderApiService(AuthenticationService authService) 
            : base(authService, "/back-orders")
        {
        }

        /// <summary>
        /// Soumet une demande de back-order
        /// </summary>
        /// <param name="backOrderRequest">Demande de back-order</param>
        /// <returns>Résultat de la demande</returns>
        public async Task<ApiResponse<BackOrderResult>> SubmitBackOrderRequestAsync(BackOrderRequest backOrderRequest)
        {
            Logger.Info($"Soumission d'une demande de back-order pour le client: {backOrderRequest.CustomerCode}");
            
            var request = new
            {
                customer_id = backOrderRequest.CustomerId,
                customer_code = backOrderRequest.CustomerCode,
                requested_date = backOrderRequest.RequestedDate.ToString("yyyy-MM-dd"),
                notes = backOrderRequest.Notes,
                back_order_lines = backOrderRequest.Lines,
                priority = "normal",
                notify_customer = true
            };

            return await PostAsync<BackOrderResult>("/back-order-request", request);
        }

        /// <summary>
        /// Récupère une demande de back-order par son ID
        /// </summary>
        /// <param name="backOrderId">ID du back-order</param>
        /// <returns>Demande de back-order</returns>
        public async Task<ApiResponse<BackOrderRequest>> GetBackOrderRequestAsync(string backOrderId)
        {
            Logger.Info($"Récupération de la demande de back-order: {backOrderId}");
            
            var request = new
            {
                back_order_id = backOrderId,
                include_lines = true,
                include_status_history = true
            };

            return await PostAsync<BackOrderRequest>("/get-back-order-request", request);
        }

        /// <summary>
        /// Récupère les demandes de back-order d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <param name="status">Statut des demandes</param>
        /// <returns>Liste des demandes de back-order</returns>
        public async Task<PagedApiResponse<BackOrderRequest>> GetCustomerBackOrdersAsync(string customerId, DateTime? fromDate = null, DateTime? toDate = null, string status = null)
        {
            Logger.Info($"Récupération des demandes de back-order du client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                from_date = fromDate?.ToString("yyyy-MM-dd"),
                to_date = toDate?.ToString("yyyy-MM-dd"),
                status = status,
                page = 1,
                page_size = AppConfig.DefaultPageSize,
                include_lines = false
            };

            return await PostAsync<PagedApiResponse<BackOrderRequest>>("/get-customer-back-orders", request);
        }

        /// <summary>
        /// Met à jour le statut d'une demande de back-order
        /// </summary>
        /// <param name="backOrderId">ID du back-order</param>
        /// <param name="status">Nouveau statut</param>
        /// <param name="notes">Notes optionnelles</param>
        /// <returns>Résultat de la mise à jour</returns>
        public async Task<ApiResponse<BackOrderStatusUpdateResult>> UpdateBackOrderStatusAsync(string backOrderId, string status, string notes = null)
        {
            Logger.Info($"Mise à jour du statut du back-order {backOrderId} vers: {status}");
            
            var request = new
            {
                back_order_id = backOrderId,
                status = status,
                notes = notes,
                notify_customer = true
            };

            return await PostAsync<BackOrderStatusUpdateResult>("/update-back-order-status", request);
        }

        /// <summary>
        /// Annule une demande de back-order
        /// </summary>
        /// <param name="backOrderId">ID du back-order</param>
        /// <param name="reason">Raison de l'annulation</param>
        /// <returns>Résultat de l'annulation</returns>
        public async Task<ApiResponse<BackOrderCancellationResult>> CancelBackOrderAsync(string backOrderId, string reason)
        {
            Logger.Info($"Annulation du back-order: {backOrderId}");
            
            var request = new
            {
                back_order_id = backOrderId,
                cancellation_reason = reason,
                notify_customer = true
            };

            return await PostAsync<BackOrderCancellationResult>("/cancel-back-order", request);
        }

        /// <summary>
        /// Convertit un back-order en commande
        /// </summary>
        /// <param name="backOrderId">ID du back-order</param>
        /// <param name="conversionData">Données de conversion</param>
        /// <returns>Commande créée</returns>
        public async Task<ApiResponse<Order>> ConvertBackOrderToOrderAsync(string backOrderId, BackOrderConversionData conversionData)
        {
            Logger.Info($"Conversion du back-order {backOrderId} en commande");
            
            var request = new
            {
                back_order_id = backOrderId,
                conversion_data = conversionData,
                auto_confirm = false,
                send_notification = true
            };

            return await PostAsync<Order>("/convert-back-order-to-order", request);
        }

        /// <summary>
        /// Récupère les back-orders en attente de traitement
        /// </summary>
        /// <param name="priority">Priorité (optionnel)</param>
        /// <returns>Liste des back-orders en attente</returns>
        public async Task<PagedApiResponse<BackOrderRequest>> GetPendingBackOrdersAsync(string priority = null)
        {
            Logger.Info($"Récupération des back-orders en attente de traitement");
            
            var request = new
            {
                status = "pending",
                priority = priority,
                page = 1,
                page_size = AppConfig.DefaultPageSize,
                include_lines = false
            };

            return await PostAsync<PagedApiResponse<BackOrderRequest>>("/get-pending-back-orders", request);
        }

        /// <summary>
        /// Récupère l'historique des statuts d'un back-order
        /// </summary>
        /// <param name="backOrderId">ID du back-order</param>
        /// <returns>Historique des statuts</returns>
        public async Task<ApiResponse<List<BackOrderStatusHistory>>> GetBackOrderStatusHistoryAsync(string backOrderId)
        {
            Logger.Info($"Récupération de l'historique des statuts pour le back-order: {backOrderId}");
            
            var request = new
            {
                back_order_id = backOrderId
            };

            return await PostAsync<List<BackOrderStatusHistory>>("/get-back-order-status-history", request);
        }

        /// <summary>
        /// Valide une demande de back-order
        /// </summary>
        /// <param name="backOrderId">ID du back-order</param>
        /// <param name="validationNotes">Notes de validation</param>
        /// <returns>Résultat de la validation</returns>
        public async Task<ApiResponse<BackOrderValidationResult>> ValidateBackOrderAsync(string backOrderId, string validationNotes = null)
        {
            Logger.Info($"Validation du back-order: {backOrderId}");
            
            var request = new
            {
                back_order_id = backOrderId,
                validation_notes = validationNotes,
                validate_availability = true,
                validate_customer = true,
                validate_priority = true
            };

            return await PostAsync<BackOrderValidationResult>("/validate-back-order", request);
        }

        /// <summary>
        /// Récupère les statistiques des back-orders
        /// </summary>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <returns>Statistiques des back-orders</returns>
        public async Task<ApiResponse<BackOrderStatistics>> GetBackOrderStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            Logger.Info($"Récupération des statistiques des back-orders");
            
            var request = new
            {
                from_date = fromDate?.ToString("yyyy-MM-dd"),
                to_date = toDate?.ToString("yyyy-MM-dd"),
                include_customer_breakdown = true,
                include_product_breakdown = true
            };

            return await PostAsync<BackOrderStatistics>("/get-back-order-statistics", request);
        }
    }

    /// <summary>
    /// Résultat de demande de back-order
    /// </summary>
    public class BackOrderResult
    {
        public string BackOrderId { get; set; }
        public string BackOrderNumber { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Résultat de mise à jour de statut de back-order
    /// </summary>
    public class BackOrderStatusUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string PreviousStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Résultat d'annulation de back-order
    /// </summary>
    public class BackOrderCancellationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CancellationDate { get; set; }
        public string CancellationReason { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Données de conversion de back-order en commande
    /// </summary>
    public class BackOrderConversionData
    {
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string PaymentTerms { get; set; }
        public string Notes { get; set; }
        public bool AutoConfirm { get; set; } = false;
    }

    /// <summary>
    /// Résultat de validation de back-order
    /// </summary>
    public class BackOrderValidationResult
    {
        public bool IsValid { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<BackOrderValidationIssue> Issues { get; set; } = new List<BackOrderValidationIssue>();
    }

    /// <summary>
    /// Problème de validation de back-order
    /// </summary>
    public class BackOrderValidationIssue
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public string ProductCode { get; set; }
        public string Field { get; set; }
    }

    /// <summary>
    /// Historique des statuts de back-order
    /// </summary>
    public class BackOrderStatusHistory
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Statistiques des back-orders
    /// </summary>
    public class BackOrderStatistics
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int CancelledRequests { get; set; }
        public decimal AverageProcessingTime { get; set; }
        public List<BackOrderCustomerStats> CustomerBreakdown { get; set; } = new List<BackOrderCustomerStats>();
        public List<BackOrderProductStats> ProductBreakdown { get; set; } = new List<BackOrderProductStats>();
    }

    /// <summary>
    /// Statistiques par client
    /// </summary>
    public class BackOrderCustomerStats
    {
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public int RequestCount { get; set; }
        public decimal TotalValue { get; set; }
    }

    /// <summary>
    /// Statistiques par produit
    /// </summary>
    public class BackOrderProductStats
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int RequestCount { get; set; }
        public decimal TotalQuantity { get; set; }
    }
}
