using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PracticMauiApp.Helpers
{
    static class CertificateDownloader
    {
        // Accept any certificate, even if the certificate is invalid
        // We don't care about security here. The only goal is to get the certificate, not to transmit data.
        private static readonly RemoteCertificateValidationCallback s_certificateCallback = (_, _, _, _) => true;

        public static async Task<X509Certificate2> GetCertificateAsync(string domain, int port = 443)
        {
            using var client = new TcpClient(domain, port);
            using var sslStream = new SslStream(client.GetStream(), leaveInnerStreamOpen: true, s_certificateCallback);

            // Initiate the connection, so it will download the server certificate
            await sslStream.AuthenticateAsClientAsync(domain).ConfigureAwait(false);

            // Duplicate the certificate because "serverCertificate" won't be accessible
            // after disposing the stream, so not accessible outsite this method
            var serverCertificate = sslStream.RemoteCertificate;
            if (serverCertificate != null)
                return new X509Certificate2(serverCertificate);

            return null;
        }

        private static X509ChainElementCollection callbackChainElements = null;
        private static bool CertificateValidationCallback(
            Object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            callbackChainElements = chain.ChainElements;
            return true;
        }

        public static X509ChainElementCollection DownloadSslCertificate(string url)
        {
            X509Certificate2 cert = null;
            using TcpClient client = new();
            client.Connect(url, 443);

            SslStream ssl = new(client.GetStream(), false, CertificateValidationCallback, null);
            try
            {
                ssl.AuthenticateAsClient(url); //will call CertificateValidationCallback
            }
            catch (AuthenticationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ssl.Close();
                client.Close();
                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ssl.Close();
                client.Close();
                return null;
            }
            cert = new X509Certificate2(ssl.RemoteCertificate);
            ssl.Close();
            client.Close();
            return callbackChainElements;
        }

        /// <summary>
        /// Export a certificate to a PEM format string
        /// </summary>
        /// <param name="cert">The certificate to export</param>
        /// <returns>A PEM encoded string</returns>
        public static string ExportToPem(X509Certificate2 cert)
        {
            StringBuilder builder = new();

            try
            {
                builder.AppendLine("-----BEGIN CERTIFICATE-----");
                builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
                builder.AppendLine("-----END CERTIFICATE-----");

            }
            catch (Exception)
            {
            }

            return builder.ToString();
        }
    }
}
