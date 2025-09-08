using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des devis PPG Live
    /// </summary>
    public class QuotationApiService : BaseApiService
    {
        public QuotationApiService(AuthenticationService authService) 
            : base(authService, "/quotations")
        {
        }

        /// <summary>
        /// Crée un nouveau devis
        /// </summary>
        /// <param name="quotation">Données du devis</param>
        /// <returns>Devis créé</returns>
        public async Task<ApiResponse<Quotation>> CreateQuotationAsync(Quotation quotation)
        {
            Logger.Info($"Création d'un nouveau devis pour le client: {quotation.CustomerCode}");
            
            var request = new
            {
                customer_id = quotation.CustomerId,
                customer_code = quotation.CustomerCode,
                quotation_date = quotation.QuotationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                valid_until = quotation.ValidUntil.ToString("yyyy-MM-dd"),
                delivery_address = quotation.DeliveryAddress,
                payment_terms = quotation.PaymentTerms,
                notes = quotation.Notes,
                quotation_lines = quotation.Lines,
                auto_calculate = true,
                send_to_customer = false
            };

            return await PostAsync<Quotation>("/create-quotation", request);
        }

        /// <summary>
        /// Récupère un devis par son ID
        /// </summary>
        /// <param name="quotationId">ID du devis</param>
        /// <returns>Devis</returns>
        public async Task<ApiResponse<Quotation>> GetQuotationAsync(string quotationId)
        {
            Logger.Info($"Récupération du devis: {quotationId}");
            
            var request = new
            {
                quotation_id = quotationId,
                include_lines = true,
                include_pdf = true,
                include_status_history = true
            };

            return await PostAsync<Quotation>("/get-quotation", request);
        }

        /// <summary>
        /// Récupère un devis par son numéro
        /// </summary>
        /// <param name="quotationNumber">Numéro du devis</param>
        /// <returns>Devis</returns>
        public async Task<ApiResponse<Quotation>> GetQuotationByNumberAsync(string quotationNumber)
        {
            Logger.Info($"Récupération du devis par numéro: {quotationNumber}");
            
            var request = new
            {
                quotation_number = quotationNumber,
                include_lines = true,
                include_pdf = true
            };

            return await PostAsync<Quotation>("/get-quotation-by-number", request);
        }

        /// <summary>
        /// Récupère les devis d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <param name="status">Statut des devis</param>
        /// <returns>Liste des devis</returns>
        public async Task<PagedApiResponse<Quotation>> GetCustomerQuotationsAsync(string customerId, DateTime? fromDate = null, DateTime? toDate = null, string status = null)
        {
            Logger.Info($"Récupération des devis du client: {customerId}");
            
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

            return await PostAsync<PagedApiResponse<Quotation>>("/get-customer-quotations", request);
        }

        /// <summary>
        /// Met à jour un devis
        /// </summary>
        /// <param name="quotation">Données du devis à mettre à jour</param>
        /// <returns>Devis mis à jour</returns>
        public async Task<ApiResponse<Quotation>> UpdateQuotationAsync(Quotation quotation)
        {
            Logger.Info($"Mise à jour du devis: {quotation.QuotationNumber}");
            
            var request = new
            {
                quotation_id = quotation.Id,
                valid_until = quotation.ValidUntil.ToString("yyyy-MM-dd"),
                delivery_address = quotation.DeliveryAddress,
                payment_terms = quotation.PaymentTerms,
                notes = quotation.Notes,
                quotation_lines = quotation.Lines,
                auto_calculate = true
            };

            return await PutAsync<Quotation>($"/{quotation.Id}", request);
        }

        /// <summary>
        /// Convertit un devis en commande
        /// </summary>
        /// <param name="quotationId">ID du devis</param>
        /// <param name="conversionData">Données de conversion</param>
        /// <returns>Commande créée</returns>
        public async Task<ApiResponse<Order>> ConvertQuotationToOrderAsync(string quotationId, QuotationConversionData conversionData)
        {
            Logger.Info($"Conversion du devis {quotationId} en commande");
            
            var request = new
            {
                quotation_id = quotationId,
                conversion_data = conversionData,
                auto_confirm = false,
                send_notification = true
            };

            return await PostAsync<Order>("/convert-to-order", request);
        }

        /// <summary>
        /// Valide un devis
        /// </summary>
        /// <param name="quotationId">ID du devis</param>
        /// <param name="validationNotes">Notes de validation</param>
        /// <returns>Résultat de la validation</returns>
        public async Task<ApiResponse<QuotationValidationResult>> ValidateQuotationAsync(string quotationId, string validationNotes = null)
        {
            Logger.Info($"Validation du devis: {quotationId}");
            
            var request = new
            {
                quotation_id = quotationId,
                validation_notes = validationNotes,
                validate_pricing = true,
                validate_validity = true,
                validate_customer = true
            };

            return await PostAsync<QuotationValidationResult>("/validate-quotation", request);
        }

        /// <summary>
        /// Annule un devis
        /// </summary>
        /// <param name="quotationId">ID du devis</param>
        /// <param name="reason">Raison de l'annulation</param>
        /// <returns>Résultat de l'annulation</returns>
        public async Task<ApiResponse<QuotationCancellationResult>> CancelQuotationAsync(string quotationId, string reason)
        {
            Logger.Info($"Annulation du devis: {quotationId}");
            
            var request = new
            {
                quotation_id = quotationId,
                cancellation_reason = reason,
                notify_customer = true
            };

            return await PostAsync<QuotationCancellationResult>("/cancel-quotation", request);
        }

        /// <summary>
        /// Télécharge le PDF d'un devis
        /// </summary>
        /// <param name="quotationId">ID du devis</param>
        /// <returns>PDF du devis</returns>
        public async Task<ApiResponse<QuotationPdf>> DownloadQuotationPdfAsync(string quotationId)
        {
            Logger.Info($"Téléchargement du PDF du devis: {quotationId}");
            
            var request = new
            {
                quotation_id = quotationId,
                format = "pdf",
                include_signature = true
            };

            return await PostAsync<QuotationPdf>("/download-quotation-pdf", request);
        }

        /// <summary>
        /// Envoie un devis par email au client
        /// </summary>
        /// <param name="quotationId">ID du devis</param>
        /// <param name="emailData">Données d'email</param>
        /// <returns>Résultat de l'envoi</returns>
        public async Task<ApiResponse<EmailResult>> SendQuotationByEmailAsync(string quotationId, EmailData emailData)
        {
            Logger.Info($"Envoi du devis {quotationId} par email");
            
            var request = new
            {
                quotation_id = quotationId,
                email_data = emailData,
                include_pdf = true,
                track_opening = true
            };

            return await PostAsync<EmailResult>("/send-quotation-email", request);
        }

        /// <summary>
        /// Récupère l'historique des statuts d'un devis
        /// </summary>
        /// <param name="quotationId">ID du devis</param>
        /// <returns>Historique des statuts</returns>
        public async Task<ApiResponse<List<QuotationStatusHistory>>> GetQuotationStatusHistoryAsync(string quotationId)
        {
            Logger.Info($"Récupération de l'historique des statuts pour le devis: {quotationId}");
            
            var request = new
            {
                quotation_id = quotationId
            };

            return await PostAsync<List<QuotationStatusHistory>>("/get-quotation-status-history", request);
        }
    }

    /// <summary>
    /// Données de conversion de devis en commande
    /// </summary>
    public class QuotationConversionData
    {
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string PaymentTerms { get; set; }
        public string Notes { get; set; }
        public bool AutoConfirm { get; set; } = false;
    }

    /// <summary>
    /// Résultat de validation de devis
    /// </summary>
    public class QuotationValidationResult
    {
        public bool IsValid { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<QuotationValidationIssue> Issues { get; set; } = new List<QuotationValidationIssue>();
    }

    /// <summary>
    /// Problème de validation de devis
    /// </summary>
    public class QuotationValidationIssue
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public string ProductCode { get; set; }
        public string Field { get; set; }
    }

    /// <summary>
    /// Résultat d'annulation de devis
    /// </summary>
    public class QuotationCancellationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CancellationDate { get; set; }
        public string CancellationReason { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// PDF de devis
    /// </summary>
    public class QuotationPdf
    {
        public string QuotationId { get; set; }
        public string QuotationNumber { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string DownloadUrl { get; set; }
        public byte[] FileData { get; set; }
    }

    /// <summary>
    /// Données d'email
    /// </summary>
    public class EmailData
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
    }

    /// <summary>
    /// Résultat d'envoi d'email
    /// </summary>
    public class EmailResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string EmailId { get; set; }
        public DateTime SentDate { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Historique des statuts de devis
    /// </summary>
    public class QuotationStatusHistory
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
    }
}
