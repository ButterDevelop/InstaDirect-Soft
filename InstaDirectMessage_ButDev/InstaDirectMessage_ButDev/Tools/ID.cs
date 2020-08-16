using InstaDirectMessage_ButDev.Tools;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Management;

namespace InstaDirectMessage_ButDev.Tools
{
    public class ID
    {
        public static string IDNumber = Generate();
        public static string NewIDNumber = GenerateNew();
        public static string Generate()
        {
            RegistryKey reg = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System");
            string BIOS = B64X.Encrypt(Registry.GetValue(reg.ToString(), "SystemBiosDate", "NoExists").ToString());
            string WindowsKey = B64X.Encrypt(Tools.WindowsKey.GetWindowsProductKey().Substring(0, 20));
            return MD5MyHash.Encode(B64X.Decrypt(BIOS) + B64X.Decrypt(WindowsKey) + "InstAccChecker").Substring(0, 20).ToUpper();
        }

        public static string GenerateNew()
        {
            string proc = "", mother = "", uuid = "", os = "";
            ManagementObjectSearcher searcher;
            //процессор
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get()) proc = B64X.Encrypt(queryObj["ProcessorId"].ToString());
            //мать
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Card");
            foreach (ManagementObject queryObj in searcher.Get()) mother = B64X.Encrypt(queryObj["SerialNumber"].ToString());
            //UUID
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT UUID FROM Win32_ComputerSystemProduct");
            foreach (ManagementObject queryObj in searcher.Get()) uuid = B64X.Encrypt(queryObj["UUID"].ToString());

            //ОС
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get()) os = B64X.Encrypt(queryObj["SerialNumber"].ToString());

            return MD5MyHash.Encode(B64X.Decrypt(proc) + B64X.Decrypt(uuid) + "InstAccCheckerTipaNew?" + B64X.Decrypt(mother)).Substring(0, 20).ToUpper();
        }

    }
}
