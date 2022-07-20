using Chams.Vtumanager.Fulfillment.NineMobile.Services;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Pretups;
using Marvin.StreamExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.NineMobile;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn;
using System.Reflection.Metadata;

namespace Chams.Vtumanager.Provisioning.Services.Mtn
{
    public class MtnTopupService : IMtnTopupService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MtnTopupService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MtnSettings _settings;
        private string _clientId;
        private string _clientSecret;
        public MtnTopupService(
            IConfiguration config,
            ILogger<MtnTopupService> logger,
IHttpClientFactory clientFactory)
        {
            _config = config;
            _logger = logger;
            _clientFactory = clientFactory;
            _settings = new MtnSettings();
            _settings = _config.GetSection("MtnTopupSettings").Get<MtnSettings>();
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<AccessTokenResponse> GetAccessToken()
        {
            _logger.LogInformation($"Inside GetToken service request");

            _clientId = _config["MtnTopupSettings:V1:Username"];
            _clientSecret = _config["MtnTopupSettings:V1:Password"];
            string _url = _config["MtnTopupSettings:TokenUrl"];
            AccessTokenResponse tokenResponse = new AccessTokenResponse();

            try
            {
                //AccessScopes scope = AccessScopes.CR_PERSON_READ;
                AccessTokenRequest tokenRequest = new AccessTokenRequest
                {
                    grant_type = "client_credentials",
                    client_secret = _clientSecret,
                    client_id = _clientId,
                };


                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("grant_type", tokenRequest.grant_type);
                parameters.Add("client_id", tokenRequest.client_id);
                parameters.Add("client_secret", tokenRequest.client_secret);




                var httpClient = _clientFactory.CreateClient("MtnTopupClient");

                var request = new HttpRequestMessage(HttpMethod.Post, _url)
                {
                    Content = new FormUrlEncodedContent(parameters)
                };
                _logger.LogInformation($"Calling createToken MTN URL:  {httpClient.BaseAddress} {request.RequestUri}");


                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"MTN getaccesstoken api call returned with status code {response.StatusCode} {validationErrors}");
                    }

                    var contentStream = await response.Content.ReadAsStringAsync();
                    //using var streamReader = new StreamReader(contentStream);
                    tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contentStream);

                    // using var jsonReader = new JsonTextReader(streamReader);
                    //JsonSerializer  = new JsonSerializer();
                    //serializer.Deserialize<AccessTokenResponse>(jsonReader);

                    //token = await contentStream.Content.ReadAsAsync<AccessTokenResponse>(new[] { new JsonMediaTypeFormatter() });

