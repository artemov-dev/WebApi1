using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace WebApi1
{
    public sealed class NtlmNegotiateHandler : NegotiateHandler
    {
        public NtlmNegotiateHandler(
            IOptionsMonitor<NegotiateOptions> options,
            ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);

            if (Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                Response.Headers.Append(Microsoft.Net.Http.Headers.HeaderNames.WWWAuthenticate, "NTLM");
            }
        }
    }
}
