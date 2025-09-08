using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des factures PPG Live
    /// </summary>
    public class InvoiceApiService : BaseApiService
    {
        public InvoiceApiService(AuthenticationService authService) 
            : base(authService, "/invoices")
        {
        }

        /// <summary>
        /// Récupère la liste des factures d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <param name="status">Statut des factures</param>
        /// <returns>Liste des factures</returns>
        public async Task<PagedApiResponse<InvoiceSummary>> GetInvoiceListAsync(string customerId, DateTime? fromDate = null, DateTime? toDate = null, string status = null)
        {
            Logger.Info($"Récupération de la liste des factures du client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                from_date = fromDate?.ToString("yyyy-MM-dd"),
                to_date = toDate?.ToString("yyyy-MM-dd"),
                status = status,
                page = 1,
                page_size = AppConfig.DefaultPageSize,
                include_summary_only = true
            };

            return await PostAsync<PagedApiResponse<InvoiceSummary>>("/get-invoice-list", request);
        }

        /// <summary>
        /// Récupère les détails d'une facture spécifique
        /// </summary>
        /// <param name="invoiceId">ID de la facture</param>
        /// <returns>Détails de la facture</returns>
        public async Task<ApiResponse<Invoice>> GetInvoiceAsync(string invoiceId)
        {
            Logger.Info($"Récupération des détails de la facture: {invoiceId}");
            
            var request = new
            {
                invoice_id = invoiceId,
                include_lines = true,
                include_payments = true,
                include_pdf = true
            };

            return await PostAsync<Invoice>("/get-invoice", request);
        }

        /// <summary>
        /// Récupère une facture par son numéro
        /// </summary>
        /// <param name="invoiceNumber">Numéro de la facture</param>
        /// <returns>Détails de la facture</returns>
        public async Task<ApiResponse<Invoice>> GetInvoiceByNumberAsync(string invoiceNumber)
        {
            Logger.Info($"Récupération de la facture par numéro: {invoiceNumber}");
            
            var request = new
            {
                invoice_number = invoiceNumber,
                include_lines = true,
                include_payments = true,
                include_pdf = true
            };

            return await PostAsync<Invoice>("/get-invoice-by-number", request);
        }

        /// <summary>
        /// Récupère les factures en attente de paiement
        /// </summary>
        /// <param name="customerId">ID du client (optionnel)</param>
        /// <param name="overdueOnly">Seulement les factures en retard</param>
        /// <returns>Liste des factures en attente</returns>
        public async Task<PagedApiResponse<InvoiceSummary>> GetPendingInvoicesAsync(string customerId = null, bool overdueOnly = false)
        {
            Logger.Info($"Récupération des factures en attente de paiement");
            
            var request = new
            {
                customer_id = customerId,
                overdue_only = overdueOnly,
                status = "pending",
                page = 1,
                page_size = AppConfig.DefaultPageSize
            };

            return await PostAsync<PagedApiResponse<InvoiceSummary>>("/get-pending-invoices", request);
        }

        /// <summary>
        /// Enregistre un paiement pour une facture
        /// </summary>
        /// <param name="invoiceId">ID de la facture</param>
        /// <param name="paymentData">Données du paiement</param>
        /// <returns>Résultat du paiement</returns>
        public async Task<ApiResponse<PaymentResult>> RecordPaymentAsync(string invoiceId, PaymentData paymentData)
        {
            Logger.Info($"Enregistrement d'un paiement pour la facture: {invoiceId}");
            
            var request = new
            {
                invoice_id = invoiceId,
                payment_data = paymentData,
                update_status = true,
                send_receipt = true
            };

            return await PostAsync<PaymentResult>("/record-payment", request);
        }

        /// <summary>
        /// Annule une facture
        /// </summary>
        /// <param name="invoiceId">ID de la facture</param>
        /// <param name="reason">Raison de l'annulation</param>
        /// <returns>Résultat de l'annulation</returns>
        public async Task<ApiResponse<InvoiceCancellationResult>> CancelInvoiceAsync(string invoiceId, string reason)
        {
            Logger.Info($"Annulation de la facture: {invoiceId}");
            
            var request = new
            {
                invoice_id = invoiceId,
                cancellation_reason = reason,
                notify_customer = true,
                create_credit_note = true
            };

            return await PostAsync<InvoiceCancellationResult>("/cancel-invoice", request);
        }

        /// <summary>
        /// Télécharge le PDF d'une facture
        /// </summary>
        /// <param name="invoiceId">ID de la facture</param>
        /// <returns>PDF de la facture</returns>
        public async Task<ApiResponse<InvoicePdf>> DownloadInvoicePdfAsync(string invoiceId)
        {
            Logger.Info($"Téléchargement du PDF de la facture: {invoiceId}");
            
            var request = new
            {
                invoice_id = invoiceId,
                format = "pdf",
                include_signature = true
            };

            return await PostAsync<InvoicePdf>("/download-invoice-pdf", request);
        }

        /// <summary>
        /// Récupère l'historique des paiements d'une facture
        /// </summary>
        /// <param name="invoiceId">ID de la facture</param>
        /// <returns>Historique des paiements</returns>
        public async Task<ApiResponse<List<PaymentHistory>>> GetPaymentHistoryAsync(string invoiceId)
        {
            Logger.Info($"Récupération de l'historique des paiements pour la facture: {invoiceId}");
            
            var request = new
            {
                invoice_id = invoiceId
            };

            return await PostAsync<List<PaymentHistory>>("/get-payment-history", request);
        }

        /// <summary>
        /// Génère un relevé de factures pour un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <returns>Relevé de factures</returns>
        public async Task<ApiResponse<InvoiceStatement>> GenerateInvoiceStatementAsync(string customerId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Logger.Info($"Génération du relevé de factures pour le client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                from_date = fromDate?.ToString("yyyy-MM-dd"),
                to_date = toDate?.ToString("yyyy-MM-dd"),
                include_payments = true,
                include_outstanding = true
            };

            return await PostAsync<InvoiceStatement>("/generate-invoice-statement", request);
        }
    }

    /// <summary>
    /// Données de paiement
    /// </summary>
    public class PaymentData
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public string BankAccount { get; set; }
    }

    /// <summary>
    /// Résultat de paiement
    /// </summary>
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string NewStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Résultat d'annulation de facture
    /// </summary>
    public class InvoiceCancellationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CancellationDate { get; set; }
        public string CancellationReason { get; set; }
        public string CreditNoteNumber { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// PDF de facture
    /// </summary>
    public class InvoicePdf
    {
        public string InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string DownloadUrl { get; set; }
        public byte[] FileData { get; set; }
    }

    /// <summary>
    /// Historique des paiements
    /// </summary>
    public class PaymentHistory
    {
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Relevé de factures
    /// </summary>
    public class InvoiceStatement
    {
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalInvoiced { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalOutstanding { get; set; }
        public List<InvoiceSummary> Invoices { get; set; } = new List<InvoiceSummary>();
        public List<PaymentHistory> Payments { get; set; } = new List<PaymentHistory>();
        public string PdfUrl { get; set; }
    }
}
