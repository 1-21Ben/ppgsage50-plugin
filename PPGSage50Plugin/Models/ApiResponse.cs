using System;
using System.Collections.Generic;

namespace PPGSage50Plugin.Models
{
    /// <summary>
    /// Modèle générique pour les réponses API
    /// </summary>
    /// <typeparam name="T">Type de données retournées</typeparam>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string RequestId { get; set; }
    }

    /// <summary>
    /// Modèle pour les réponses de pagination
    /// </summary>
    /// <typeparam name="T">Type de données retournées</typeparam>
    public class PagedApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; } = new List<T>();
        public PaginationInfo Pagination { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string RequestId { get; set; }
    }

    /// <summary>
    /// Informations de pagination
    /// </summary>
    public class PaginationInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    /// <summary>
    /// Modèle pour les requêtes API
    /// </summary>
    public class ApiRequest
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public string ClientVersion { get; set; }
    }
}
