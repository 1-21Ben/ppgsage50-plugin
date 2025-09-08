using System;
using System.Collections.Generic;

namespace PPGSage50Plugin.Models
{
    /// <summary>
    /// Modèle représentant une facture dans le système PPG Live
    /// </summary>
    public class Invoice
    {
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string PaymentTerms { get; set; }
        public string OrderReference { get; set; }
        public string Notes { get; set; }
        public List<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string PdfUrl { get; set; }
    }

    /// <summary>
    /// Ligne de facture
    /// </summary>
    public class InvoiceLine
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TaxRate { get; set; }
        public decimal LineTotal { get; set; }
        public string Unit { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Résumé de facture pour les listes
    /// </summary>
    public class InvoiceSummary
    {
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Currency { get; set; }
    }
}
