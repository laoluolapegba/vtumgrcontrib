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
            GloAirtimeResultEnvelope.Envelope resultEnvelope = new GloAirtimeResultEnvelope.Envelope();
            try
            {

                //string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var sb = new System.Text.StringBuilder(487);
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:vtu=""http://vtu.glo.com"">");
                sb.AppendLine(@"   <soapenv:Header/>");
                sb.AppendLine(@"   <soapenv:Body>");
                sb.AppendLine(@"      <vtu:Vend>");
                sb.AppendLine(@"         <DestAccount>" + pinRechargeRequest.Msisdn + "</DestAccount>");
                sb.AppendLine(@"         <Amount>" + pinRechargeRequest.Amount + "</Amount>");
                sb.AppendLine(@"         <Msg>Airtime Purchase</Msg>");
                sb.AppendLine(@"         <SequenceNo>"+ pinRechargeRequest.transId + "</SequenceNo>");
                sb.AppendLine(@"         <DealerNo>" + dealerNo + "</DealerNo>");
                sb.AppendLine(@"         <Password>"+ password + "</Password>");
                sb.AppendLine(@"      </vtu:Vend>");
                sb.AppendLine(@"   </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                //var sb = new System.Text.StringBuilder(487);
                //sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:vtu=""http://vtu.glo.com"">");
                //sb.AppendLine(@"   <soapenv:Header/>");
                //sb.AppendLine(@"   <soapenv:Body>");
                //sb.AppendLine(@"      <vtu:Vend>");
                //sb.AppendLine(@"         <DestAccount>08055555108</DestAccount>");
                //sb.AppendLine(@"         <Amount>50</Amount>");
                //sb.AppendLine(@"         <Msg>Airtiem Purchase</Msg>");
                //sb.AppendLine(@"         <SequenceNo>202207151848453</SequenceNo>");
                //sb.AppendLine(@"         <DealerNo>05720150819131328AG</DealerNo>");
                //sb.AppendLine(@"         <Password>mmt10mmt10</Password>");
                //sb.AppendLine(@"      </vtu:Vend>");
                //sb.AppendLine(@"   </soapenv:Body>");
                //sb.AppendLine(@"</soapenv:Envelope>");


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
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"GloAirtimeRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
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

            GloDataResultEnvelope.Envelope resultEnvelope = new GloDataResultEnvelope.Envelope();
            try
            {

                string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var sb = new System.Text.StringBuilder(2079);
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ext=""http://external.interfaces.ers.seamless.com/"">");
                sb.AppendLine(@" <soapenv:Header/>");
                sb.AppendLine(@" <soapenv:Body>");
                sb.AppendLine(@"    <ext:requestTopup>");
                sb.AppendLine(@"       <!--Optional:-->");
                sb.AppendLine(@"       <context>");
                sb.AppendLine(@"          <channel>WSClient</channel>");
                sb.AppendLine(@"          <clientComment>" + pinRechargeRequest.transId + "</clientComment>");
                sb.AppendLine(@"          <clientId>ERS</clientId>");
                sb.AppendLine(@"          <prepareOnly>false</prepareOnly>");
                sb.AppendLine(@"          <clientReference>" + pinRechargeRequest.transId + "</clientReference>");
                sb.AppendLine(@"          <clientRequestTimeout>500</clientRequestTimeout>");
                sb.AppendLine(@"          <initiatorPrincipalId>");
                sb.AppendLine(@"                <id>" + _settings.Initiator.Id + "</id>");
                sb.AppendLine(@"                <type>RESELLERUSER</type>");
                sb.AppendLine(@"                <userId>" + _settings.Initiator.UserId + "</userId>");
                sb.AppendLine(@"          </initiatorPrincipalId>");
                sb.AppendLine(@"          <password>" + _settings.Initiator.Password + "</password>");
                sb.AppendLine(@"          <transactionProperties>");
                sb.AppendLine(@"             <!--Zero or more repetitions:-->");
                sb.AppendLine(@"             <entry>");
                sb.AppendLine(@"                      <key>TRANSACTION_TYPE</key>");
                sb.AppendLine(@"                      <value>PRODUCT_RECHARGE</value>");
                sb.AppendLine(@"             </entry>");
                sb.AppendLine(@"          </transactionProperties>");
                sb.AppendLine(@"       </context>");
                sb.AppendLine(@"       <senderPrincipalId>");
                sb.AppendLine(@"          <id>" + _settings.Initiator.Id + "</id>");
                sb.AppendLine(@"          <type>RESELLERUSER</type>");
                sb.AppendLine(@"          <userId>" + _settings.Initiator.UserId + "</userId>");
                sb.AppendLine(@"       </senderPrincipalId>");
                sb.AppendLine(@"       <topupPrincipalId>");
                sb.AppendLine(@"          <id>" + pinRechargeRequest.Msisdn + "</id>");
                sb.AppendLine(@"          <type>SUBSCRIBERMSISDN</type>");
                sb.AppendLine(@"          <userId></userId>");
                sb.AppendLine(@"       </topupPrincipalId>");
                sb.AppendLine(@"       <!--Optional:-->");
                sb.AppendLine(@"       <senderAccountSpecifier>");
                sb.AppendLine(@"          <accountId>" + _settings.Initiator.Id + "</accountId>");
                sb.AppendLine(@"          <accountTypeId>RESELLER</accountTypeId>");
                sb.AppendLine(@"       </senderAccountSpecifier>");
                sb.AppendLine(@"       <!--Optional:-->");
                sb.AppendLine(@"       <topupAccountSpecifier>");
                sb.AppendLine(@"          <accountId>" + pinRechargeRequest.Msisdn + "</accountId>");
                sb.AppendLine(@"          <accountTypeId>DATA_BUNDLE</accountTypeId>");
                sb.AppendLine(@"       </topupAccountSpecifier>");
                sb.AppendLine(@"       <!--Optional:-->");
                sb.AppendLine(@"       <productId>" + pinRechargeRequest.ProductCode + "</productId>");
                sb.AppendLine(@"       <!--Optional:-->");
                sb.AppendLine(@"       <amount>");
                sb.AppendLine(@"          <currency>NGN</currency>");
                sb.AppendLine(@"          <value>" + pinRechargeRequest.Amount + "</value>");
                sb.AppendLine(@"       </amount>");
                sb.AppendLine(@"    </ext:requestTopup>");
                sb.AppendLine(@" </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");


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
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"GloDataRecharge api call returned with status code {response.StatusCode} {validationErrors}");
                    }
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

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"GloDataRecharge svc failed for transId : {pinRechargeRequest.transId} with error {ex}");
            }

            return resultEnvelope;
        }

        public async Task<GloDataResultEnvelope.Envelope> QueryTransactionStatus(QueryTransactionStatusRequest queryTransaction)
        {

            _logger.LogInformation($"calling Glo QueryTransactionStatus svc for transId : { queryTransaction.transactionId}");

            GloDataResultEnvelope.Envelope resultEnvelope = new GloDataResultEnvelope.Envelope();
            try
            {

                string tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var sb = new System.Text.StringBuilder(2079);
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ext=""http://external.interfaces.ers.seamless.com/"">");
                sb.AppendLine(@" <soapenv:Header/>");
                sb.AppendLine(@" <soapenv:Body>");
                sb.AppendLine(@"    <ext:requestTopup>");
                sb.AppendLine(@"       <!--Optional:-->");
                sb.AppendLine(@"       <context>");
                sb.AppendLine(@"          <channel>WSClient</channel>");
                sb.AppendLine(@"          <clientComment>" + queryTransaction.transactionId + "</clientComment>");
                sb.AppendLine(@"          <clientId>ERS</clientId>");
                sb.AppendLine(@"          <prepareOnly>false</prepareOnly>");
                sb.AppendLine(@"          <clientReference>" + queryTransaction.transactionId + "</clientReference>");
                sb.AppendLine(@"          <clientRequestTimeout>500</clientRequestTimeout>");
                sb.AppendLine(@"          <initiatorPrincipalId>");
                sb.AppendLine(@"                <id>" + _settings.Initiator.Id + "</id>");
                sb.AppendLine(@"                <type>RESELLERUSER</type>");
                sb.AppendLine(@"                <userId>" + _settings.Initiator.UserId + "</userId>");
                sb.AppendLine(@"          </initiatorPrincipalId>");
                sb.AppendLine(@"          <password>" + _settings.Initiator.Password + "</password>");
                sb.AppendLine(@"          <transactionProperties>");



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
                _logger.LogInformation($"Calling GloDataRecharge URL  {request.RequestUri}");
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

            //try
            //{
            //}
            catch (Exception ex)
            {
                _logger.LogError($"QueryTransactionStatus svc failed for transId : {queryTransaction.transactionId} with error {ex}");
            }

            return resultEnvelope;
        }
    }
}
