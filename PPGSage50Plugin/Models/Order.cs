using System;
using System.Collections.Generic;

namespace PPGSage50Plugin.Models
{
    /// <summary>
    /// Modèle représentant une commande dans le système PPG Live
    /// </summary>
    public class Order
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryAddress { get; set; }
        public string Notes { get; set; }
        public List<OrderLine> Lines { get; set; } = new List<OrderLine>();
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    /// <summary>
    /// Ligne de commande
    /// </summary>
    public class OrderLine
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

    /// <summary>
    /// Résultat de simulation de commande
    /// </summary>
    public class OrderSimulation
    {
        public bool IsValid { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public List<OrderLine> Lines { get; set; } = new List<OrderLine>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// Demande de back-order
    /// </summary>
    public class BackOrderRequest
    {
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Notes { get; set; }
        public List<BackOrderLine> Lines { get; set; } = new List<BackOrderLine>();
    }

    /// <summary>
    /// Ligne de back-order
    /// </summary>
    public class BackOrderLine
    {
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string Notes { get; set; }
    }
}
