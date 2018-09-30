using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Azure;

namespace Apprenticeship.AzureFunction.Scheduler
{
    public static class ArmSfaScheduler
    {
        public static string PublishSettingFilePath { get; set; }
        public static string SubscriptionName { get; set; }
        public static CertificateCloudCredentials FromPublishSettingsFile(string path, string subscriptionName)
        {
            var profile = XDocument.Load(path);
            var subscriptionId = profile.Descendants("Subscription")
                .First(element => element.Attribute("Name").Value == subscriptionName)
                .Attribute("Id").Value;
            var certificate = new X509Certificate2(
                Convert.FromBase64String
                (
                    profile.Descendants("PublishProfile").Descendants("Subscription").FirstOrDefault().Attribute("ManagementCertificate").Value));
            return new CertificateCloudCredentials(subscriptionId, certificate);
        }

        public static void SetPublishSettingsProperties()
        {
            PublishSettingFilePath =
                @"C:\\SFA_Beta\\Experiments\\DSADemo\\DSA.Demo\\Visual Studio Enterprise with MSDN-Visual Studio Enterprise-9-4-2017-credentials.publishsettings";//@"D:\\azdem.publishsettings";
            SubscriptionName = "Visual Studio Enterprise with MSDN";// "Azdem194D92901Y";

        }
      
    }
}
