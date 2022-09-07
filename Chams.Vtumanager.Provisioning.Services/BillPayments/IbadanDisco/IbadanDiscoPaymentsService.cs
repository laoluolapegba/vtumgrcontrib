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
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Services.BillPayments.IbadanDisco;
using Chams.Vtumanager.Provisioning.Entities.IbadanDisco;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Multichoice
{
    public class IbadanDiscoPaymentsService : IIbadanDiscoPaymentsService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<IIbadanDiscoPaymentsService> _logger;
        private readonly IConfiguration _configuration;
        public IbadanDiscoPaymentsService(
            ILogger<IIbadanDiscoPaymentsService> logger,
            IConfiguration configuration,
            IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
            _logger = logger;
            _configuration = configuration;



        }



        #region IbadanDisco
        public async Task<BillPaymentsResponse> IbadanDiscoPrepaidAsync(IbadanDiscoRequest paymentRequest, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Inside EkoElectricPrepaidAsync service request");

            BillPaymentsResponse result = new BillPaymentsResponse();

            string gatewayURL = _configuration["BaxiBillsAPI:URL"];

            var httpClient = _httpFactory.CreateClient("BaxiBillsAPI");

            string baxi_Username = _configuration["BaxiBillsAPI:BAXI_USERNAME"];
            string BAXI_SEC_TOKEN = _configuration["BaxiBillsAPI:BAXI_SEC_TOKEN"];


            string jsonRequest = JsonConvert.SerializeObject(paymentRequest); //  JsonConvert.SerializeObject(dstvRequest);

            string sha256 = sha256hash(jsonRequest);


            string hashedPayload = HexString2B64String(sha256);
            //Console.WriteLine("hashedPayload=" + hashedPayload);
            var x_mspdate = DateTime.UtcNow; // DateTime.Now.ToString("R"); Wed, 17 Aug 2022 21:14:00 GMT
            Int32 unixTimestamp = (int)x_mspdate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            string serveletPath = _configuration["BaxiBillsAPI:ServletPath"];

            StringBuilder strToSign = new StringBuilder();
            strToSign.Append("POST");
            //strToSign.Append( Environment.NewLine);
            strToSign.Append(serveletPath);
            //strToSign.Append(Environment.NewLine);
            strToSign.Append(unixTimestamp);
            strToSign.Append(hashedPayload);

            //var requestData = "POST/rest/consumer/v2/exchange16607235533RvQHdls/XuKe4a28q8HeK2xDZTYd7oo1YPCUedRsGM=";
            //POST/rest/consumer/v2/exchange16607238733RvQHdls/XuKe4a28q8HeK2xDZTYd7oo1YPCUedRsGM=

            string signature = HexString2B64String(ComputeHMAC_SHA1(strToSign.ToString(), BAXI_SEC_TOKEN));

            string authHeader = "Baxi" + " " + baxi_Username + ":" + signature;  //Baxi baxi_ZN1GmmLtE:mmKFvIJUA9ZEQyFoIJlrFYpR3gU=
            //authHeader = "Baxi baxi_ZN1GmmLtE:+tcdGZB7tqwXrnRsgLRcMVW5Ydg=";

            httpClient.DefaultRequestHeaders.Add("Authorization", authHeader); //[{"key":"x-msp-date","value":"{{x-msp-date}}","type":"text"}]
            httpClient.DefaultRequestHeaders.Add("x-msp-date", x_mspdate.ToString("R")); // x_mspdate.ToString("R"));


            var httpRequest = new HttpRequestMessage(HttpMethod.Post, gatewayURL);
            _logger.LogInformation($"Calling DstvPaymentAsync  {httpRequest.RequestUri}");

            using (var httpContent = CreateHttpContent(paymentRequest))
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
                        //var dstverror = JsonConvert.DeserializeObject<DstvError>(errorStream.ToString());

                        result = JsonConvert.DeserializeObject<BillPaymentsResponse>(errorStream.ToString());

                    }
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStream = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation($"api call DstvPaymentAsync returned with contentstream  {contentStream}");
                        result = JsonConvert.DeserializeObject<BillPaymentsResponse>(contentStream);
                    }

                }
            }

            return result;
        }


        public async Task<BillPaymentsResponse> IbadanDiscoPostpaidAsync(IbadanDiscoRequest paymentRequest, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Inside EkoElectricPostpaidAsync service request");

            BillPaymentsResponse result = new BillPaymentsResponse();

            string gatewayURL = _configuration["BaxiBillsAPI:URL"];

            var httpClient = _httpFactory.CreateClient("BaxiBillsAPI");

            string baxi_Username = _configuration["BaxiBillsAPI:BAXI_USERNAME"];
            string BAXI_SEC_TOKEN = _configuration["BaxiBillsAPI:BAXI_SEC_TOKEN"];


            //byte[] decodedSecretToken = Convert.FromBase64String(BAXI_SEC_TOKEN);


            //var js = new JavaScriptSerializer(); string jsonRequest  = js.Serialize(dstvRequest);

            string jsonRequest = JsonConvert.SerializeObject(paymentRequest); //  JsonConvert.SerializeObject(dstvRequest);

            string sha256 = sha256hash(jsonRequest);


            string hashedPayload = HexString2B64String(sha256);
            //Console.WriteLine("hashedPayload=" + hashedPayload);
            var x_mspdate = DateTime.UtcNow; // DateTime.Now.ToString("R"); Wed, 17 Aug 2022 21:14:00 GMT
            Int32 unixTimestamp = (int)x_mspdate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            string serveletPath = _configuration["BaxiBillsAPI:ServletPath"];

            StringBuilder strToSign = new StringBuilder();
            strToSign.Append("POST");
            //strToSign.Append( Environment.NewLine);
            strToSign.Append(serveletPath);
            //strToSign.Append(Environment.NewLine);
            strToSign.Append(unixTimestamp);
            strToSign.Append(hashedPayload);

            //var requestData = "POST/rest/consumer/v2/exchange16607235533RvQHdls/XuKe4a28q8HeK2xDZTYd7oo1YPCUedRsGM=";
            //POST/rest/consumer/v2/exchange16607238733RvQHdls/XuKe4a28q8HeK2xDZTYd7oo1YPCUedRsGM=

            string signature = HexString2B64String(ComputeHMAC_SHA1(strToSign.ToString(), BAXI_SEC_TOKEN));

            string authHeader = "Baxi" + " " + baxi_Username + ":" + signature;  //Baxi baxi_ZN1GmmLtE:mmKFvIJUA9ZEQyFoIJlrFYpR3gU=
            //authHeader = "Baxi baxi_ZN1GmmLtE:+tcdGZB7tqwXrnRsgLRcMVW5Ydg=";

            httpClient.DefaultRequestHeaders.Add("Authorization", authHeader); //[{"key":"x-msp-date","value":"{{x-msp-date}}","type":"text"}]
            httpClient.DefaultRequestHeaders.Add("x-msp-date", x_mspdate.ToString("R")); // x_mspdate.ToString("R"));


            var httpRequest = new HttpRequestMessage(HttpMethod.Post, gatewayURL);
            _logger.LogInformation($"Calling DstvPaymentAsync  {httpRequest.RequestUri}");

            using (var httpContent = CreateHttpContent(paymentRequest))
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
                        //var dstverror = JsonConvert.DeserializeObject<DstvError>(errorStream.ToString());

                        result = JsonConvert.DeserializeObject<BillPaymentsResponse>(errorStream.ToString());

                    }
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStream = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation($"api call DstvPaymentAsync returned with contentstream  {contentStream}");
                        result = JsonConvert.DeserializeObject<BillPaymentsResponse>(contentStream);
                    }

                }
            }

            return result;
        }

        #endregion
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

        public static string ComputeHMAC_SHA1(string input, string keystring)
        {
            byte[] key = Encoding.UTF8.GetBytes(keystring);
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
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(toEncode);
            string returnVal = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnVal;
        }
        public static string HexString2B64String(string input)
        {
            return System.Convert.ToBase64String(HexStringToHex(input));
        }
        public static byte[] HexStringToHex(string inputHex)
        {
            var resultantArray = new byte[inputHex.Length / 2];
            for (var i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = System.Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }

    }
}
