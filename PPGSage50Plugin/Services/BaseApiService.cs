using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PPGSage50Plugin.Configuration;
using PPGSage50Plugin.Models;

namespace PPGSage50Plugin.Services
{
    /// <summary>
    /// Service de base pour les appels API PPG Live
    /// </summary>
    public abstract class BaseApiService
    {
        protected readonly HttpClient _httpClient;
        protected readonly AuthenticationService _authService;
        protected readonly string _baseEndpoint;

        protected BaseApiService(AuthenticationService authService, string baseEndpoint)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _baseEndpoint = baseEndpoint ?? throw new ArgumentNullException(nameof(baseEndpoint));
            
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(AppConfig.PPGLiveApiBaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(AppConfig.ApiTimeoutSeconds);
        }

        /// <summary>
        /// Effectue un appel API GET
        /// </summary>
        /// <typeparam name="T">Type de réponse attendu</typeparam>
        /// <param name="endpoint">Endpoint à appeler</param>
        /// <param name="parameters">Paramètres de requête</param>
        /// <returns>Réponse API</returns>
        protected async Task<ApiResponse<T>> GetAsync<T>(string endpoint, object parameters = null)
        {
            return await ExecuteRequestAsync<T>(HttpMethod.Get, endpoint, parameters);
        }

        /// <summary>
        /// Effectue un appel API POST
        /// </summary>
        /// <typeparam name="T">Type de réponse attendu</typeparam>
        /// <param name="endpoint">Endpoint à appeler</param>
        /// <param name="data">Données à envoyer</param>
        /// <returns>Réponse API</returns>
        protected async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data = null)
        {
            return await ExecuteRequestAsync<T>(HttpMethod.Post, endpoint, data);
        }

        /// <summary>
        /// Effectue un appel API PUT
        /// </summary>
        /// <typeparam name="T">Type de réponse attendu</typeparam>
        /// <param name="endpoint">Endpoint à appeler</param>
        /// <param name="data">Données à envoyer</param>
        /// <returns>Réponse API</returns>
        protected async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data = null)
        {
            return await ExecuteRequestAsync<T>(HttpMethod.Put, endpoint, data);
        }

        /// <summary>
        /// Effectue un appel API DELETE
        /// </summary>
        /// <typeparam name="T">Type de réponse attendu</typeparam>
        /// <param name="endpoint">Endpoint à appeler</param>
        /// <returns>Réponse API</returns>
        protected async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
        {
            return await ExecuteRequestAsync<T>(HttpMethod.Delete, endpoint);
        }

        /// <summary>
        /// Exécute une requête HTTP avec gestion des erreurs et retry
        /// </summary>
        /// <typeparam name="T">Type de réponse attendu</typeparam>
        /// <param name="method">Méthode HTTP</param>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="data">Données à envoyer</param>
        /// <returns>Réponse API</returns>
        private async Task<ApiResponse<T>> ExecuteRequestAsync<T>(HttpMethod method, string endpoint, object data = null)
        {
            var requestId = Guid.NewGuid().ToString();
            var startTime = DateTime.UtcNow;
            var fullEndpoint = $"{_baseEndpoint}{endpoint}";

            Logger.Info($"Début appel API - {method.Method} {fullEndpoint} - RequestId: {requestId}");

            for (int attempt = 1; attempt <= AppConfig.MaxRetryAttempts; attempt++)
            {
                try
                {
                    using (var request = new HttpRequestMessage(method, fullEndpoint))
                    {
                        // Ajouter l'authentification
                        await _authService.AddAuthenticationHeadersAsync(request);

                        // Ajouter les headers standards
                        request.Headers.Add("X-Request-ID", requestId);
                        request.Headers.Add("X-Client-Version", "1.0.0");
                        request.Headers.Add("Accept", "application/json");

                        // Ajouter le contenu pour POST/PUT
                        if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put))
                        {
                            var json = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                DateFormatHandling = DateFormatHandling.IsoDateFormat
                            });
                            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                        }

                        // Exécuter la requête
                        var response = await _httpClient.SendAsync(request);
                        var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

                        // Lire la réponse
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            Logger.LogApiCall(fullEndpoint, method.Method, true, (long)duration, requestId);

                            // Désérialiser la réponse
                            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);
                            apiResponse.RequestId = requestId;
                            return apiResponse;
                        }
                        else
                        {
                            Logger.LogApiError(fullEndpoint, method.Method, (int)response.StatusCode, responseContent, requestId);
                            
                            // Retry si erreur temporaire
                            if (IsRetryableError(response.StatusCode) && attempt < AppConfig.MaxRetryAttempts)
                            {
                                Logger.Warning($"Tentative {attempt} échouée, retry dans {AppConfig.RetryDelayMs}ms");
                                await Task.Delay(AppConfig.RetryDelayMs * attempt);
                                continue;
                            }

                            return new ApiResponse<T>
                            {
                                Success = false,
                                Message = $"Erreur HTTP {response.StatusCode}: {responseContent}",
                                Errors = new List<string> { $"HTTP {response.StatusCode}" },
                                RequestId = requestId
                            };
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    Logger.LogApiError(fullEndpoint, method.Method, 0, ex.Message, requestId);

                    if (attempt < AppConfig.MaxRetryAttempts)
                    {
                        Logger.Warning($"Erreur de connexion, tentative {attempt}, retry dans {AppConfig.RetryDelayMs}ms");
                        await Task.Delay(AppConfig.RetryDelayMs * attempt);
                        continue;
                    }

                    return new ApiResponse<T>
                    {
                        Success = false,
                        Message = $"Erreur de connexion: {ex.Message}",
                        Errors = new List<string> { "CONNECTION_ERROR" },
                        RequestId = requestId
                    };
                }
                catch (TaskCanceledException ex)
                {
                    var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    Logger.LogApiError(fullEndpoint, method.Method, 0, "Timeout", requestId);

                    return new ApiResponse<T>
                    {
                        Success = false,
                        Message = "Timeout de la requête",
                        Errors = new List<string> { "TIMEOUT" },
                        RequestId = requestId
                    };
                }
                catch (Exception ex)
                {
                    var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    Logger.Error($"Erreur inattendue lors de l'appel API: {ex.Message}", ex);

                    return new ApiResponse<T>
                    {
                        Success = false,
                        Message = $"Erreur inattendue: {ex.Message}",
                        Errors = new List<string> { "UNEXPECTED_ERROR" },
                        RequestId = requestId
                    };
                }
            }

            return new ApiResponse<T>
            {
                Success = false,
                Message = "Nombre maximum de tentatives atteint",
                Errors = new List<string> { "MAX_RETRIES_EXCEEDED" },
                RequestId = requestId
            };
        }

        /// <summary>
        /// Détermine si une erreur HTTP est retryable
        /// </summary>
        /// <param name="statusCode">Code de statut HTTP</param>
        /// <returns>True si l'erreur est retryable</returns>
        private bool IsRetryableError(System.Net.HttpStatusCode statusCode)
        {
            return statusCode == System.Net.HttpStatusCode.RequestTimeout ||
                   statusCode == System.Net.HttpStatusCode.TooManyRequests ||
                   statusCode == System.Net.HttpStatusCode.InternalServerError ||
                   statusCode == System.Net.HttpStatusCode.BadGateway ||
                   statusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                   statusCode == System.Net.HttpStatusCode.GatewayTimeout;
        }

        /// <summary>
        /// Dispose des ressources
        /// </summary>
        public virtual void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
