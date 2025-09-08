using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des commandes PPG Live
    /// </summary>
    public class OrderApiService : BaseApiService
    {
        public OrderApiService(AuthenticationService authService) 
            : base(authService, "/orders")
        {
        }

        /// <summary>
        /// Simule une commande avant envoi
        /// </summary>
        /// <param name="order">Commande à simuler</param>
        /// <returns>Résultat de la simulation</returns>
        public async Task<ApiResponse<OrderSimulation>> SimulateOrderAsync(Order order)
        {
            Logger.Info($"Simulation de commande pour le client: {order.CustomerCode}");
            
            var request = new
            {
                customer_id = order.CustomerId,
                customer_code = order.CustomerCode,
                order_lines = order.Lines,
                delivery_date = order.DeliveryDate,
                notes = order.Notes,
                validate_stock = true,
                validate_pricing = true,
                validate_customer = true
            };

            return await PostAsync<OrderSimulation>("/simulate-order", request);
        }

        /// <summary>
        /// Envoie une commande réelle vers PPG Live
        /// </summary>
        /// <param name="order">Commande à envoyer</param>
        /// <returns>Commande créée</returns>
        public async Task<ApiResponse<Order>> SendOrderV2Async(Order order)
        {
            Logger.Info($"Envoi de commande réelle pour le client: {order.CustomerCode}");
            
            var request = new
            {
                customer_id = order.CustomerId,
                customer_code = order.CustomerCode,
                order_date = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                delivery_date = order.DeliveryDate?.ToString("yyyy-MM-dd"),
                delivery_address = order.DeliveryAddress,
                payment_terms = order.PaymentTerms,
                notes = order.Notes,
                order_lines = order.Lines,
                auto_confirm = false,
                send_notification = true
            };

            return await PostAsync<Order>("/send-order-v2", request);
        }

        /// <summary>
        /// Récupère une commande par son ID
        /// </summary>
        /// <param name="orderId">ID de la commande</param>
        /// <returns>Commande</returns>
        public async Task<ApiResponse<Order>> GetOrderAsync(string orderId)
        {
            Logger.Info($"Récupération de la commande: {orderId}");
            
            var request = new
            {
                order_id = orderId,
                include_lines = true,
                include_status_history = true
            };

            return await PostAsync<Order>("/get-order", request);
        }

        /// <summary>
        /// Récupère les commandes d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <param name="status">Statut des commandes</param>
        /// <returns>Liste des commandes</returns>
        public async Task<PagedApiResponse<Order>> GetCustomerOrdersAsync(string customerId, DateTime? fromDate = null, DateTime? toDate = null, string status = null)
        {
            Logger.Info($"Récupération des commandes du client: {customerId}");
            
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

            return await PostAsync<PagedApiResponse<Order>>("/get-customer-orders", request);
        }

        /// <summary>
        /// Annule une commande
        /// </summary>
        /// <param name="orderId">ID de la commande</param>
        /// <param name="reason">Raison de l'annulation</param>
        /// <returns>Résultat de l'annulation</returns>
        public async Task<ApiResponse<OrderCancellationResult>> CancelOrderAsync(string orderId, string reason)
        {
            Logger.Info($"Annulation de la commande: {orderId}");
            
            var request = new
            {
                order_id = orderId,
                cancellation_reason = reason,
                notify_customer = true
            };

            return await PostAsync<OrderCancellationResult>("/cancel-order", request);
        }

        /// <summary>
        /// Met à jour le statut d'une commande
        /// </summary>
        /// <param name="orderId">ID de la commande</param>
        /// <param name="status">Nouveau statut</param>
        /// <param name="notes">Notes optionnelles</param>
        /// <returns>Résultat de la mise à jour</returns>
        public async Task<ApiResponse<OrderStatusUpdateResult>> UpdateOrderStatusAsync(string orderId, string status, string notes = null)
        {
            Logger.Info($"Mise à jour du statut de la commande {orderId} vers: {status}");
            
            var request = new
            {
                order_id = orderId,
                status = status,
                notes = notes,
                notify_customer = true
            };

            return await PostAsync<OrderStatusUpdateResult>("/update-order-status", request);
        }

        /// <summary>
        /// Valide une commande avant envoi
        /// </summary>
        /// <param name="order">Commande à valider</param>
        /// <returns>Résultat de la validation</returns>
        public async Task<ApiResponse<OrderValidationResult>> ValidateOrderAsync(Order order)
        {
            Logger.Info($"Validation de la commande pour le client: {order.CustomerCode}");
            
            var request = new
            {
                customer_id = order.CustomerId,
                customer_code = order.CustomerCode,
                order_lines = order.Lines,
                validate_stock = true,
                validate_pricing = true,
                validate_customer = true,
                validate_delivery = true
            };

            return await PostAsync<OrderValidationResult>("/validate-order", request);
        }

        /// <summary>
        /// Récupère l'historique des statuts d'une commande
        /// </summary>
        /// <param name="orderId">ID de la commande</param>
        /// <returns>Historique des statuts</returns>
        public async Task<ApiResponse<List<OrderStatusHistory>>> GetOrderStatusHistoryAsync(string orderId)
        {
            Logger.Info($"Récupération de l'historique des statuts pour la commande: {orderId}");
            
            var request = new
            {
                order_id = orderId
            };

            return await PostAsync<List<OrderStatusHistory>>("/get-order-status-history", request);
        }
    }

    /// <summary>
    /// Résultat d'annulation de commande
    /// </summary>
    public class OrderCancellationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CancellationDate { get; set; }
        public string CancellationReason { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Résultat de mise à jour de statut
    /// </summary>
    public class OrderStatusUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string PreviousStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Résultat de validation de commande
    /// </summary>
    public class OrderValidationResult
    {
        public bool IsValid { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<OrderValidationIssue> Issues { get; set; } = new List<OrderValidationIssue>();
    }

    /// <summary>
    /// Problème de validation de commande
    /// </summary>
    public class OrderValidationIssue
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public string ProductCode { get; set; }
        public string Field { get; set; }
    }

    /// <summary>
    /// Historique des statuts de commande
    /// </summary>
    public class OrderStatusHistory
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
    }
}
