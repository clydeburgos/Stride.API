using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Stride.Domain.Type;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;


namespace Stride.Core.Utility.Helpers
{
    public class HttpRequestHelper
    {
        public static async Task<string> SendRequestAsync<T>(HttpClient client, HttpRequestMethodsEnum httpMethod, string apiPath, T? payload, string authKey = "")
        {

            var payloadToJson = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(authKey))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authKey}");
            }

            if (httpMethod == HttpRequestMethodsEnum.GET)
            {
                using var response = await client.GetAsync(apiPath);
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else if (httpMethod == HttpRequestMethodsEnum.POST)
            {
                using var response = await client.PostAsync(apiPath, payloadToJson);
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else if (httpMethod == HttpRequestMethodsEnum.PUT)
            {
                using var response = await client.PutAsync(apiPath, payloadToJson);
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else if (httpMethod == HttpRequestMethodsEnum.DELETE)
            {
                using var response = await client.DeleteAsync(apiPath);
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            return string.Empty;
        }

        public static async Task<Stream> SendRequestAsStreamAsync<T>(HttpClient client, HttpRequestMethodsEnum httpMethod, string apiPath, T? payload, string authKey = "")
        {
            var payloadToJson = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(authKey))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authKey}");
            }

            if (httpMethod == HttpRequestMethodsEnum.GET)
            {
                using var response = await client.GetAsync(apiPath);
                return await response.Content.ReadAsStreamAsync();
            }
            else if (httpMethod == HttpRequestMethodsEnum.POST)
            {
                using var response = await client.PostAsync(apiPath, payloadToJson);
                string responseBody = await response.Content.ReadAsStringAsync();
                return await response.Content.ReadAsStreamAsync();
            }
            else if (httpMethod == HttpRequestMethodsEnum.PUT)
            {
                using var response = await client.PutAsync(apiPath, payloadToJson);
                string responseBody = await response.Content.ReadAsStringAsync();
                return await response.Content.ReadAsStreamAsync();
            }
            else if (httpMethod == HttpRequestMethodsEnum.DELETE)
            {
                using var response = await client.DeleteAsync(apiPath);
                string responseBody = await response.Content.ReadAsStringAsync();
                return await response.Content.ReadAsStreamAsync();
            }
            return null;
        }

        public static async Task<string> SendRequest<T>(HttpClient client, HttpRequestMethodsEnum httpMethod, string baseAddress, string apiPath, string header, dynamic payload, string authKey = "")
        {
            try
            {
                var requestHeader = JsonConvert.DeserializeObject<dynamic>(header);

                apiPath = baseAddress + apiPath;
                var payloadToJson = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                if (!string.IsNullOrEmpty(authKey))
                {
                    client.DefaultRequestHeaders.Add("Authorization", authKey);
                }

                if (requestHeader != null)
                {
                    foreach (var headerItem in requestHeader)
                    {
                        var header_key = headerItem["key"].Value;
                        var header_value = headerItem["value"].Value;
                        client.DefaultRequestHeaders.Add(header_key, header_value);
                    }
                }


                if (httpMethod == HttpRequestMethodsEnum.GET)
                {
                    using var response = await client.GetAsync(apiPath);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else if (httpMethod == HttpRequestMethodsEnum.POST)
                {
                    using var response = await client.PostAsync(apiPath, payloadToJson);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else if (httpMethod == HttpRequestMethodsEnum.PUT)
                {
                    using var response = await client.PutAsync(apiPath, payloadToJson);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else if (httpMethod == HttpRequestMethodsEnum.DELETE)
                {
                    using var response = await client.DeleteAsync(apiPath);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                //log
                return string.Empty;
            }
        }
        public static async Task<string> GetIPAddress(HttpContext context)
        {
            try
            {
                string ip = string.Empty;

                if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
                {
                    ip = context.Request.Headers["X-Forwarded-For"];
                }
                else
                {
                    ip = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
                }

                if (ip == "::1")
                {
                    ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Skip(1).Take(1).FirstOrDefault().ToString();
                }

                if (ip.Contains(":"))
                {
                    ip = ip.Split(':')[0];
                }
                return await Task.FromResult(ip);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string DomainNameExtractor(string url)
        {
            var regex = new Regex(@"^((https?|ftp)://)?(www\.)?(?<domain>[^/]+)(/|$)");
            Match match = regex.Match(url);
            if (match.Success)
            {
                string domain = match.Groups["domain"].Value;
                int freq = domain.Where(x => (x == '.')).Count();
                while (freq > 2)
                {
                    if (freq > 2)
                    {
                        var domainSplited = domain.Split('.', 2);
                        domain = domainSplited[1];
                        freq = domain.Where(x => (x == '.')).Count();
                    }
                }
                return domain;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
