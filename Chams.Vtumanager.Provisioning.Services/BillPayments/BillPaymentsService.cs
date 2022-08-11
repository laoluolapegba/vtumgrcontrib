using Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Marvin.StreamExtensions;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Dstv;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments
{
    public class BillPaymentsService :IBillPaymentsService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<BillPaymentsService> _logger;
        private readonly IConfiguration _configuration;
        public BillPaymentsService(
            ILogger<BillPaymentsService> logger,
            IConfiguration configuration,
            IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
            _logger = logger;
            _configuration = configuration;



        }
        public async Task<DstvResponse> DstvPaymentAsync(DstvRequest dstvRequest, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Inside SearchbyPhoneNoAsync service request");

            DstvResponse result = new DstvResponse();

            string gatewayURL = _configuration["BaxiBillsAPI:URL"];

            var httpClient = _httpFactory.CreateClient("BaxiBillsAPI");

            string baxi_Username = _configuration["BaxiBillsAPI:BAXI_USERNAME"];
            string BAXI_SEC_TOKEN = _configuration["BaxiBillsAPI:BAXI_SEC_TOKEN"];


            byte[] decodedSecretToken = Convert.FromBase64String(BAXI_SEC_TOKEN);

            

            var jsonRequest = JsonConvert.SerializeObject(dstvRequest);
            string hashedPayload = EncodetoBase64(sha256hash(jsonRequest));
            var x_mspdate = DateTime.Now.ToString("R");
            string serveletPath = _configuration["BaxiBillsAPI:ServletPath"];

            StringBuilder strToSign = new StringBuilder();
            strToSign.Append("POST");
            strToSign.Append( Environment.NewLine);
            strToSign.Append(hashedPayload);
            strToSign.Append(Environment.NewLine);
            strToSign.Append(x_mspdate);
            strToSign.Append(Environment.NewLine);
            strToSign.Append(serveletPath);

            ////Signature = Base64(HMAC-SHA1([BAXI SEC. TOKEN], UTF-8-Encoding(StringToSign)));
            string signature = EncodetoBase64( ComputeHMAC_SHA1(strToSign.ToString(), decodedSecretToken));

            string authHeader = "MSP" + " " + baxi_Username + ":" + signature;

            httpClient.DefaultRequestHeaders.Add("Authorization", authHeader); //[{"key":"x-msp-date","value":"{{x-msp-date}}","type":"text"}]
            httpClient.DefaultRequestHeaders.Add("x-msp-date", x_mspdate);


            var httpRequest = new HttpRequestMessage(HttpMethod.Get, gatewayURL);
            _logger.LogInformation($"Calling DstvPaymentAsync  {httpRequest.RequestUri}");

            using (var httpContent = CreateHttpContent(dstvRequest))
            {
                httpRequest.Content = httpContent;

                using (var response = await httpClient
                    .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    //response.EnsureSuccessStatusCode();
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"api call DstvPaymentAsync returned with statusCode {response.StatusCode} reason: {response.ReasonPhrase}");
                        var errorStream = await response.Content.ReadAsStringAsync();

                        //var validationErrors = errorStream.ReadAndDeserializeFromJson();
                        _logger.LogWarning($"api call DstvPaymentAsync returned with status code: {response.StatusCode} validationErrors: -- {errorStream} --");

                        result = JsonConvert.DeserializeObject<DstvResponse>(errorStream.ToString());

                    }
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStream = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation($"api call DstvPaymentAsync returned with contentstream  {contentStream}");
                        result = JsonConvert.DeserializeObject<DstvResponse>(contentStream);
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// serialize to stream instead of string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;
            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            return httpContent;
        }
        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }

        public static string ComputeHMAC_SHA1(string input, byte[] key)
        {
            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            return myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        }
        public static string sha256hash(string stringtohash)
        {
            return String.Concat(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(stringtohash)).Select(item => item.ToString("x2")));
        }

        public static string EncodetoBase64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnVal = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnVal;
        }
    }
}
