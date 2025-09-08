using System;
using System.Collections.Generic;

namespace PPGSage50Plugin.Models
{
    /// <summary>
    /// Modèle représentant un produit dans le système PPG Live
    /// </summary>
    public class Product
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
        public string Unit { get; set; }
        public decimal Weight { get; set; }
        public string WeightUnit { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public List<ProductStock> Stock { get; set; } = new List<ProductStock>();
    }

    /// <summary>
    /// Information de stock pour un produit
    /// </summary>
    public class ProductStock
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Prix spécifique pour un client
    /// </summary>
    public class CustomerPrice
    {
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
