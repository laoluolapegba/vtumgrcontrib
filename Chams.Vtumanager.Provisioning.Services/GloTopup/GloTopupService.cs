using Chams.Vtumanager.Fulfillment.NineMobile.Services;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Pretups;
using Marvin.StreamExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Glo;
using Newtonsoft.Json;
using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Microsoft.AspNetCore.Mvc;

namespace Chams.Vtumanager.Provisioning.Services.GloTopup
{
    public class GloTopupService : IGloTopupService
    {

        private readonly IConfiguration _config;
        private readonly ILogger<GloTopupService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly GloTopupSettings _settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="clientFactory"></param>
        public GloTopupService(
            IConfiguration config,
            ILogger<GloTopupService> logger,
IHttpClientFactory clientFactory)
        {
            _config = config;
            _logger = logger;
            _clientFactory = clientFactory;
            _settings = new GloTopupSettings();
            _settings = _config.GetSection("GloTopupSettings").Get<GloTopupSettings>();
        }

        /// <summary>
        /// Airtel Glo Airtime Request
        /// </summary>
        /// <param name="pinRechargeRequest"></param>
        /// <returns></returns>
        public async Task<GloAirtimeResultEnvelope.Envelope> GloAirtimeRecharge(PinlessRechargeRequest pinRechargeRequest)
        {

            _logger.LogInformation($"calling GloAirtimeRecharge svc for transId : {pinRechargeRequest.transId}");
            string dealerNo = _config["GloTopupSettings:InitiatorPrincipal:DealerNo"];
            string password = _config["GloTopupSettings:InitiatorPrincipal:Password"];
            string Id = _config["GloTopupSettings:InitiatorPrincipal:Id"];
            string userId = _config["GloTopupSettings:InitiatorPrincipal:UserId"];
            

            GloAirtimeResultEnvelope.Envelope resultEnvelope = new GloAirtimeResultEnvelope.Envelope();
            try
            {

                //string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var sb = new System.Text.StringBuilder(487);
                sb.Append(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:vtu=""http://vtu.glo.com"">");
                sb.Append(@"   <soapenv:Header/>");
                sb.Append(@"   <soapenv:Body>");
                sb.Append(@"      <vtu:Vend>");
                sb.Append(@"         <DestAccount>" + pinRechargeRequest.Msisdn + "</DestAccount>");
                sb.Append(@"         <Amount>" + pinRechargeRequest.Amount + "</Amount>");
                sb.Append(@"         <Msg>Airtime Purchase</Msg>");
                sb.Append(@"         <SequenceNo>"+ pinRechargeRequest.transId + "</SequenceNo>");
                sb.Append(@"         <DealerNo>" + dealerNo + "</DealerNo>");
                sb.Append(@"         <Password>"+ password + "</Password>");
                sb.Append(@"      </vtu:Vend>");
                sb.Append(@"   </soapenv:Body>");
                sb.Append(@"</soapenv:Envelope>");

                //var sb = new System.Text.StringBuilder(487);
                //sb.Append(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:vtu=""http://vtu.glo.com"">");
                //sb.Append(@"   <soapenv:Header/>");
                //sb.Append(@"   <soapenv:Body>");
                //sb.Append(@"      <vtu:Vend>");
                //sb.Append(@"         <DestAccount>08055555108</DestAccount>");
                //sb.Append(@"         <Amount>50</Amount>");
                //sb.Append(@"         <Msg>Airtiem Purchase</Msg>");
                //sb.Append(@"         <SequenceNo>202207151848453</SequenceNo>");
                //sb.Append(@"         <DealerNo>05720150819131328AG</DealerNo>");
                //sb.Append(@"         <Password>mmt10mmt10</Password>");
                //sb.Append(@"      </vtu:Vend>");
                //sb.Append(@"   </soapenv:Body>");
                //sb.Append(@"</soapenv:Envelope>");


                _logger.LogInformation($"GloAirtimeRecharge soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("GloRechargeClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);



                var request = new HttpRequestMessage(HttpMethod.Post, _settings.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling GloAirtimeRecharge URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStringAsync();
                        _logger.LogWarning($"api call GloAirtimeRecharge returned with status code: {response.StatusCode} validationErrors: -- {errorStream} --");

                        

                        // var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        // _logger.LogWarning($"GloAirtimeRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        var contentStream = await response.Content.ReadAsStringAsync();

                        _logger.LogInformation($"GloAirtimeRecharge response = {contentStream}");


                        using (var stringReader = new StringReader(contentStream))
                        {
                            using (XmlReader reader = new XmlTextReader(stringReader))
                            {
                                var serializer = new XmlSerializer(typeof(GloAirtimeResultEnvelope.Envelope));
                                resultEnvelope = serializer.Deserialize(reader) as GloAirtimeResultEnvelope.Envelope;
                            }
                        }
                    }

                    

                }
            }

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"GloAirtimeRecharge svc failed for transId : {pinRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }

        public async Task<GloDataResultEnvelope.Envelope> GloDataRecharge(PinlessRechargeRequest pinRechargeRequest)
        {

            _logger.LogInformation($"calling GloDataRecharge svc for transId : {pinRechargeRequest.transId}");
            string dealerNo = _config["GloTopupSettings:InitiatorPrincipal:DealerNo"];
            string password = _config["GloTopupSettings:InitiatorPrincipal:Password"];
            string Id = _config["GloTopupSettings:InitiatorPrincipal:Id"];
            string userId = _config["GloTopupSettings:InitiatorPrincipal:UserId"];

            GloDataResultEnvelope.Envelope resultEnvelope = new GloDataResultEnvelope.Envelope();
            try
            {

                string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var sb = new System.Text.StringBuilder(2079);
                sb.Append(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ext=""http://external.interfaces.ers.seamless.com/"">");
                sb.Append(@" <soapenv:Header/>");
                sb.Append(@" <soapenv:Body>");
                sb.Append(@"    <ext:requestTopup>");
                sb.Append(@"       <!--Optional:-->");
                sb.Append(@"       <context>");
                sb.Append(@"          <channel>WSClient</channel>");
                sb.Append(@"          <clientComment>" + pinRechargeRequest.transId + "</clientComment>");
                sb.Append(@"          <clientId>ERS</clientId>");
                sb.Append(@"          <prepareOnly>false</prepareOnly>");
                sb.Append(@"          <clientReference>" + pinRechargeRequest.transId + "</clientReference>");
                sb.Append(@"          <clientRequestTimeout>500</clientRequestTimeout>");
                sb.Append(@"          <initiatorPrincipalId>");
                sb.Append(@"                <id>" + Id + "</id>");
                sb.Append(@"                <type>RESELLERUSER</type>");
                sb.Append(@"                <userId>" + userId + "</userId>");
                sb.Append(@"          </initiatorPrincipalId>");
                sb.Append(@"          <password>" + password + "</password>");
                sb.Append(@"          <transactionProperties>");
                sb.Append(@"             <!--Zero or more repetitions:-->");
                sb.Append(@"             <entry>");
                sb.Append(@"                      <key>TRANSACTION_TYPE</key>");
                sb.Append(@"                      <value>PRODUCT_RECHARGE</value>");
                sb.Append(@"             </entry>");
                sb.Append(@"          </transactionProperties>");
                sb.Append(@"       </context>");
                sb.Append(@"       <senderPrincipalId>");
                sb.Append(@"          <id>" + Id + "</id>");
                sb.Append(@"          <type>RESELLERUSER</type>");
                sb.Append(@"          <userId>" + userId + "</userId>");
                sb.Append(@"       </senderPrincipalId>");
                sb.Append(@"       <topupPrincipalId>");
                sb.Append(@"          <id>" + pinRechargeRequest.Msisdn + "</id>");
                sb.Append(@"          <type>SUBSCRIBERMSISDN</type>");
                sb.Append(@"          <userId></userId>");
                sb.Append(@"       </topupPrincipalId>");
                sb.Append(@"       <!--Optional:-->");
                sb.Append(@"       <senderAccountSpecifier>");
                sb.Append(@"          <accountId>" + Id + "</accountId>");
                sb.Append(@"          <accountTypeId>RESELLER</accountTypeId>");
                sb.Append(@"       </senderAccountSpecifier>");
                sb.Append(@"       <!--Optional:-->");
                sb.Append(@"       <topupAccountSpecifier>");
                sb.Append(@"          <accountId>" + pinRechargeRequest.Msisdn + "</accountId>");
                sb.Append(@"          <accountTypeId>DATA_BUNDLE</accountTypeId>");
                sb.Append(@"       </topupAccountSpecifier>");
                sb.Append(@"       <!--Optional:-->");
                sb.Append(@"       <productId>" + pinRechargeRequest.ProductCode + "</productId>");
                sb.Append(@"       <!--Optional:-->");
                sb.Append(@"       <amount>");
                sb.Append(@"          <currency>NGN</currency>");
                sb.Append(@"          <value>" + pinRechargeRequest.Amount + "</value>");
                sb.Append(@"       </amount>");
                sb.Append(@"    </ext:requestTopup>");
                sb.Append(@" </soapenv:Body>");
                sb.Append(@"</soapenv:Envelope>");


                _logger.LogInformation($"GloDataRecharge soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("GloRechargeClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);


                var request = new HttpRequestMessage(HttpMethod.Post, _settings.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling GloDataRecharge URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStringAsync();
                        _logger.LogWarning($"api call GloDataRecharge returned with status code: {response.StatusCode} validationErrors: -- {errorStream} --");


                    }
                    if (response.IsSuccessStatusCode)
                    {
                        var contentStream = await response.Content.ReadAsStringAsync();

                        _logger.LogInformation($"GloDataRecharge response = {contentStream}");


                        using (var stringReader = new StringReader(contentStream))
                        {
                            using (XmlReader reader = new XmlTextReader(stringReader))
                            {
                                var serializer = new XmlSerializer(typeof(GloDataResultEnvelope.Envelope));
                                resultEnvelope = serializer.Deserialize(reader) as GloDataResultEnvelope.Envelope;
                            }
                        }
                    }

                    

                }
            }

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"GloDataRecharge svc failed for transId : {pinRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }

        public async Task<QueryTxnStatusResponse> QueryTransactionStatus(QueryTransactionStatusRequest queryTransaction)
        {

            _logger.LogInformation($"calling Glo QueryTransactionStatus svc for transId : { queryTransaction.transactionId}");

            GloQueryTxnResponse.Envelope resultEnvelope = null ;
            QueryTxnStatusResponse queryTxnStatusResponse = new QueryTxnStatusResponse();
            try
            {

                string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                var sb = new System.Text.StringBuilder(1158);
                sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                sb.Append(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ext=""http://external.interfaces.ers.seamless.com/"">");
                sb.Append(@"   <soapenv:Header />");
                sb.Append(@"   <soapenv:Body>");
                sb.Append(@"      <ext:executeReport>");
                sb.Append(@"         <context>");
                sb.Append(@"            <channel>WSClient</channel>");
                sb.Append(@"            <clientComment>?</clientComment>");
                sb.Append(@"            <clientId>ERS</clientId>");
                sb.Append(@"            <clientReference>?</clientReference>");
                sb.Append(@"            <clientRequestTimeout>500</clientRequestTimeout>");
                sb.Append(@"            <initiatorPrincipalId>");
                sb.Append(@"               <id>DIST1</id>");
                sb.Append(@"               <type>RESELLERUSER</type>");
                sb.Append(@"               <userId>9900</userId>");
                sb.Append(@"            </initiatorPrincipalId>");
                sb.Append(@"            <password>2015</password>");
                sb.Append(@"         </context>");
                sb.Append(@"         <reportId>LAST_TRANSACTION</reportId>");
                sb.Append(@"         <language>en</language>");
                sb.Append(@"         <parameters>");
                sb.Append(@"            <parameter>");
                sb.Append(@"               <!--Zero or more repetitions:-->");
                sb.Append(@"               <entry>");
                sb.Append(@"                  <key>?</key>");
                sb.Append(@"                  <value>?</value>");
                sb.Append(@"               </entry>");
                sb.Append(@"            </parameter>");
                sb.Append(@"         </parameters>");
                sb.Append(@"      </ext:executeReport>");
                sb.Append(@"   </soapenv:Body>");
                sb.Append(@"</soapenv:Envelope>");




                _logger.LogInformation($"QueryTransactionStatus soap request = {sb.ToString()}");  //


                var httpClient = _clientFactory.CreateClient("GloRechargeClient");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);


                var request = new HttpRequestMessage(HttpMethod.Post, _settings.Url)
                {
                    Content = new StringContent(Regex.Unescape(sb.ToString()), Encoding.UTF8, "text/xml"),
                };
                _logger.LogInformation($"Calling GloQueryTransactionStatus URL  {request.RequestUri}");
                //request.Headers.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"QueryTransactionStatus api call returned with status code {response.StatusCode} {validationErrors}");
                    }
                    var contentStream = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"Glo QueryTransactionStatus response = {contentStream}");


                    using (var stringReader = new StringReader(contentStream))
                    {
                        using (XmlReader reader = new XmlTextReader(stringReader))
                        {
                            var serializer = new XmlSerializer(typeof(GloQueryTxnResponse.Envelope));
                            resultEnvelope = serializer.Deserialize(reader) as GloQueryTxnResponse.Envelope;
                        }
                    }

                }

                if(resultEnvelope !=null)
                {
                    queryTxnStatusResponse.statusId = resultEnvelope.Body.executeReportResponse.@return.resultCode.ToString();
                    queryTxnStatusResponse.responseMessage = resultEnvelope.Body.executeReportResponse.@return.resultDescription;
                    queryTxnStatusResponse.exchangeReference = resultEnvelope.Body.executeReportResponse.@return.report.contentString;
                    queryTxnStatusResponse.transactionReference = queryTransaction.transactionId;
                }
            }

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"Glo QueryTransactionStatus svc failed for transId : {queryTransaction.transactionId} with error {JsonConvert.SerializeObject(ex)}");
            }

            return queryTxnStatusResponse;
        }
    }
}
