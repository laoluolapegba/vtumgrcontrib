//using AutoMapper.Configuration;
using Marvin.StreamExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Sales_Mgmt.Services.Entities.EtopUp;
using SalesMgmt.Services.Entities.Common;
using SalesMgmt.Services.Entities.EtopUp;
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

namespace Sales_Mgmt.Services.Services.Evc
{   public   class LightEvcService : ILightEvcService
    {
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

        public async Task<ModifyEvcResponseEnvelope.Envelope> ModifyEvcInventory(ModifyEvcTransaction modifyEvcTransaction,
            IConfiguration config, 
            ILogger _logger,
            IHttpClientFactory _clientFactory)
        {
            //_url = _config["EVC:Url"];
            //_action = _config["EVC:SoapAction"];
            //_channelID = _config["EVC:SoapAction"];
            //_rechargeType = _config["EVC:SoapAction"];
            //_processTypeId = _config["EVC:SoapAction"];
            //_sourceID = _config["EVC:SoapAction"];
            //_prechargeUsername = _config["EVC:SoapAction"];
            //_prechargePassword = _config["EVC:SoapAction"];
            

            EvcSettings _settings = new EvcSettings();
            _settings = config.GetSection("EvcSettings").Get<EvcSettings>();
            _logger.LogInformation($"calling ModifyEvcInventory svc for transId : {modifyEvcTransaction.transId}");

            //string transId = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
            ModifyEvcResponseEnvelope.Envelope resultEnvelope = new ModifyEvcResponseEnvelope.Envelope();
            try
            {
                var sb = new System.Text.StringBuilder(1185);
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" ");
                sb.AppendLine(@"xmlns:bus=""http://www.huawei.com/evcinterface/websmapmgrmsg""");
                sb.AppendLine(@"xmlns:com=""http://www.huawei.com/evcinterface/common"" ");
                sb.AppendLine(@"xmlns:bus1=""http://www.huawei.com/evcinterface/websmapmgr"">");
                sb.AppendLine(@"<soapenv:Header/>");
                sb.AppendLine(@"<soapenv:Body>");
                sb.AppendLine(@"<bus:ModifyEVCInventoryRequestMsg>");
                sb.AppendLine(@"<RequestHeader>");
                sb.AppendLine(@"<com:CommandId>ModifyEVCInventory</com:CommandId>");
                sb.AppendLine(@"<com:SerialNo>" + modifyEvcTransaction.transId + "</com:SerialNo>");
                sb.AppendLine(@"<!--Optional:-->");
                sb.AppendLine(@"<com:SessionEntity>");
                sb.AppendLine(@"<com:Name>" + _settings.modifyInventory.Username + "</com:Name>");
                sb.AppendLine(@"<com:Password>" + _settings.modifyInventory.Password + "</com:Password>");
                sb.AppendLine(@"<com:RemoteAddress>1</com:RemoteAddress>");
                sb.AppendLine(@"</com:SessionEntity>");
                sb.AppendLine(@"</RequestHeader>");
                sb.AppendLine(@"<ModifyEVCInventoryRequest>");
                sb.AppendLine(@"<bus1:EntityType>0</bus1:EntityType>");
                sb.AppendLine(@"<bus1:EntityID>" + modifyEvcTransaction.msisdn + "</bus1:EntityID>");
                sb.AppendLine(@"<bus1:Amount>" + modifyEvcTransaction.transAmount + "</bus1:Amount>  ");
                sb.AppendLine(@"<bus1:Commission>1</bus1:Commission>         ");
                sb.AppendLine(@"<bus1:Reason>" + modifyEvcTransaction.transDesc + "</bus1:Reason>");
                sb.AppendLine(@"</ModifyEVCInventoryRequest>");
                sb.AppendLine(@"</bus:ModifyEVCInventoryRequestMsg>");
                sb.AppendLine(@"</soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                _logger.LogInformation($"etopUpTransferOrder soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("EVCModifyInventoryClient");


                var request = new HttpRequestMessage(HttpMethod.Post, _settings.modifyInventory.Url) //"searchByNIN"
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling etopUpTransferOrder URL  {request.RequestUri}");
                //request.Headers.Clear();
                string soapAction = "";
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Headers.Add("SOAPAction", "\"ModifyEVCInventory\""); //_settings.modifyInventory.SoapAction

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"etopUpTransferOrder api call returned with status code {response.StatusCode} {validationErrors}");

                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            //var errorStream = await response.Content.ReadAsStreamAsync();
                            //var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            // show this to the user
                            _logger.LogWarning($"EtopUpTransferOrder api call returned with status code {response.StatusCode} ");

                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // trigger a login flow
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"EtopUpTransferOrder call returned with status code {response.StatusCode} {validationErrors}");
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"EtopUpTransferOrdersoap response = {contentStream}");  //= {contentStream}
                                                                                                    //deserialize the response
                                                                                                    //MobileNIMC.Services.Services.Models.CreateTokenEnvelope.Envelope resultEnvelope = null;

                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(ModifyEvcResponseEnvelope.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as ModifyEvcResponseEnvelope.Envelope;
                        }
                    }



                }
            }

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"ModifyEvcTransactionsoap failed with {ex}");
                throw ex;
            }

            return resultEnvelope;
        }
    }
}
