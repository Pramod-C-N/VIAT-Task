using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace vita.Web.Middleware
{
    internal class Shared
    {
        public class Ref<T>
        {
            public Ref() { }
            public Ref(T value) { Value = value; }
            public T Value { get; set; }
            public override string ToString()
            {
                T value = Value;
                return value == null ? "" : value.ToString();
            }
            public static implicit operator T(Ref<T> r) { return r.Value; }
            public static implicit operator Ref<T>(T value) { return new Ref<T>(value); }
        }

        public async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            string body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        public async Task UpdateResponseBodyAsync(Ref<HttpContext> context, int statusCode, object body)
        {
            context.Value.Response.StatusCode = statusCode;
            context.Value.Response.Headers.Add("Content-Type", "application/json");
            await context.Value.Response.WriteAsync(JsonConvert.SerializeObject(body));
            await context.Value.Response.CompleteAsync();
            return;
        }
    }
}
