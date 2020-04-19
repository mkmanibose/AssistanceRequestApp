namespace AssistanceRequestApp.Common
{
    using System;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="ClientIPAddressDetails" />.
    /// </summary>
    public class ClientIPAddressDetails
    {
        /// <summary>
        /// The GetClientIP.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetClientIP()
        {
            string ipAddress = string.Empty;
            try
            {
                IPHostEntry Host = default(IPHostEntry);
                string Hostname = null;
                Hostname = System.Environment.MachineName;
                Host = Dns.GetHostEntry(Hostname);
                foreach (IPAddress IP in Host.AddressList)
                {
                    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = Convert.ToString(IP);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Length > 199)
                {
                    ipAddress = ex.Message.Substring(0, 199);
                }
                else
                {
                    ipAddress = ex.Message;
                }
            }
            return ipAddress;
        }
    }
}
