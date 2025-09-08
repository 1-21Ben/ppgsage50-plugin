using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des produits et stocks PPG Live
    /// </summary>
    public class ProductApiService : BaseApiService
    {
        public ProductApiService(AuthenticationService authService) 
            : base(authService, "/products")
        {
        }

        /// <summary>
        /// Récupère les informations de stock d'un produit
        /// </summary>
        /// <param name="productCode">Code du produit</param>
        /// <param name="warehouseId">ID de l'entrepôt (optionnel)</param>
        /// <returns>Informations de stock</returns>
        public async Task<ApiResponse<ProductStock>> GetStockInfoAsync(string productCode, string warehouseId = null)
        {
            Logger.Info($"Récupération des informations de stock pour le produit: {productCode}");
            
            var request = new
            {
                product_code = productCode,
                warehouse_id = warehouseId,
                include_reserved = true,
                include_available = true
            };

            return await PostAsync<ProductStock>("/get-stock-info", request);
        }

        /// <summary>
        /// Récupère les informations de stock de plusieurs produits
        /// </summary>
        /// <param name="productCodes">Codes des produits</param>
        /// <param name="warehouseId">ID de l'entrepôt (optionnel)</param>
        /// <returns>Informations de stock</returns>
        public async Task<ApiResponse<List<ProductStock>>> GetMultipleStockInfoAsync(List<string> productCodes, string warehouseId = null)
        {
            Logger.Info($"Récupération des informations de stock pour {productCodes.Count} produits");
            
            var request = new
            {
                product_codes = productCodes,
                warehouse_id = warehouseId,
                include_reserved = true,
                include_available = true
            };

            return await PostAsync<List<ProductStock>>("/get-multiple-stock-info", request);
        }

        /// <summary>
        /// Récupère les détails d'un produit
        /// </summary>
        /// <param name="productCode">Code du produit</param>
        /// <returns>Détails du produit</returns>
        public async Task<ApiResponse<Product>> GetProductDetailsAsync(string productCode)
        {
            Logger.Info($"Récupération des détails du produit: {productCode}");
            
            var request = new
            {
                product_code = productCode,
                include_stock = true,
                include_pricing = true,
                include_specifications = true
            };

            return await PostAsync<Product>("/get-product-details", request);
        }

        /// <summary>
        /// Recherche des produits par critères
        /// </summary>
        /// <param name="searchCriteria">Critères de recherche</param>
        /// <returns>Liste des produits</returns>
        public async Task<PagedApiResponse<Product>> SearchProductsAsync(ProductSearchCriteria searchCriteria)
        {
            Logger.Info($"Recherche de produits avec critères: {searchCriteria}");
            
            return await PostAsync<PagedApiResponse<Product>>("/search-products", searchCriteria);
        }

        /// <summary>
        /// Synchronise les produits depuis PPG Live vers Sage 50
        /// </summary>
        /// <param name="lastSyncDate">Date de dernière synchronisation</param>
        /// <param name="category">Catégorie de produits (optionnel)</param>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<ApiResponse<SyncResult>> SyncProductsAsync(DateTime? lastSyncDate = null, string category = null)
        {
            Logger.Info($"Synchronisation des produits depuis PPG Live");
            
            var request = new
            {
                last_sync_date = lastSyncDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                category = category,
                sync_mode = "full",
                include_inactive = false,
                include_stock = true
            };

            return await PostAsync<SyncResult>("/sync-products", request);
        }

        /// <summary>
        /// Vérifie la disponibilité d'un produit pour une quantité donnée
        /// </summary>
        /// <param name="productCode">Code du produit</param>
        /// <param name="quantity">Quantité demandée</param>
        /// <param name="warehouseId">ID de l'entrepôt (optionnel)</param>
        /// <returns>Résultat de la vérification</returns>
        public async Task<ApiResponse<AvailabilityCheck>> CheckProductAvailabilityAsync(string productCode, decimal quantity, string warehouseId = null)
        {
            Logger.Info($"Vérification de la disponibilité du produit {productCode} pour {quantity} unités");
            
            var request = new
            {
                product_code = productCode,
                quantity = quantity,
                warehouse_id = warehouseId,
                include_lead_time = true,
                include_alternatives = true
            };

            return await PostAsync<AvailabilityCheck>("/check-product-availability", request);
        }

        /// <summary>
        /// Récupère les mouvements de stock d'un produit
        /// </summary>
        /// <param name="productCode">Code du produit</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <returns>Mouvements de stock</returns>
        public async Task<ApiResponse<List<StockMovement>>> GetStockMovementsAsync(string productCode, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Logger.Info($"Récupération des mouvements de stock pour le produit: {productCode}");
            
            var request = new
            {
                product_code = productCode,
                from_date = fromDate?.ToString("yyyy-MM-dd"),
                to_date = toDate?.ToString("yyyy-MM-dd"),
                include_all_types = true
            };

            return await PostAsync<List<StockMovement>>("/get-stock-movements", request);
        }

        /// <summary>
        /// Récupère les produits en rupture de stock
        /// </summary>
        /// <param name="warehouseId">ID de l'entrepôt (optionnel)</param>
        /// <returns>Liste des produits en rupture</returns>
        public async Task<ApiResponse<List<ProductStock>>> GetOutOfStockProductsAsync(string warehouseId = null)
        {
            Logger.Info($"Récupération des produits en rupture de stock");
            
            var request = new
            {
                warehouse_id = warehouseId,
                include_zero_stock = true,
                include_negative_stock = true
            };

            return await PostAsync<List<ProductStock>>("/get-out-of-stock-products", request);
        }
    }

    /// <summary>
    /// Critères de recherche pour les produits
    /// </summary>
    public class ProductSearchCriteria
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public bool? InStock { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    /// <summary>
    /// Vérification de disponibilité
    /// </summary>
    public class AvailabilityCheck
    {
        public string ProductCode { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public int LeadTimeDays { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public List<ProductAlternative> Alternatives { get; set; } = new List<ProductAlternative>();
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }

    /// <summary>
    /// Produit alternatif
    /// </summary>
    public class ProductAlternative
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal PriceDifference { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Mouvement de stock
    /// </summary>
    public class StockMovement
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        public decimal Quantity { get; set; }
        public decimal Balance { get; set; }
        public string WarehouseId { get; set; }
        public string Notes { get; set; }
    }
}
