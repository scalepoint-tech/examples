using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Scalepoint.OAuth.TokenClient;

namespace ScalepointIdGetToken
{
    /// <summary>
    /// This sample explains the principles behind Scalepoint Authentification described at https://dev.scalepoint.com/authentication
    /// run from commandline example:
    /// dotnet "future_insurance" "ClientCertificate.pfx" "certificatePassword" "case_integration"
    /// </summary>
    public static class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Please specify all required parameters: client Id, certificate file, certificate file password, scope.");
                return;
            }

            // Your client identifier. Will be provided by Scalepoint
            var clientId = args[0];

            // Self-signed X.509 certificate (2048 bit RSA) with private key. Used to sign the assertion
            // Procedure of generating self-signed keys described in the documentation: https://dev.scalepoint.com/authentication#self-signed-certificate-generation
            var certificateFile = args[1];
            var certificatePassword = args[2];

            // In current example we are using direct load certificate from file
            // It also could be stored in certificates store,
            // in this case, to load certificate follow documentation: https://msdn.microsoft.com/en-us/library/system.security.cryptography.x509certificates.x509store(v=vs.110).aspx
            var certificate = new X509Certificate2(certificateFile, certificatePassword);

            // OAuth2 scope(s) to request access to. I.e. case_integration
            var scopeName = args[3];
            // In real life scopes could be more then one
            var scopes = new[] { scopeName };

            // Here it is test authentication endpoint. You could find more information by the link https://dev.scalepoint.com/authentication#endpoints
            var environment = "https://test-accounts.scalepoint.com/connect/token";
            var authenticationEndpointForTests = new Uri(environment);

            // This sample uses Scalepoint-baked .NET lib, which is client helper for OAuth2 Token endpoint: https://github.com/Scalepoint/oauth-token-net-client
            var tokenClient = new ClientCredentialsGrantTokenClient(
                authenticationEndpointForTests.AbsoluteUri,
                new JwtBearerClientAssertionCredentials(
                    authenticationEndpointForTests.AbsoluteUri,
                    clientId,
                    certificate
                )
            );

            var accessToken =  await tokenClient.GetTokenAsync(scopes).ConfigureAwait(false);

            Console.WriteLine(accessToken);
        }
    }
}
