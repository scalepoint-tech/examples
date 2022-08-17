package com.scalepoint.examples;

import com.scalepoint.oauth_token_client.CertificateWithPrivateKey;
import com.scalepoint.oauth_token_client.ClientCredentialsGrantTokenClient;
import com.scalepoint.oauth_token_client.JwtBearerClientAssertionCredentials;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.security.*;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.security.interfaces.RSAPrivateKey;

public class Application {
    /**
     *
     * This example explains the principles behind Scalepoint Authentication scheme described at https://dev.scalepoint.com/authentication
     */
    public static void main(String[] args) throws Exception {

        if (args.length < 4) {
            System.err.println("Please specify all required parameters: client_id, certificate file, certificate file password, scope.");
            System.exit(1);
        }

        // Your client identifier provided by Scalepoint
        String clientId = args[0];

        // Java Key Store (JKS) with imported Self-signed X.509 certificate (2048 bit RSA) with private key. Used to sign the assertion
        // Procedure of generating self-signed keys is described in the documentation: https://dev.scalepoint.com/authentication#self-signed-certificate-generation
        // Please send us the certificate (but not the private key)
        String certificateFile = args[1];
        String certificatePassword = args[2];

        // OAuth2 scope(s) to request access to. I.e. case_integration
        String scopeName = args[3];

        CertificateWithPrivateKey keyPair = loadCertificate(certificateFile, certificatePassword);

        // There can be more than one scope, but we recommend requesting tokens separately with client_credentials,
        // each token carrying only the scopes required by the resource you want to access
        String[] scopes = { scopeName };

        // The endpoint hardcoded below is for test environment. More info: https://dev.scalepoint.com/authentication#endpoints
        String scalepointTokenUrl = "https://sandbox-accounts.scalepoint.com/connect/token";
        // If you are going to use a your own proxy please specify it here, otherwise it is the same as tokenUrl
        String companyProxyTokenUrl = "https://sandbox-accounts.scalepoint.com/connect/token";

        // This example uses Scalepoint OAuth2 client helper library: https://github.com/Scalepoint/oauth-token-java-client
        // Reuse the instance throughout the application lifecycle
        ClientCredentialsGrantTokenClient tokenClient = new ClientCredentialsGrantTokenClient(
                companyProxyTokenUrl,
                new JwtBearerClientAssertionCredentials(
                        scalepointTokenUrl,
                        clientId,
                        keyPair
                )
        );

        //You should call this immediately before calling the resource endpoint protected by the token
        // Do not store or reuse the value outside a single invocation yourself. The library already implements caching, so instead store and reuse the thread-safe "tokenClient" instance as mentioned above
        // This token is confidential. Only send it to the resource server
        final String token = tokenClient.getToken(scopes);

        System.out.println("accessToken = " + token);

    }


    private static CertificateWithPrivateKey loadCertificate(String certificateFile, String keyStorePassword) throws KeyStoreException, IOException, UnrecoverableKeyException, NoSuchAlgorithmException, CertificateException {
        // Loading the certificate from java key store (JKS)
        KeyStore keyStore =  KeyStore.getInstance(KeyStore.getDefaultType());
        RSAPrivateKey privateKey;
        X509Certificate certificate;
        File keyStoreFile = new File(certificateFile);
        try (InputStream stream = new FileInputStream(keyStoreFile)) {
            keyStore.load(stream, null);
        }
        String alias = keyStore.aliases().nextElement();
        privateKey = (RSAPrivateKey) keyStore.getKey(alias, keyStorePassword.toCharArray());
        certificate = (X509Certificate) keyStore.getCertificate(alias);
        return new CertificateWithPrivateKey(privateKey, certificate);
    }
}
