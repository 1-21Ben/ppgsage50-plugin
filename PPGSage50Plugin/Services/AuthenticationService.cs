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
    /// Service d'authentification pour l'API PPG Live
    /// </summary>
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private DateTime _tokenExpiry;
        private readonly object _lockObject = new object();

        public AuthenticationService(HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri(AppConfig.PPGLiveApiBaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(AppConfig.ApiTimeoutSeconds);
        }

        /// <summary>
        /// Obtient un token d'accès valide
        /// </summary>
        /// <returns>Token d'accès</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            lock (_lockObject)
            {
                // Vérifier si le token est encore valide (avec une marge de 5 minutes)
                if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiry > DateTime.UtcNow.AddMinutes(5))
                {
                    return _accessToken;
                }
            }

            try
            {
                Logger.Info("Demande d'un nouveau token d'accès à l'API PPG Live");

                var authRequest = new
                {
                    api_key = AppConfig.PPGLiveApiKey,
                    api_secret = AppConfig.PPGLiveApiSecret,
                    grant_type = "client_credentials"
                };

                var json = JsonConvert.SerializeObject(authRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/auth/token", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

                if (authResponse.Success && !string.IsNullOrEmpty(authResponse.AccessToken))
                {
                    lock (_lockObject)
                    {
                        _accessToken = authResponse.AccessToken;
                        _tokenExpiry = DateTime.UtcNow.AddSeconds(authResponse.ExpiresIn - 300); // 5 minutes de marge
                    }

                    Logger.Info("Token d'accès obtenu avec succès");
                    return _accessToken;
                }
                else
                {
                    Logger.Error($"Échec de l'authentification: {authResponse.Message}");
                    throw new AuthenticationException($"Échec de l'authentification: {authResponse.Message}");
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"Erreur HTTP lors de l'authentification: {ex.Message}");
                throw new AuthenticationException($"Erreur de connexion à l'API PPG Live: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Logger.Error($"Erreur inattendue lors de l'authentification: {ex.Message}");
                throw new AuthenticationException($"Erreur inattendue lors de l'authentification: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Valide les credentials de l'API
        /// </summary>
        /// <returns>True si les credentials sont valides</returns>
        public async Task<bool> ValidateCredentialsAsync()
        {
            try
            {
                var token = await GetAccessTokenAsync();
                return !string.IsNullOrEmpty(token);
            }
            catch (AuthenticationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Invalide le token actuel (force une nouvelle authentification)
        /// </summary>
        public void InvalidateToken()
        {
            lock (_lockObject)
            {
                _accessToken = null;
                _tokenExpiry = DateTime.MinValue;
            }
            Logger.Info("Token d'accès invalidé");
        }

        /// <summary>
        /// Ajoute les headers d'authentification à une requête HTTP
        /// </summary>
        /// <param name="request">Requête HTTP</param>
        public async Task AddAuthenticationHeadersAsync(HttpRequestMessage request)
        {
            var token = await GetAccessTokenAsync();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("X-API-Key", AppConfig.PPGLiveApiKey);
        }

        /// <summary>
        /// Dispose des ressources
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    /// <summary>
    /// Réponse d'authentification
    /// </summary>
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Exception d'authentification
    /// </summary>
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
