using System.Net;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public static class HttpHelper
    {
        public static async Task WriteResponse(HttpListenerResponse response, string responseString)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }
    }
}
