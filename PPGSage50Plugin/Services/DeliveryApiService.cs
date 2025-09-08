using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des bons de livraison PPG Live
    /// </summary>
    public class DeliveryApiService : BaseApiService
    {
        public DeliveryApiService(AuthenticationService authService) 
            : base(authService, "/delivery")
        {
        }

        /// <summary>
        /// Récupère un bon de livraison par son ID
        /// </summary>
        /// <param name="deliveryNoteId">ID du bon de livraison</param>
        /// <returns>Bon de livraison</returns>
        public async Task<ApiResponse<DeliveryNote>> GetDeliveryNoteAsync(string deliveryNoteId)
        {
            Logger.Info($"Récupération du bon de livraison: {deliveryNoteId}");
            
            var request = new
            {
                delivery_note_id = deliveryNoteId,
                include_lines = true,
                include_tracking = true,
                include_pdf = true
            };

            return await PostAsync<DeliveryNote>("/get-delivery-note", request);
        }

        /// <summary>
        /// Récupère le bon de livraison associé à une commande
        /// </summary>
        /// <param name="orderId">ID de la commande</param>
        /// <returns>Bon de livraison</returns>
        public async Task<ApiResponse<DeliveryNote>> GetDeliveryNoteByOrderAsync(string orderId)
        {
            Logger.Info($"Récupération du bon de livraison pour la commande: {orderId}");
            
            var request = new
            {
                order_id = orderId,
                include_lines = true,
                include_tracking = true,
                include_pdf = true
            };

            return await PostAsync<DeliveryNote>("/get-delivery-note-by-order", request);
        }

        /// <summary>
        /// Récupère les bons de livraison d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <param name="status">Statut des livraisons</param>
        /// <returns>Liste des bons de livraison</returns>
        public async Task<PagedApiResponse<DeliveryNote>> GetCustomerDeliveryNotesAsync(string customerId, DateTime? fromDate = null, DateTime? toDate = null, string status = null)
        {
            Logger.Info($"Récupération des bons de livraison du client: {customerId}");
            
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

            return await PostAsync<PagedApiResponse<DeliveryNote>>("/get-customer-delivery-notes", request);
        }

        /// <summary>
        /// Récupère les informations de suivi d'une livraison
        /// </summary>
        /// <param name="trackingNumber">Numéro de suivi</param>
        /// <returns>Informations de suivi</returns>
        public async Task<ApiResponse<TrackingInfo>> GetTrackingInfoAsync(string trackingNumber)
        {
            Logger.Info($"Récupération des informations de suivi: {trackingNumber}");
            
            var request = new
            {
                tracking_number = trackingNumber,
                include_history = true,
                include_estimated_delivery = true
            };

            return await PostAsync<TrackingInfo>("/get-tracking-info", request);
        }

        /// <summary>
        /// Met à jour le statut d'une livraison
        /// </summary>
        /// <param name="deliveryNoteId">ID du bon de livraison</param>
        /// <param name="status">Nouveau statut</param>
        /// <param name="notes">Notes optionnelles</param>
        /// <returns>Résultat de la mise à jour</returns>
        public async Task<ApiResponse<DeliveryStatusUpdateResult>> UpdateDeliveryStatusAsync(string deliveryNoteId, string status, string notes = null)
        {
            Logger.Info($"Mise à jour du statut de livraison {deliveryNoteId} vers: {status}");
            
            var request = new
            {
                delivery_note_id = deliveryNoteId,
                status = status,
                notes = notes,
                notify_customer = true
            };

            return await PostAsync<DeliveryStatusUpdateResult>("/update-delivery-status", request);
        }

        /// <summary>
        /// Récupère l'historique des statuts d'une livraison
        /// </summary>
        /// <param name="deliveryNoteId">ID du bon de livraison</param>
        /// <returns>Historique des statuts</returns>
        public async Task<ApiResponse<List<DeliveryStatusHistory>>> GetDeliveryStatusHistoryAsync(string deliveryNoteId)
        {
            Logger.Info($"Récupération de l'historique des statuts pour la livraison: {deliveryNoteId}");
            
            var request = new
            {
                delivery_note_id = deliveryNoteId
            };

            return await PostAsync<List<DeliveryStatusHistory>>("/get-delivery-status-history", request);
        }

        /// <summary>
        /// Télécharge le PDF d'un bon de livraison
        /// </summary>
        /// <param name="deliveryNoteId">ID du bon de livraison</param>
        /// <returns>URL du PDF ou données binaires</returns>
        public async Task<ApiResponse<DeliveryNotePdf>> DownloadDeliveryNotePdfAsync(string deliveryNoteId)
        {
            Logger.Info($"Téléchargement du PDF du bon de livraison: {deliveryNoteId}");
            
            var request = new
            {
                delivery_note_id = deliveryNoteId,
                format = "pdf",
                include_signature = true
            };

            return await PostAsync<DeliveryNotePdf>("/download-delivery-note-pdf", request);
        }

        /// <summary>
        /// Valide une livraison
        /// </summary>
        /// <param name="deliveryNoteId">ID du bon de livraison</param>
        /// <param name="validationData">Données de validation</param>
        /// <returns>Résultat de la validation</returns>
        public async Task<ApiResponse<DeliveryValidationResult>> ValidateDeliveryAsync(string deliveryNoteId, DeliveryValidationData validationData)
        {
            Logger.Info($"Validation de la livraison: {deliveryNoteId}");
            
            var request = new
            {
                delivery_note_id = deliveryNoteId,
                validation_data = validationData,
                validate_quantities = true,
                validate_condition = true
            };

            return await PostAsync<DeliveryValidationResult>("/validate-delivery", request);
        }
    }

    /// <summary>
    /// Informations de suivi
    /// </summary>
    public class TrackingInfo
    {
        public string TrackingNumber { get; set; }
        public string CarrierName { get; set; }
        public string Status { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string CurrentLocation { get; set; }
        public List<TrackingEvent> Events { get; set; } = new List<TrackingEvent>();
        public string TrackingUrl { get; set; }
    }

    /// <summary>
    /// Événement de suivi
    /// </summary>
    public class TrackingEvent
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Résultat de mise à jour de statut de livraison
    /// </summary>
    public class DeliveryStatusUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string PreviousStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Historique des statuts de livraison
    /// </summary>
    public class DeliveryStatusHistory
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string Location { get; set; }
    }

    /// <summary>
    /// PDF de bon de livraison
    /// </summary>
    public class DeliveryNotePdf
    {
        public string DeliveryNoteId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string DownloadUrl { get; set; }
        public byte[] FileData { get; set; }
    }

    /// <summary>
    /// Données de validation de livraison
    /// </summary>
    public class DeliveryValidationData
    {
        public List<DeliveryValidationLine> Lines { get; set; } = new List<DeliveryValidationLine>();
        public string Condition { get; set; }
        public string Notes { get; set; }
        public string Signature { get; set; }
        public DateTime ValidationDate { get; set; }
    }

    /// <summary>
    /// Ligne de validation de livraison
    /// </summary>
    public class DeliveryValidationLine
    {
        public string ProductCode { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public string Condition { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Résultat de validation de livraison
    /// </summary>
    public class DeliveryValidationResult
    {
        public bool IsValid { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<DeliveryValidationIssue> Issues { get; set; } = new List<DeliveryValidationIssue>();
    }

    /// <summary>
    /// Problème de validation de livraison
    /// </summary>
    public class DeliveryValidationIssue
    {
        public string ProductCode { get; set; }
        public string IssueType { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
    }
}
