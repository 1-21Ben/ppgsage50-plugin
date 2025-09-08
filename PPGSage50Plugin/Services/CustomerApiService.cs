using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service API pour la gestion des clients PPG Live
    /// </summary>
    public class CustomerApiService : BaseApiService
    {
        public CustomerApiService(AuthenticationService authService) 
            : base(authService, "/customers")
        {
        }

        /// <summary>
        /// Récupère les données d'un client spécifique
        /// </summary>
        /// <param name="customerId">ID du client</param>
        /// <returns>Données du client</returns>
        public async Task<ApiResponse<Customer>> GetCustomerDataAsync(string customerId)
        {
            Logger.Info($"Récupération des données du client: {customerId}");
            
            var request = new
            {
                customer_id = customerId,
                include_contacts = true,
                include_pricing = true
            };

            return await PostAsync<Customer>("/get-customer-data", request);
        }

        /// <summary>
        /// Récupère les données de plusieurs clients
        /// </summary>
        /// <param name="customerIds">Liste des IDs clients</param>
        /// <returns>Liste des clients</returns>
        public async Task<PagedApiResponse<Customer>> GetCustomersDataAsync(List<string> customerIds)
        {
            Logger.Info($"Récupération des données de {customerIds.Count} clients");
            
            var request = new
            {
                customer_ids = customerIds,
                include_contacts = true,
                include_pricing = true,
                page = 1,
                page_size = AppConfig.DefaultPageSize
            };

            return await PostAsync<PagedApiResponse<Customer>>("/get-customers-data", request);
        }

        /// <summary>
        /// Recherche des clients par critères
        /// </summary>
        /// <param name="searchCriteria">Critères de recherche</param>
        /// <returns>Liste des clients correspondants</returns>
        public async Task<PagedApiResponse<Customer>> SearchCustomersAsync(CustomerSearchCriteria searchCriteria)
        {
            Logger.Info($"Recherche de clients avec critères: {searchCriteria}");
            
            return await PostAsync<PagedApiResponse<Customer>>("/search-customers", searchCriteria);
        }

        /// <summary>
        /// Synchronise les clients depuis PPG Live vers Sage 50
        /// </summary>
        /// <param name="lastSyncDate">Date de dernière synchronisation</param>
        /// <returns>Résultat de la synchronisation</returns>
        public async Task<ApiResponse<SyncResult>> SyncCustomersAsync(DateTime? lastSyncDate = null)
        {
            Logger.Info($"Synchronisation des clients depuis PPG Live");
            
            var request = new
            {
                last_sync_date = lastSyncDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                sync_mode = "full",
                include_inactive = false
            };

            return await PostAsync<SyncResult>("/sync-customers", request);
        }

        /// <summary>
        /// Met à jour les informations d'un client
        /// </summary>
        /// <param name="customer">Données du client à mettre à jour</param>
        /// <returns>Client mis à jour</returns>
        public async Task<ApiResponse<Customer>> UpdateCustomerAsync(Customer customer)
        {
            Logger.Info($"Mise à jour du client: {customer.Code}");
            
            return await PutAsync<Customer>($"/{customer.Id}", customer);
        }

        /// <summary>
        /// Crée un nouveau client
        /// </summary>
        /// <param name="customer">Données du nouveau client</param>
        /// <returns>Client créé</returns>
        public async Task<ApiResponse<Customer>> CreateCustomerAsync(Customer customer)
        {
            Logger.Info($"Création d'un nouveau client: {customer.Code}");
            
            return await PostAsync<Customer>("/", customer);
        }

        /// <summary>
        /// Valide les données d'un client
        /// </summary>
        /// <param name="customer">Client à valider</param>
        /// <returns>Résultat de la validation</returns>
        public async Task<ApiResponse<ValidationResult>> ValidateCustomerAsync(Customer customer)
        {
            Logger.Info($"Validation du client: {customer.Code}");
            
            return await PostAsync<ValidationResult>("/validate-customer", customer);
        }
    }

    /// <summary>
    /// Critères de recherche pour les clients
    /// </summary>
    public class CustomerSearchCriteria
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    /// <summary>
    /// Résultat de synchronisation
    /// </summary>
    public class SyncResult
    {
        public int TotalRecords { get; set; }
        public int ProcessedRecords { get; set; }
        public int SuccessRecords { get; set; }
        public int ErrorRecords { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public DateTime SyncDate { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Résultat de validation
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
