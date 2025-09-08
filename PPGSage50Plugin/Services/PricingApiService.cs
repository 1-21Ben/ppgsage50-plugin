using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des tarifs PPG Live
    /// </summary>
    public class PricingApiService : BaseApiService
    {
        public PricingApiService(AuthenticationService authService) 
            : base(authService, "/pricing")
        {
        }

        /// <summary>
        /// Récupère les tarifs spécifiques d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="productCodes">Codes des produits</param>
        /// <returns>Tarifs spécifiques du client</returns>
        public async Task<ApiResponse<List<CustomerPrice>>> GetCustomerSpecificPriceAsync(string customerId, List<string> productCodes = null)
        {
            Logger.Info($"Récupération des tarifs spécifiques pour le client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                product_codes = productCodes,
                include_discounts = true,
                include_validity_periods = true
            };

            return await PostAsync<List<CustomerPrice>>("/get-customer-specific-price", request);
        }

        /// <summary>
        /// Récupère le tarif d'un produit spécifique pour un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="productCode">Code du produit</param>
        /// <returns>Tarif du produit</returns>
        public async Task<ApiResponse<CustomerPrice>> GetProductPriceAsync(string customerId, string productCode)
        {
            Logger.Info($"Récupération du tarif du produit {productCode} pour le client {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                product_code = productCode,
                include_discounts = true
            };

            return await PostAsync<CustomerPrice>("/get-product-price", request);
        }

        /// <summary>
        /// Calcule le prix total d'une commande avec les tarifs spécifiques
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="orderLines">Lignes de commande</param>
        /// <returns>Calcul des prix</returns>
        public async Task<ApiResponse<PriceCalculation>> CalculateOrderPriceAsync(string customerId, List<OrderLine> orderLines)
        {
            Logger.Info($"Calcul des prix pour la commande du client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                order_lines = orderLines,
                include_taxes = true,
                include_discounts = true
            };

            return await PostAsync<PriceCalculation>("/calculate-order-price", request);
        }

        /// <summary>
        /// Récupère les conditions commerciales d'un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <returns>Conditions commerciales</returns>
        public async Task<ApiResponse<CommercialConditions>> GetCommercialConditionsAsync(string customerId)
        {
            Logger.Info($"Récupération des conditions commerciales pour le client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                include_payment_terms = true,
                include_delivery_terms = true
            };

            return await PostAsync<CommercialConditions>("/get-commercial-conditions", request);
        }

        /// <summary>
        /// Valide les tarifs d'une commande
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="orderLines">Lignes de commande</param>
        /// <returns>Résultat de la validation</returns>
        public async Task<ApiResponse<PriceValidationResult>> ValidateOrderPricesAsync(string customerId, List<OrderLine> orderLines)
        {
            Logger.Info($"Validation des tarifs pour la commande du client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                order_lines = orderLines,
                validate_discounts = true,
                validate_validity = true
            };

            return await PostAsync<PriceValidationResult>("/validate-order-prices", request);
        }

        /// <summary>
        /// Récupère l'historique des tarifs d'un produit pour un client
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <param name="productCode">Code du produit</param>
        /// <param name="fromDate">Date de début</param>
        /// <param name="toDate">Date de fin</param>
        /// <returns>Historique des tarifs</returns>
        public async Task<ApiResponse<List<PriceHistory>>> GetPriceHistoryAsync(string customerId, string productCode, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Logger.Info($"Récupération de l'historique des tarifs pour le produit {productCode} et le client {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                product_code = productCode,
                from_date = fromDate?.ToString("yyyy-MM-dd"),
                to_date = toDate?.ToString("yyyy-MM-dd")
            };

            return await PostAsync<List<PriceHistory>>("/get-price-history", request);
        }
    }

    /// <summary>
    /// Calcul de prix pour une commande
    /// </summary>
    public class PriceCalculation
    {
        public decimal SubTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public List<PriceCalculationLine> Lines { get; set; } = new List<PriceCalculationLine>();
        public List<DiscountApplied> Discounts { get; set; } = new List<DiscountApplied>();
    }

    /// <summary>
    /// Ligne de calcul de prix
    /// </summary>
    public class PriceCalculationLine
    {
        public string ProductCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineDiscount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
    }

    /// <summary>
    /// Remise appliquée
    /// </summary>
    public class DiscountApplied
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Conditions commerciales
    /// </summary>
    public class CommercialConditions
    {
        public string CustomerId { get; set; }
        public string PaymentTerms { get; set; }
        public int PaymentDays { get; set; }
        public string DeliveryTerms { get; set; }
        public string Currency { get; set; }
        public decimal CreditLimit { get; set; }
        public List<DiscountRule> DiscountRules { get; set; } = new List<DiscountRule>();
    }

    /// <summary>
    /// Règle de remise
    /// </summary>
    public class DiscountRule
    {
        public string Type { get; set; }
        public decimal MinAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Résultat de validation des prix
    /// </summary>
    public class PriceValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<PriceValidationIssue> Issues { get; set; } = new List<PriceValidationIssue>();
    }

    /// <summary>
    /// Problème de validation de prix
    /// </summary>
    public class PriceValidationIssue
    {
        public string ProductCode { get; set; }
        public string IssueType { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
    }

    /// <summary>
    /// Historique des prix
    /// </summary>
    public class PriceHistory
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string ChangeReason { get; set; }
        public string UserId { get; set; }
    }
}
