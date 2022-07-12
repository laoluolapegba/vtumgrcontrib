//using AutoMapper.Configuration;
using Marvin.StreamExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.NineMobile;

namespace Chams.Vtumanager.Provisioning.Services.NineMobileEvc


{   public   class LightEvcService : ILightEvcService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LightEvcService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        EvcSettings _settings;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="clientFactory"></param>
        public LightEvcService(
            IConfiguration config,
            ILogger<LightEvcService> logger,
            IHttpClientFactory clientFactory)
        {
            _config = config;
            _logger = logger;
            _clientFactory = clientFactory;
            _settings = new EvcSettings();
            _settings = _config.GetSection("EvcSettings").Get<EvcSettings>();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinlessRechargeRequest"></param>
        /// <returns></returns>
        public async Task<RechargeResponseEnvelope.Envelope> PinlessRecharge(PinlessRechargeRequest pinlessRechargeRequest)
        {
 
            _logger.LogInformation($"calling PinlessRecharge svc for transId : {pinlessRechargeRequest.transId}");

            RechargeResponseEnvelope.Envelope resultEnvelope = new RechargeResponseEnvelope.Envelope();
            try
            {
                string rechargeType = pinlessRechargeRequest.rechargeType == 2 ? "991" : "001";
                var sb = new System.Text.StringBuilder(1112);
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:com=""http://sdf.cellc.net/commonDataModel"">");
                sb.AppendLine(@"<soapenv:Header/>");
                sb.AppendLine(@"<soapenv:Body>");
                sb.AppendLine(@"<com:SDF_Data>");
                sb.AppendLine(@"<com:header>");
                sb.AppendLine(@"<com:processTypeID>" + _settings.pinlessRecharge.processTypeID + "</com:processTypeID>");
                sb.AppendLine(@"<com:externalReference>" + pinlessRechargeRequest.transId + "</com:externalReference>");
                sb.AppendLine(@"<com:sourceID>" + _settings.pinlessRecharge.sourceID + "</com:sourceID>");
                sb.AppendLine(@"<com:username>" + _settings.pinlessRecharge.Username + "</com:username>");
                sb.AppendLine(@"<com:password>" + _settings.pinlessRecharge.Password + "</com:password>");
                sb.AppendLine(@"<com:processFlag>1</com:processFlag>");
                sb.AppendLine(@"</com:header>");
                sb.AppendLine(@"<com:parameters name="""">");
                sb.AppendLine(@"<com:parameter name=""RechargeType"">" + rechargeType + "</com:parameter>");
                sb.AppendLine(@"<com:parameter name=""MSISDN"">" + pinlessRechargeRequest.Msisdn + "</com:parameter>");
                sb.AppendLine(@"<com:parameter name=""Amount"">" + pinlessRechargeRequest.Amount + "</com:parameter>");
                sb.AppendLine(@"<com:parameter name=""Channel_ID"">" + _settings.pinlessRecharge.Channel_ID + "</com:parameter>");
                sb.AppendLine(@"</com:parameters>");
                sb.AppendLine(@"<com:result>");
                sb.AppendLine(@"<com:statusCode/>");
                sb.AppendLine(@"<com:errorCode/>");
                sb.AppendLine(@"<com:errorDescription/>");
                sb.AppendLine(@"</com:result>");
                sb.AppendLine(@"</com:SDF_Data>");
                sb.AppendLine(@"</soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                _logger.LogInformation($"PinlessRecharge soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("PinlessRechargeClient");
                
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.pinlessRecharge.Url) 
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling PinlessRecharge URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Headers.Add("SOAPAction", _settings.pinlessRecharge.SoapAction);
                request.Headers.Add("key", _settings.pinlessRecharge.Key);
                request.Headers.Add("token", _settings.pinlessRecharge.Token);

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"PinlessRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"PinlessRechargesoap response = {contentStream}");


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
                _logger.LogError($"PinlessRecharge svc failed for transId : {pinlessRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryBalanceRequest"></param>
        /// <returns></returns>
        public async Task<QueryBalanceResponseEnvelope.Envelope> QueryEvcBalance(QueryBalanceRequest queryBalanceRequest)
        {
            string _url = _config["EvcSettings:QueryBalance:Url"];
            _logger.LogInformation($"calling QueryBalanceRequest svc for transId : {queryBalanceRequest.transId}");

            QueryBalanceResponseEnvelope.Envelope resultEnvelope = new QueryBalanceResponseEnvelope.Envelope();
            try
            {

                var sb = new System.Text.StringBuilder(1244);
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:bus=""http://www.huawei.com/bme/evcinterface/evc/businessmgrmsg"" xmlns:com=""http://www.huawei.com/bme/evcinterface/common"" xmlns:bus1=""http://www.huawei.com/bme/evcinterface/evc/businessmgr"">");
                sb.AppendLine(@"<soapenv:Header/>");
                sb.AppendLine(@"<soapenv:Body>");
                sb.AppendLine(@"<bus:QueryEVCBalanceRequestMsg>");
                sb.AppendLine(@"<RequestHeader>");
                sb.AppendLine(@"<com:CommandId>Dealer.QueryInventoryInfo</com:CommandId>");
                sb.AppendLine(@"<com:Version>1</com:Version>");
                sb.AppendLine(@"<com:TransactionId>1</com:TransactionId>");
                sb.AppendLine(@"<com:SequenceId>1</com:SequenceId>");
                sb.AppendLine(@"<com:RequestType>Event</com:RequestType>");
                sb.AppendLine(@"<!--Optional:-->");
                sb.AppendLine(@"<com:SessionEntity>");
                sb.AppendLine(@"<com:Name>" + _settings.queryBalance.Username + "</com:Name>");
                sb.AppendLine(@"<com:Password>" + _settings.queryBalance.Password + "</com:Password>");
                sb.AppendLine(@"<com:RemoteAddress>1</com:RemoteAddress>");
                sb.AppendLine(@"</com:SessionEntity>");
                sb.AppendLine(@"<com:SerialNo>" + queryBalanceRequest.transId + "</com:SerialNo>");
                sb.AppendLine(@"");
                sb.AppendLine(@"</RequestHeader>");
                sb.AppendLine(@"<QueryEVCBalanceRequest>");
                sb.AppendLine(@"<bus1:UserType>0</bus1:UserType>");
                sb.AppendLine(@"<bus1:UserAccount>" + queryBalanceRequest.EvcAccountCode + "</bus1:UserAccount>");
                sb.AppendLine(@"</QueryEVCBalanceRequest>");
                sb.AppendLine(@"</bus:QueryEVCBalanceRequestMsg>");
                sb.AppendLine(@"</soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                _logger.LogInformation($"QueryBalanceRequest soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("EVCQueryBalanceClient");


                var request = new HttpRequestMessage(HttpMethod.Post, _url) //_settings.queryBalance.Url
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling QueryBalanceRequest URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Headers.Add("SOAPAction", _settings.queryBalance.SoapAction);

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"QueryBalanceRequest api call returned with status code {response.StatusCode} {validationErrors}");

                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            // show this to the user
                            _logger.LogWarning($"QueryBalanceRequest api call returned with status code {response.StatusCode} {validationErrors}");

                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // trigger a login flow
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"QueryBalanceRequest call returned with status code {response.StatusCode} {validationErrors}");
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"QueryBalanceRequest response = {contentStream}");  //= {contentStream}
                                                                                                //deserialize the response
                                                                                                //MobileNIMC.Services.Services.Models.CreateTokenEnvelope.Envelope resultEnvelope = null;

                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(QueryBalanceResponseEnvelope.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as QueryBalanceResponseEnvelope.Envelope;
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"QueryBalanceRequest failed with {ex}");
            }

            return resultEnvelope;
        }

        public async Task<QueryBalanceResponseEnvelope.Envelope> QueryTransactionStatus(QueryBalanceRequest queryBalanceRequest)
        {
            string _url = _config["EvcSettings:QueryBalance:Url"];
            _logger.LogInformation($"calling QueryBalanceRequest svc for transId : {queryBalanceRequest.transId}");

            QueryBalanceResponseEnvelope.Envelope resultEnvelope = new QueryBalanceResponseEnvelope.Envelope();
            try
            {

                var sb = new System.Text.StringBuilder(1244);
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:bus=""http://www.huawei.com/bme/evcinterface/evc/businessmgrmsg"" xmlns:com=""http://www.huawei.com/bme/evcinterface/common"" xmlns:bus1=""http://www.huawei.com/bme/evcinterface/evc/businessmgr"">");
                sb.AppendLine(@"<soapenv:Header/>");
                sb.AppendLine(@"<soapenv:Body>");
                sb.AppendLine(@"<bus:QueryEVCBalanceRequestMsg>");
                sb.AppendLine(@"<RequestHeader>");
                sb.AppendLine(@"<com:CommandId>Dealer.QueryInventoryInfo</com:CommandId>");
                sb.AppendLine(@"<com:Version>1</com:Version>");
                sb.AppendLine(@"<com:TransactionId>1</com:TransactionId>");
                sb.AppendLine(@"<com:SequenceId>1</com:SequenceId>");
                sb.AppendLine(@"<com:RequestType>Event</com:RequestType>");
                sb.AppendLine(@"<!--Optional:-->");
                sb.AppendLine(@"<com:SessionEntity>");
                sb.AppendLine(@"<com:Name>" + _settings.queryBalance.Username + "</com:Name>");
                sb.AppendLine(@"<com:Password>" + _settings.queryBalance.Password + "</com:Password>");
                sb.AppendLine(@"<com:RemoteAddress>1</com:RemoteAddress>");
                sb.AppendLine(@"</com:SessionEntity>");
                sb.AppendLine(@"<com:SerialNo>" + queryBalanceRequest.transId + "</com:SerialNo>");
                sb.AppendLine(@"");
                sb.AppendLine(@"</RequestHeader>");
                sb.AppendLine(@"<QueryEVCBalanceRequest>");
                sb.AppendLine(@"<bus1:UserType>0</bus1:UserType>");
                sb.AppendLine(@"<bus1:UserAccount>" + queryBalanceRequest.EvcAccountCode + "</bus1:UserAccount>");
                sb.AppendLine(@"</QueryEVCBalanceRequest>");
                sb.AppendLine(@"</bus:QueryEVCBalanceRequestMsg>");
                sb.AppendLine(@"</soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                _logger.LogInformation($"QueryBalanceRequest soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("EVCQueryBalanceClient");


                var request = new HttpRequestMessage(HttpMethod.Post, _url) //_settings.queryBalance.Url
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling QueryBalanceRequest URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Headers.Add("SOAPAction", _settings.queryBalance.SoapAction);

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"QueryBalanceRequest api call returned with status code {response.StatusCode} {validationErrors}");

                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            // show this to the user
                            _logger.LogWarning($"QueryBalanceRequest api call returned with status code {response.StatusCode} {validationErrors}");

                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // trigger a login flow
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"QueryBalanceRequest call returned with status code {response.StatusCode} {validationErrors}");
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"QueryBalanceRequest response = {contentStream}");  //= {contentStream}
                                                                                                //deserialize the response
                                                                                                //MobileNIMC.Services.Services.Models.CreateTokenEnvelope.Envelope resultEnvelope = null;

                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(QueryBalanceResponseEnvelope.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as QueryBalanceResponseEnvelope.Envelope;
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"QueryBalanceRequest failed with {ex}");
            }

            return resultEnvelope;
        }
    }
}
