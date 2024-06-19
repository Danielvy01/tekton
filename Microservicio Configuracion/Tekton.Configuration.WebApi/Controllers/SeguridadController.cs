namespace Tekton.Configuration.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        /// <summary>
        /// Método consulta mensaje config
        /// </summary>     
        /// <param name="QueryMensajeConfiguracionById"></param>
        /// <remarks>
        /// El método consulta mensaje config
        /// </remarks>
        /// <example>
        /// <code>               
        /// </code>
        /// </example>
        /// <returns>Retorna Response<IActionResult>.</returns>
        [HttpPost("authorize")]
        [ProducesResponseType(typeof(QueryMensajeConfiguracion), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> authorize()
        {
            try
            {
                string authorizationRequest = "{0}?response_type=code&scope=openid%20profile&" +
                    "redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}";
                string clientId = "OHqMhgSLyMAPrMDi0fsHNOhQWeIa";
                string state = RandomDataBase64url(32);
                string codeVerifier = RandomDataBase64url(32);
                string codeChallenge = Base64urlencodeNoPadding(Sha256(codeVerifier));
                string codeChallengeMethod = "S256";
                string authorizationEndpoint = "https://localhost:9443/oauth2/authorize";
                var redirectUri = "http://localhost:4200/callback/";
                if (!redirectUri.EndsWith("/"))
                {
                    redirectUri += "/";
                }
                // Creates HttpListener to listen for requests on above redirect URI.
                // Create the OAuth2 authorization request.
                string urlAuthorizationRequest = string.Format(authorizationRequest,
                    authorizationEndpoint,
                    Uri.EscapeDataString(redirectUri),
                    clientId,
                    state,
                    codeChallenge,
                    codeChallengeMethod);


                //await _mediator.Send(QueryMensajeConfiguracionById)

                return Ok(new
                {
                    url = urlAuthorizationRequest,
                    codeVerifier = codeVerifier
                });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "error");
                throw;
            }
            finally
            {
                if (Application.Wrappers.Constantes.HABILITAR_LOG && Request != null) Serilog.Log.Information(Request.Path + "\t");
            }
        }
        [HttpPost("token")]
        [ProducesResponseType(typeof(QueryMensajeConfiguracion), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> token(string code, string codeVerifier)
        {
            try
            {
                string tokenRequestBody = "code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}" +
                    "&client_secret={4}&scope=&grant_type=authorization_code";
                string clientID = "OHqMhgSLyMAPrMDi0fsHNOhQWeIa";
                string clientSecret = "4kPMNo_3MxAwBhsIerOQO6Sb9QYa";
                string redirectURL = "http://localhost:4200/callback/";
                string urltokenRequestBody = string.Format(tokenRequestBody,
                    code,
                    Uri.EscapeDataString(redirectURL),
                    clientID,
                    codeVerifier,
                    clientSecret
                    );

                string urlToken = "https://localhost:9443/oauth2/token?" + urltokenRequestBody;

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                var baseAddress = new Uri(urlToken);
                string responseData = "";
                using (var httpClient = new HttpClient(clientHandler) { BaseAddress = baseAddress })
                {
                    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    StringContent requestContent = null;
                    requestContent = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (var response = await httpClient.PostAsync(baseAddress, requestContent))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }


                return Ok(new
                {
                    url = responseData
                });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "error");
                throw;
            }
            finally
            {
                if (Application.Wrappers.Constantes.HABILITAR_LOG && Request != null) Serilog.Log.Information(Request.Path + "\t" + Newtonsoft.Json.JsonConvert.SerializeObject(code));
            }
        }

        [HttpPost("userinfo")]
        [ProducesResponseType(typeof(QueryMensajeConfiguracion), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> userinfo(string token)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                string urlUserInfo = "https://localhost:9443/oauth2/userinfo";

                var baseAddress = new Uri(urlUserInfo);
                string responseData = "";
                using (var httpClient = new HttpClient(clientHandler) { BaseAddress = baseAddress })
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    httpClient.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                    using (var response = await httpClient.GetAsync(""))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }

                return Ok(new
                {
                    data = responseData
                });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "error");
                throw;
            }
            finally
            {
                if (Application.Wrappers.Constantes.HABILITAR_LOG && Request != null) Serilog.Log.Information(Request.Path + "\t" + Newtonsoft.Json.JsonConvert.SerializeObject(token));
            }
        }

        public static string RandomDataBase64url(uint length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return Base64urlencodeNoPadding(bytes);
        }
        public static byte[] Sha256(string inputStirng)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputStirng);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }

        /// <summary>
        /// Base64url no-padding that encodes the given input buffer.
        /// </summary>
        public static string Base64urlencodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);
            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");
            return base64;
        }
    }
}
