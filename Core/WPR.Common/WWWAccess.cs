using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace WPR.Common
{
    public static class WWWAccess
    {
        public static async Task<string> CallUrlForString(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }
        public static async Task<byte[]> CallUrlDownload(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetByteArrayAsync(fullUrl);
            return await response;
        }
    }
}
