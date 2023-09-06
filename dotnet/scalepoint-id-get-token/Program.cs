using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Scalepoint.OAuth.TokenClient;

namespace ScalepointIdGetToken
{
    /// <summary>
    /// This example explains the principles behind Scalepoint Authentication scheme described at https://dev.scalepoint.com/authentication
    /// run from commandline:
    /// dotnet run "future_insurance" "ClientCertificate.pfx" "pfxPassword" "case_integration"
    /// </summary>
    public static class Program
    {
        static async Task<int> Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.Error.WriteLine("Please specify all required parameters: client_id, certificate file, certificate file password, scope.");
                return 1;
            }

            // Your client identifier provided by Scalepoint
            var clientId = args[0];

            // Self-signed X.509 certificate (2048 bit RSA) with private key. Used to sign the assertion
            // Procedure of generating self-signed keys is described in the documentation: https://dev.scalepoint.com/authentication#self-signed-certificate-generation
            // Please send us the certificate (but not the private key)
            var certificateFile = args[1];
            var certificatePassword = args[2];

            // OAuth2 scope(s) to request access to. I.e. case_integration
            var scopeName = args[3];

            // DISCLAIMER: In current example we are directly loading the certificate from file for simplicity
            // At least on Windows, loading it from Windows certificate store is probably a better choice for a real application
            // We recommend to use "LocalMachine/My" store and grant read access to the private key to the windows service account used to run the client side code
            // In this case, follow this documentation: https://msdn.microsoft.com/en-us/library/system.security.cryptography.x509certificates.x509store(v=vs.110).aspx
            // You can reliably lookup the certificate by thumbprint
            var certificate = new X509Certificate2(certificateFile, certificatePassword);

            // There can be more than one scope, but we recommend requesting tokens separately with client_credentials,
            // each token carrying only the scopes required by the resource you want to access
            var scopes = new[] { scopeName };

            // The endpoint hardcoded below is for test environment. More info: https://dev.scalepoint.com/authentication#endpoints
            var environment = "https://sandbox-accounts.scalepoint.com/connect/token";
            var authenticationEndpointForTests = new Uri(environment);

            // This example uses Scalepoint OAuth2 client helper library: https://github.com/Scalepoint/oauth-token-net-client
            // Reuse the instance throughout the application lifecycle
            var tokenClient = new ClientCredentialsGrantTokenClient(
                authenticationEndpointForTests.AbsoluteUri,
                new JwtBearerClientAssertionCredentials(
                    authenticationEndpointForTests.AbsoluteUri,
                    clientId,
                    certificate
                )
            );

            // You should call this _immediately_ before calling the resource endpoint protected by the token
            // Do not store or reuse the value outside a single invocation yourself. The library already implements caching, so instead store and reuse the thread-safe "tokenClient" instance as mentioned above
            // This token is confidential. Only send it to the resource server
            var accessToken = await tokenClient.GetTokenAsync(scopes).ConfigureAwait(false);

            Console.WriteLine(accessToken);
            return 0;
        }
    }
}
