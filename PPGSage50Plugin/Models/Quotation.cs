using System;
using System.Collections.Generic;

namespace PPGSage50Plugin.Models
{
    /// <summary>
    /// Modèle représentant un devis dans le système PPG Live
    /// </summary>
    public class Quotation
    {
        public string Id { get; set; }
        public string QuotationNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryAddress { get; set; }
        public string Notes { get; set; }
        public List<QuotationLine> Lines { get; set; } = new List<QuotationLine>();
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string PdfUrl { get; set; }
    }

    /// <summary>
    /// Ligne de devis
    /// </summary>
    public class QuotationLine
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal LineTotal { get; set; }
        public string Unit { get; set; }
        public string Notes { get; set; }
    }
}
