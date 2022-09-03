using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Marvin.StreamExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Pretups;
using Microsoft.Extensions.Primitives;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AirtelPretupsService : IAirtelPretupsService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AirtelPretupsService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly PretupsSettings _settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="clientFactory"></param>
        public AirtelPretupsService(
            IConfiguration config,
            ILogger<AirtelPretupsService> logger,
            IHttpClientFactory clientFactory)
        {
            _config = config;
            _logger = logger;
            _clientFactory = clientFactory;
            _settings = new PretupsSettings();
            _settings = _config.GetSection("PretupsSettings").Get<PretupsSettings>();
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
        /// Airtel Pretups Airtime Request
        /// </summary>
        /// <param name="pinRechargeRequest"></param>
        /// <returns></returns>
        public async Task<PretupsRechargeResponseEnvelope.COMMAND> AirtimeRecharge(PinlessRechargeRequest pinRechargeRequest)
        {

            _logger.LogInformation($"calling AirtimeRecharge svc for transId : {pinRechargeRequest.transId}");

            PretupsRechargeResponseEnvelope.COMMAND resultEnvelope = new PretupsRechargeResponseEnvelope.COMMAND();
            try
            {
                string rechargeType = pinRechargeRequest.rechargeType == 2 ? _settings.transactionType.DataPurchase : _settings.transactionType.AirtimePurchase;

                string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
               
                //sb.AppendLine(@"<?xml version=""1.0""?><COMMAND><TYPE>EXRCTRFREQ</TYPE><DATE>19/07/2022 13:17:10</DATE><EXTNWCODE>NG</EXTNWCODE><MSISDN>8087397977</MSISDN><PIN>4477</PIN><LOGINID></LOGINID><PASSWORD></PASSWORD><EXTCODE></EXTCODE><EXTREFNUM>2022071631355013</EXTREFNUM><MSISDN2>8022226516</MSISDN2><AMOUNT>50</AMOUNT><LANGUAGE1>1</LANGUAGE1><LANGUAGE2>1</LANGUAGE2><SELECTOR>1</SELECTOR></COMMAND>");
                
                var sb = new System.Text.StringBuilder(444);
                sb.Append(@"<?xml version=""1.0""?>");
                sb.Append(@"<COMMAND>");
                sb.Append(@"<TYPE>EXRCTRFREQ</TYPE>");
                sb.Append(@"<DATE>" + tranDate + "</DATE>");
                sb.Append(@"<EXTNWCODE>NG</EXTNWCODE>");
                sb.Append(@"<MSISDN>" + _settings.PartnerMsisdn + "</MSISDN>");
                sb.Append(@"<PIN>" + _settings.PIN + "</PIN>");
                sb.Append(@"<LOGINID></LOGINID>");
                sb.Append(@"<PASSWORD></PASSWORD>");
                sb.Append(@"<EXTCODE></EXTCODE>");
                sb.Append(@"<EXTREFNUM>"+ pinRechargeRequest.transId + "</EXTREFNUM>");
                sb.Append(@"<MSISDN2>" + pinRechargeRequest.Msisdn + "</MSISDN2>");
                sb.Append(@"<AMOUNT>" + pinRechargeRequest.Amount + "</AMOUNT>");
                sb.Append(@"<LANGUAGE1>1</LANGUAGE1>");
                sb.Append(@"<LANGUAGE2>1</LANGUAGE2>");
                sb.Append(@"<SELECTOR>1</SELECTOR>");
                sb.Append(@"</COMMAND>");

                //var sb = new System.Text.StringBuilder(440);
                //sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                //sb.AppendLine(@"<COMMAND>");
                //sb.AppendLine(@"   <TYPE>" + rechargeType + "</TYPE>");
                //sb.AppendLine(@"   <DATE>" + tranDate + "</DATE>");
                //sb.AppendLine(@"   <EXTNWCODE>NG</EXTNWCODE>");
                //sb.AppendLine(@"   <MSISDN>" + _settings.PartnerMsisdn + "</MSISDN>");
                //sb.AppendLine(@"   <PIN>" + _settings.PIN + "</PIN>");
                //sb.AppendLine(@"   <LOGINID />");
                //sb.AppendLine(@"   <PASSWORD />");
                //sb.AppendLine(@"   <EXTCODE />");
                //sb.AppendLine(@"   <EXTREFNUM>" + _settings.PartnerCode + "</EXTREFNUM>");
                //sb.AppendLine(@"   <MSISDN2>" + pinRechargeRequest.Msisdn + "</MSISDN2>");
                //sb.AppendLine(@"   <AMOUNT>" + pinRechargeRequest.Amount + "</AMOUNT>");
                //sb.AppendLine(@"   <LANGUAGE1>1</LANGUAGE1>");
                //sb.AppendLine(@"   <LANGUAGE2>1</LANGUAGE2>");
                //sb.AppendLine(@"   <SELECTOR>1</SELECTOR>");
                //sb.AppendLine(@"</COMMAND>");

                _logger.LogInformation($"AirtimeRecharge soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("PretupsRechargeClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling AirtimeRecharge URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"AirtimeRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"AirtimeRecharge response = {contentStream}");


                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(PretupsRechargeResponseEnvelope.COMMAND));
                            resultEnvelope = serializer.Deserialize(reader) as PretupsRechargeResponseEnvelope.COMMAND;
                        }
                    }

                }
            }

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"AirtimeRecharge svc failed for transId : {pinRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }

        /// <summary>
        /// Airtel Pretups Databundle Request
        /// </summary>
        /// <param name="pinRechargeRequest"></param>
        /// <returns></returns>
        public async Task<PretupsRechargeResponseEnvelope.COMMAND> DataRecharge(PinlessRechargeRequest pinRechargeRequest)
        {

            _logger.LogInformation($"calling Airtel DataRecharge svc for transId : {pinRechargeRequest.transId}");

            PretupsRechargeResponseEnvelope.COMMAND resultEnvelope = new PretupsRechargeResponseEnvelope.COMMAND();
            try
            {
                //string rechargeType = pinRechargeRequest.rechargeType == 2 ? _settings.transactionType.DataPurchase : _settings.transactionType.AirtimePurchase;

                string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");


                var sb = new System.Text.StringBuilder(468);
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.AppendLine(@"<!DOCTYPE COMMAND PUBLIC ""-//Ocam//DTD XML Command1.0//EN"" ""xml/command.dtd"">");
                sb.AppendLine(@"<COMMAND>");
                sb.AppendLine(@"<TYPE>VASSELLREQ</TYPE>");
                sb.AppendLine(@"<DATE>" + tranDate + "</DATE>");
                sb.AppendLine(@"<EXTNWCODE>NG</EXTNWCODE>");
                sb.AppendLine(@"<MSISDN>" + _settings.PartnerMsisdn + "</MSISDN>");
                sb.AppendLine(@"<PIN>" + _settings.PIN + "</PIN>");
                sb.AppendLine(@"<LOGINID/>");
                sb.AppendLine(@"<PASSWORD/>");
                sb.AppendLine(@"<EXTCODE/>");
                sb.AppendLine(@"<EXTREFNUM>" + _settings.PartnerCode + "</EXTREFNUM>");
                sb.AppendLine(@"<SUBSMSISDN>" + pinRechargeRequest.Msisdn + "</SUBSMSISDN>");
                sb.AppendLine(@"<AMT>" + pinRechargeRequest.Amount + "</AMT>");
                sb.AppendLine(@"<SUBSERVICE>" + pinRechargeRequest.ProductCode + "</SUBSERVICE>");
                sb.AppendLine(@"</COMMAND>");

                _logger.LogInformation($"Airtel DataRecharge soap request = {sb.ToString()}");


                var httpClient = _clientFactory.CreateClient("PretupsRechargeClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling Airtel DataRecharge URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"Airtel DataRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"Airtel DataRecharge response = {contentStream}");


                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(PretupsRechargeResponseEnvelope.COMMAND));
                            resultEnvelope = serializer.Deserialize(reader) as PretupsRechargeResponseEnvelope.COMMAND;
                        }
                    }

                }
            }

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"Airtel DataRecharge svc failed for transId : {pinRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }
        /// <summary>
        /// Request Transaction status
        /// </summary>
        /// <param name="queryTransactionStatusRequest"></param>
        /// <returns></returns>
        public async Task<RequestBalanceResp> QueryTransactionStatus(QueryTransactionStatusRequest queryTransactionStatusRequest)
        {

            string _url = _config["PretupsSettings:Url"];
            _logger.LogInformation($"calling QueryTransactionStatus svc for transId : {queryTransactionStatusRequest.TransactionReference}");

            RequestBalanceResp resultEnvelope = new RequestBalanceResp();
            try
            {

                var sb = new System.Text.StringBuilder(1244);

                _logger.LogInformation($"Airtel QueryTransactionStatus soap request = {sb.ToString()}");  //

                var httpClient = _clientFactory.CreateClient("PretupsRechargeClient");


                var request = new HttpRequestMessage(HttpMethod.Post, _url) //_settings.queryBalance.Url
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling QueryBalanceRequest URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));


                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"QueryTransactionStatus api call returned with status code {response.StatusCode} {validationErrors}");

                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            // show this to the user
                            _logger.LogWarning($"QueryTransactionStatus api call returned with status code {response.StatusCode} {validationErrors}");

                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // trigger a login flow
                            var errorStream = await response.Content.ReadAsStreamAsync();
                            var validationErrors = errorStream.ReadAndDeserializeFromJson();
                            _logger.LogWarning($"QueryTransactionStatus call returned with status code {response.StatusCode} {validationErrors}");
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"QueryTransactionStatus response = {contentStream}");
                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(RequestBalanceResp));
                            resultEnvelope = serializer.Deserialize(reader) as RequestBalanceResp;
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"QueryTransactionStatus for {queryTransactionStatusRequest.TransactionReference}failed with {ex}");
            }

            return resultEnvelope;
        }
    }
}
