using System;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BitFlyerApiTester
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("API Key を入力してください：");
            var key = Console.ReadLine().Trim();

            Console.WriteLine("API Secret を入力してください：");
            var secret = Console.ReadLine().Trim();

            Hr();

            Console.WriteLine($"Key:Secret = {key}:{secret}");

            Hr();

            Console.WriteLine("API呼び出しを開始します");

            Hr();

            using (var handler = new WebRequestHandler())
            {
                // SSLのプロトコルを TLS1.2 に指定
                //handler.SslProtocols = SslProtocols.Tls12;

                // SSL証明書の検証を全く行わなくする
                //handler.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                using (var hc = new HttpClient(handler))
                {
                    var path = "/v1/me/getpermissions";
                    var req = new HttpRequestMessage(HttpMethod.Get, $"https://api.bitflyer.com{path}");

                    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                    var content = timestamp + req.Method + path;
                    var hash = new HMACSHA256(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(content));

                    req.Headers.TryAddWithoutValidation("ACCESS-KEY", key);
                    req.Headers.TryAddWithoutValidation("ACCESS-TIMESTAMP", timestamp);
                    req.Headers.TryAddWithoutValidation("ACCESS-SIGN", hash.ToHexString());

                    var res = await hc.SendAsync(req).ConfigureAwait(false);
                    var json = await res.Content.ReadAsStringAsync();

                    Console.WriteLine("HTTP Status code:");
                    Console.WriteLine(res.StatusCode);
                    Console.WriteLine("Response body:");
                    Console.WriteLine(json);
                }
            }

            Hr();

            Console.WriteLine("API呼び出しを完了しました");

            Hr();

            Console.WriteLine("Enter(CR) を打鍵で終了します");
            Console.ReadLine();
        }

        private static void Hr()
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.WriteLine();
        }
    }
}