                    _logger.LogInformation($"MTN getaccesstoken response :{contentStream}");



                }

            }
            catch (JsonReaderException ex)
            {
                _logger.LogError($"Invalid json in  acquire access token : {ex}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to acquire access token : {ex}");
            }
            finally
            {

            }

            return tokenResponse;

        }
        public async Task<MtnResponseEnvelope.Envelope> AirtimeRecharge(PinlessRechargeRequest pinlessRechargeRequest)
        {
            _logger.LogInformation($"calling MTN AirtimeRecharge svc for transId : {pinlessRechargeRequest.transId}");
            string soapAction = "urn:Vend";
            _clientId = _config["MtnTopupSettings:V1:Username"];
            _clientSecret = _config["MtnTopupSettings:V1:Password"];
            MtnResponseEnvelope.Envelope resultEnvelope = new MtnResponseEnvelope.Envelope();
            try
            {

                var sb = new System.Text.StringBuilder(584);
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://hostif.vtm.prism.co.za/xsd"">");
                sb.AppendLine(@"   <soapenv:Header />");
                sb.AppendLine(@"   <soapenv:Body>");
                sb.AppendLine(@"      <xsd:vend>");
                sb.AppendLine(@"         <xsd:origMsisdn>" + _settings.PartnerMsisdn + "</xsd:origMsisdn>");
                sb.AppendLine(@"         <xsd:destMsisdn>" + pinlessRechargeRequest.Msisdn + "</xsd:destMsisdn>");
                sb.AppendLine(@"         <xsd:amount>" + pinlessRechargeRequest.Amount + "</xsd:amount>");
                sb.AppendLine(@"         <xsd:sequence>" + pinlessRechargeRequest.transId + "</xsd:sequence>");
                sb.AppendLine(@"         <xsd:tariffTypeId>1</xsd:tariffTypeId>");
                sb.AppendLine(@"         <xsd:serviceproviderId>1</xsd:serviceproviderId>");
                sb.AppendLine(@"      </xsd:vend>");
                sb.AppendLine(@"   </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");


                _logger.LogInformation($"MTN AirtimeRecharge soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("MtnTopupClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.V1.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling MTN AirtimeRecharge URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Headers.Add("SOAPAction", soapAction);

                //Basic Authentication
                var authenticationString = $"{_clientId}:{_clientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

                _logger.LogInformation($"Base64 encoded credentials: {base64EncodedAuthenticationString}");

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"Mtn AirtimeRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"MTN AirtimeRecharge response XML = {contentStream}");


                    using (var stringReader = new StringReader(contentStream))
                    {
                        //XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
                        //Envelope envelope = (Envelope)serializer.Deserialize(reader);

                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(MtnResponseEnvelope.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as MtnResponseEnvelope.Envelope;
                        }
                    }
                    _logger.LogInformation($"MTN AirtimeRecharge responseObject = {JsonConvert.SerializeObject(resultEnvelope)}");

                }
            }

            
            catch (Exception ex)
            {
                _logger.LogError($"MTN AirtimeRecharge svc failed for transId : {pinlessRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }

        public async Task<RechargeResponseEnvelope.Envelope> DataRecharge(PinlessRechargeRequest pinlessRechargeRequest)
        {
            _logger.LogInformation($"calling MTN DataRecharge svc for transId : {pinlessRechargeRequest.transId}");
            string soapAction = "urn:Vend";
            RechargeResponseEnvelope.Envelope resultEnvelope = new RechargeResponseEnvelope.Envelope();
            try
            {

                var sb = new System.Text.StringBuilder(584);
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://hostif.vtm.prism.co.za/xsd"">");
                sb.AppendLine(@"   <soapenv:Header />");
                sb.AppendLine(@"   <soapenv:Body>");
                sb.AppendLine(@"      <xsd:vend>");
                sb.AppendLine(@"         <xsd:origMsisdn>" + _settings.PartnerMsisdn + "</xsd:origMsisdn>");
                sb.AppendLine(@"         <xsd:destMsisdn>" + pinlessRechargeRequest.Msisdn + "</xsd:destMsisdn>");
                sb.AppendLine(@"         <xsd:amount>" + pinlessRechargeRequest.Amount + "</xsd:amount>");
                sb.AppendLine(@"         <xsd:sequence>" + pinlessRechargeRequest.transId + "</xsd:sequence>");
                sb.AppendLine(@"         <xsd:tariffTypeId>" + pinlessRechargeRequest.ProductCode + "</xsd:tariffTypeId>");
                sb.AppendLine(@"         <xsd:serviceproviderId>1</xsd:serviceproviderId>");
                sb.AppendLine(@"      </xsd:vend>");
                sb.AppendLine(@"   </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");



                _logger.LogInformation($"MTN DataRecharge soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("MtnTopupClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.V1.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling MTN PinlessRecharge URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Headers.Add("SOAPAction", soapAction);

                //Basic Authentication
                var authenticationString = $"{_clientId}:{_clientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);




                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"MTN DataRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"MTN DataRecharge response = {contentStream}");


                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(RechargeResponseEnvelope.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as RechargeResponseEnvelope.Envelope;
                        }
                    }

                }
            }

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"MTN DataRecharge svc failed for transId : {pinlessRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }

        public async Task<MtnResponseEnvelope> PinlessRecharge1(
         PinlessRechargeRequest pinlessRechargeRequest, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Inside MTN PinlessRecharge service request");

            MtnResponseEnvelope result = new MtnResponseEnvelope();
            string gatewayURL = _config["MtnTopupSettings:Verion3:Url"];
            AccessTokenResponse accessToken = await GetAccessToken();

            //AccessTokenResponse accessToken = await GetAccessToken(new AccessTokenRequest
            //{
            //    scope = "pr.person.read"
            //});

            var keyValues = new List<KeyValuePair<string, string>>();

            keyValues.Add(new KeyValuePair<string, string>("access_token", accessToken.access_token));


            if (accessToken.access_token != string.Empty)
            {
                var httpClient = _clientFactory.CreateClient("MtnTopupClient");


                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);
                string requestParams = new FormUrlEncodedContent(keyValues).ReadAsStringAsync().Result;
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, gatewayURL + "?" + requestParams);
                _logger.LogInformation($"Calling MTN PinlessRecharge  {httpRequest.RequestUri}");

                using (var httpContent = new FormUrlEncodedContent(keyValues))
                {
                    httpRequest.Content = httpContent;

                    using (var response = await httpClient
                        .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                    {
                        //response.EnsureSuccessStatusCode();
                        if (!response.IsSuccessStatusCode)
                        {
                            _logger.LogInformation($"api call PinlessRecharge returned with statusCode {response.StatusCode} reason: {response.ReasonPhrase}");
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"api call PinlessRecharge returned with status code: {response.StatusCode} validationErrors: -- {validationErrors} --");

                            result = JsonConvert.DeserializeObject<MtnResponseEnvelope>(validationErrors.ToString());

                        }
                        if (response.IsSuccessStatusCode)
                        {
                            string contentStream = await response.Content.ReadAsStringAsync();
                            _logger.LogInformation($"api call PinlessRecharge returned with contentstream  {contentStream}");
                            result = JsonConvert.DeserializeObject<MtnResponseEnvelope>(contentStream);
                        }

                    }
                }
            }
            else
            {
                _logger.LogInformation($"failed to acquire token for mtn PinlessRecharge");
            }

            return result;
        }


    }
}
