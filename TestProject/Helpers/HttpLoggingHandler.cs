using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.Helpers
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public HttpLoggingHandler(HttpMessageHandler innerHandler = null) : base(
            innerHandler ?? new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var start = DateTime.Now;
            var req = request;
            var msg = $"[{req.RequestUri.AbsolutePath} -  Request]";
            int logMsgLen = 10000;

            Logger.Trace($"{msg}========Request Start==========");
            Logger.Trace($"{msg} {req.Method} {req.RequestUri.Scheme}/{req.Version} {req.RequestUri.AbsoluteUri}");
            //Logger.Trace($"{msg} Host: {req.RequestUri.Scheme}://{req.RequestUri.Host}");

            foreach (var header in req.Headers)
            {
                Logger.Trace($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
            }

            if (req.Content != null)
            {
                foreach (var header in req.Content.Headers)
                {
                    Logger.Trace($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
                }

                Logger.Trace($"{msg} Content: {req.Content}");

                if (req.Content is StringContent || IsTextBasedContentType(req.Headers) || IsTextBasedContentType(req.Content.Headers))
                {
                    var result = await req.Content.ReadAsStringAsync();

                    Logger.Trace($"{msg} {result.Truncate(logMsgLen)}");
                }
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            Logger.Trace($"{msg}==========Request End==========");

            msg = $"[{req.RequestUri.AbsolutePath} - Response]";

            Logger.Trace($"{msg}=========Response Start=========");

            var resp = response;

            Logger.Trace($"{msg} {req.RequestUri.Scheme.ToUpper()}/{resp.Version} {(int)resp.StatusCode} {resp.ReasonPhrase}");

            foreach (var header in resp.Headers)
            {
                Logger.Trace($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
            }

            if (resp.Content != null)
            {
                foreach (var header in resp.Content.Headers)
                {
                    Logger.Trace($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
                }

                Logger.Trace($"{msg} Content: {resp.Content}");

                if (resp.Content is StringContent || IsTextBasedContentType(resp.Headers) || IsTextBasedContentType(resp.Content.Headers))
                {
                    var result = await resp.Content.ReadAsStringAsync();

                    Logger.Trace($"{msg} {result.Truncate(logMsgLen)}");
                }
            }

            Logger.Trace($"{msg} Duration: {DateTime.Now - start}");
            Logger.Trace($"{msg}==========Response End==========");
            return response;
        }

        readonly string[] types = { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

        private bool IsTextBasedContentType(HttpHeaders headers)
        {
            IEnumerable<string> values;
            if (!headers.TryGetValues("Content-Type", out values))
                return false;
            var header = string.Join(" ", values).ToLowerInvariant();

            return types.Any(t => header.Contains(t));
        }
    }
}
