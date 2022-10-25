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
using Chams.Vtumanager.Provisioning.Entities.Common;

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
                sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.Append(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://hostif.vtm.prism.co.za/xsd"">");
                sb.Append(@"   <soapenv:Header />");
                sb.Append(@"   <soapenv:Body>");
                sb.Append(@"      <xsd:vend>");
                sb.Append(@"         <xsd:origMsisdn>" + _settings.PartnerMsisdn + "</xsd:origMsisdn>");
                sb.Append(@"         <xsd:destMsisdn>" + pinlessRechargeRequest.Msisdn + "</xsd:destMsisdn>");
                sb.Append(@"         <xsd:amount>" + pinlessRechargeRequest.Amount + "</xsd:amount>");
                sb.Append(@"         <xsd:sequence>" + pinlessRechargeRequest.transId + "</xsd:sequence>");
                sb.Append(@"         <xsd:tariffTypeId>1</xsd:tariffTypeId>");
                sb.Append(@"         <xsd:serviceproviderId>1</xsd:serviceproviderId>");
                sb.Append(@"      </xsd:vend>");
                sb.Append(@"   </soapenv:Body>");
                sb.Append(@"</soapenv:Envelope>");


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
                sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.Append(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://hostif.vtm.prism.co.za/xsd"">");
                sb.Append(@"   <soapenv:Header />");
                sb.Append(@"   <soapenv:Body>");
                sb.Append(@"      <xsd:vend>");
                sb.Append(@"         <xsd:origMsisdn>" + _settings.PartnerMsisdn + "</xsd:origMsisdn>");
                sb.Append(@"         <xsd:destMsisdn>" + pinlessRechargeRequest.Msisdn + "</xsd:destMsisdn>");
                sb.Append(@"         <xsd:amount>" + pinlessRechargeRequest.Amount + "</xsd:amount>");
                sb.Append(@"         <xsd:sequence>" + pinlessRechargeRequest.transId + "</xsd:sequence>");
                sb.Append(@"         <xsd:tariffTypeId>" + pinlessRechargeRequest.ProductCode + "</xsd:tariffTypeId>");
                sb.Append(@"         <xsd:serviceproviderId>1</xsd:serviceproviderId>");
                sb.Append(@"      </xsd:vend>");
                sb.Append(@"   </soapenv:Body>");
                sb.Append(@"</soapenv:Envelope>");



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

        public async Task<MtnSubscriptionResponse> MtnSubscription(
         PinlessRechargeRequest pinlessRechargeRequest)
        {
            _logger.LogInformation($"Inside MTN Subscription service request");

            string gatewayURL = _config["MtnTopupSettings:V3:Url"];
            string walletId = _config["MtnTopupSettings:V3:WalletId"];
            string apiKey = _config["MtnTopupSettings:V3:API_KEY"];
            string coundtryCode = _config["MtnTopupSettings:V3:CountryCode"];
            string credentials = _config["MtnTopupSettings:V3:Credentials"];
            string subscriptionProviderId = _config["MtnTopupSettings:V3:subscriptionProviderId"];

            MtnSubscriptionRequest subscriptionRequest = new MtnSubscriptionRequest
            {
                amountCharged = pinlessRechargeRequest.Amount.ToString(),
                beneficiaryId = pinlessRechargeRequest.Msisdn,
                correlationId = pinlessRechargeRequest.transId,
                subscriptionId = pinlessRechargeRequest.ProductCode,
                subscriptionProviderId = subscriptionProviderId
            };

            
            
            MtnSubscriptionResponse result = new MtnSubscriptionResponse();
            
            var httpClient = _clientFactory.CreateClient("MtnTopupClient");

            httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            httpClient.DefaultRequestHeaders.Add("x-country-code", coundtryCode);
            httpClient.DefaultRequestHeaders.Add("transactionId", subscriptionRequest.correlationId);
            httpClient.DefaultRequestHeaders.Add("Credentials", credentials);
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            string requestUri = gatewayURL + walletId + "/subscriptions";
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            _logger.LogInformation($"Calling MTN Subscription  {httpRequest.RequestUri}");

            using (var httpContent = CreateHttpContent(subscriptionRequest) )
            {
                httpRequest.Content = httpContent;

                using (var response = await httpClient
                    .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    //response.EnsureSuccessStatusCode();
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"api call MTN Subscription returned with statusCode {response.StatusCode} reason: {response.ReasonPhrase}");
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"api call MTN Subscription returned with status code: {response.StatusCode} validationErrors: -- {validationErrors} --");

                        result = JsonConvert.DeserializeObject<MtnSubscriptionResponse>(validationErrors.ToString());

                    }
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStream = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation($"api call MTN Subscription returned with contentstream  {contentStream}");
                        result = JsonConvert.DeserializeObject<MtnSubscriptionResponse>(contentStream);
                    }

                }
            }

            return result;
        }

        public async Task<QueryTxnStatusResponse> QueryTransactionStatus(
         QueryTransactionStatusRequest statusRequest)
        {
            _logger.LogInformation($"Inside MTN MtnQueryStatus service request");

            string gatewayURL = _config["MtnTopupSettings:V3:Url"];
            string walletId = _config["MtnTopupSettings:V3:WalletId"];
            string apiKey = _config["MtnTopupSettings:V3:API_KEY"];
            string coundtryCode = _config["MtnTopupSettings:V3:CountryCode"];
            string credentials = _config["MtnTopupSettings:V3:Credentials"];
            string subscriptionProviderId = _config["MtnTopupSettings:V3:subscriptionProviderId"];

            //https://preprod-nigeria.api.mtn.com/v2/customers/subscriptions/2348031011125/status/2021102115573975401084328?customerId=2348031011125&queryType=TRANSACTION_REFERENCE&subscriptionProviderId=ERS

            
            MtnSubscriptionResponse envelope = null;
            QueryTxnStatusResponse rsp = new QueryTxnStatusResponse();

            var httpClient = _clientFactory.CreateClient("MtnTopupClient");

            httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            httpClient.DefaultRequestHeaders.Add("x-country-code", coundtryCode);
            httpClient.DefaultRequestHeaders.Add("transactionId", statusRequest.transactionId);
            httpClient.DefaultRequestHeaders.Add("Credentials", credentials);
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            string requestUri = gatewayURL + "/subscriptions/" + walletId + "/status/" + statusRequest.TransactionReference 
                + "?customerId=" + walletId + "&queryType=TRANSACTION_REFERENCE&subscriptionProviderId=ERS";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            _logger.LogInformation($"Calling MTN MtnQueryStatus  {httpRequest.RequestUri}");

            using (var response = await httpClient
                    .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
            {
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"api call MTN MtnQueryStatus returned with statusCode {response.StatusCode} reason: {response.ReasonPhrase}");
                    var errorStream = await response.Content.ReadAsStreamAsync();
                    var validationErrors = errorStream.ReadAndDeserializeFromJson();
                    _logger.LogWarning($"api call MTN MtnQueryStatus returned with status code: {response.StatusCode} validationErrors: -- {validationErrors} --");

                    envelope = JsonConvert.DeserializeObject<MtnSubscriptionResponse>(validationErrors.ToString());

                }
                if (response.IsSuccessStatusCode)
                {
                    string contentStream = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"api call MTN MtnQueryStatus returned with contentstream  {contentStream}");
                    envelope = JsonConvert.DeserializeObject<MtnSubscriptionResponse>(contentStream);
                }

            }
            if(envelope!=null)
            {
                rsp.exchangeReference = envelope.subscriptionDescription;
                rsp.responseMessage = envelope.statusMessage;
                rsp.statusId = envelope.statusCode;
                rsp.transactionReference = envelope.transactionId;
            }
            

            return rsp;
        }


        public async Task<MtnQTxResponseEnvelope.Envelope> QueryTransactionStatusbyClientRef(QueryTransactionStatusRequest queryTransaction)
        {
            _logger.LogInformation($"calling MTN QueryTransactionStatusV1 svc for transId : {queryTransaction.TransactionReference}");
            string soapAction = "urn:QyeryTx";
            _clientId = _config["MtnTopupSettings:V1:Username"];
            _clientSecret = _config["MtnTopupSettings:V1:Password"];
            MtnQTxResponseEnvelope.Envelope resultEnvelope = null;
            try
            {

                var sb = new System.Text.StringBuilder(331);
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://hostif.vtm.prism.co.za/xsd"">");
                sb.AppendLine(@"   <soapenv:Header />");
                sb.AppendLine(@"   <soapenv:Body>");
                sb.AppendLine(@"      <xsd:querytx>");
                sb.AppendLine(@"         <xsd:sequence>" + queryTransaction.TransactionReference + "</xsd:sequence>");
                sb.AppendLine(@"      </xsd:querytx>");
                sb.AppendLine(@"   </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");


                _logger.LogInformation($"MTN QueryTransactionStatusV1 soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("MtnTopupClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.V1.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling MTN QueryTransactionStatusV1 URL  {request.RequestUri}");
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
                        _logger.LogWarning($"Mtn QueryTransactionStatusV1 api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"MTN QueryTransactionStatusV1 response XML = {contentStream}");


                    using (var stringReader = new StringReader(contentStream))
                    {
                        
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(MtnResponseEnvelope.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as MtnQTxResponseEnvelope.Envelope;
                        }
                    }
                    _logger.LogInformation($"MTN QueryTransactionStatusV1 responseObject = {JsonConvert.SerializeObject(resultEnvelope)}");

                }
            }


            catch (Exception ex)
            {
                _logger.LogError($"MTN QueryTransactionStatusV1 svc failed for transId : {queryTransaction.TransactionReference} with error {ex}");
            }

            return resultEnvelope;
        }

        public async Task<MtnQTxResponseEnvelope.Envelope> QueryTransactionStatusbyERSRef(QueryTransactionStatusRequest queryTransaction)
        {
            _logger.LogInformation($"calling MTN QueryTransactionStatusV1 svc for transId : {queryTransaction.TransactionReference}");
            string soapAction = "urn:QyeryTx";
            _clientId = _config["MtnTopupSettings:V1:Username"];
            _clientSecret = _config["MtnTopupSettings:V1:Password"];
            MtnQTxResponseEnvelope.Envelope resultEnvelope = null;
            try
            {
                var sb = new System.Text.StringBuilder(347);
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://hostif.vtm.prism.co.za/xsd"">");
                sb.AppendLine(@"   <soapenv:Header />");
                sb.AppendLine(@"   <soapenv:Body>");
                sb.AppendLine(@"      <xsd:querytx>");
                sb.AppendLine(@"         <xsd:txRef>"+ queryTransaction.TransactionReference +"</xsd:txRef>");
                sb.AppendLine(@"      </xsd:querytx>");
                sb.AppendLine(@"   </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                


                _logger.LogInformation($"MTN QueryTransactionStatusV1 soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("MtnTopupClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.V1.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling MTN QueryTransactionStatusV1 URL  {request.RequestUri}");
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
                        _logger.LogWarning($"Mtn QueryTransactionStatusV1 api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"MTN QueryTransactionStatusV1 response XML = {contentStream}");


                    using (var stringReader = new StringReader(contentStream))
                    {

                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(MtnResponseEnvelope.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as MtnQTxResponseEnvelope.Envelope;
                        }
                    }
                    _logger.LogInformation($"MTN QueryTransactionStatusV1 responseObject = {JsonConvert.SerializeObject(resultEnvelope)}");

                }
            }


            catch (Exception ex)
            {
                _logger.LogError($"MTN QueryTransactionStatusV1 svc failed for transId : {queryTransaction.TransactionReference} with error {ex}");
            }

            return resultEnvelope;
        }

    }
}
