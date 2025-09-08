using System;
using System.Collections.Generic;

namespace PPGSage50Plugin.Models
{
    /// <summary>
    /// Modèle représentant un bon de livraison dans le système PPG Live
    /// </summary>
    public class DeliveryNote
    {
        public string Id { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string CarrierName { get; set; }
        public string TrackingNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string Notes { get; set; }
        public List<DeliveryNoteLine> Lines { get; set; } = new List<DeliveryNoteLine>();
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string PdfUrl { get; set; }
    }

    /// <summary>
    /// Ligne de bon de livraison
    /// </summary>
    public class DeliveryNoteLine
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public string Unit { get; set; }
        public string Notes { get; set; }
    }
}
