using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace osu_spoofer
{
    class Spoof
    {

        public Spoof()
        {

        }

        // OSU
        public void spoofUninstallID(string generatedUID)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\osu!", true))
            {
                if (key != null)
                {
                    string newUninstallID = generatedUID;
                    key.SetValue("UninstallID", newUninstallID);
                    key.Dispose();
                }
            }
        }

        public string getUninstallID()
        {
            string ID = "";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\osu!", true))
            {
                if (key != null)
                {
                    key.GetValue("UninstallID");
                    ID = Convert.ToString(key.GetValue("UninstallID"));
                    key.Dispose();
                }
            }

            return ID;
        }

        // RANDOM ID
        public static string RandomId(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string result = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result += chars[random.Next(chars.Length)];
            }

            return result;
        }


        // HARDWARE

        // DISKS
        public void SpoofDisks()
        {
            using RegistryKey ScsiPorts = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\Scsi");
            foreach (string port in ScsiPorts.GetSubKeyNames())
            {
                using RegistryKey ScsiBuses = Registry.LocalMachine.OpenSubKey($"HARDWARE\\DEVICEMAP\\Scsi\\{port}");
                foreach (string bus in ScsiBuses.GetSubKeyNames())
                {
                    using RegistryKey ScsuiBus = Registry.LocalMachine.OpenSubKey($"HARDWARE\\DEVICEMAP\\Scsi\\{port}\\{bus}\\Target Id 0\\Logical Unit Id 0", true);
                    if (ScsuiBus != null)
                    {
                        if (ScsuiBus.GetValue("DeviceType").ToString() == "DiskPeripheral")
                        {
                            string identifier = RandomId(14);
                            string serialNumber = RandomId(14);

                            ScsuiBus.SetValue("DeviceIdentifierPage", Encoding.UTF8.GetBytes(serialNumber));
                            ScsuiBus.SetValue("Identifier", identifier);
                            ScsuiBus.SetValue("InquiryData", Encoding.UTF8.GetBytes(identifier));
                            ScsuiBus.SetValue("SerialNumber", serialNumber);
                        }
                    }
                }
            }

            using RegistryKey DiskPeripherals = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\MultifunctionAdapter\\0\\DiskController\\0\\DiskPeripheral");
            foreach (string disk in DiskPeripherals.GetSubKeyNames())
            {
                using RegistryKey DiskPeripheral = Registry.LocalMachine.OpenSubKey($"HARDWARE\\DESCRIPTION\\System\\MultifunctionAdapter\\0\\DiskController\\0\\DiskPeripheral\\{disk}", true);
                DiskPeripheral.SetValue("Identifier", $"{RandomId(8)}-{RandomId(8)}-A");
            }
        }

        // Spoof HwProfileGuid, MachineGuid, MachineId
        public void SpoofHwGUID()
        {
            using RegistryKey HardwareGUID = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\Hardware Profiles\\0001", true);
            HardwareGUID.SetValue("HwProfileGuid", $"{{{Guid.NewGuid()}}}");
        }

        public void SpoofMachineGUID()
        {
            using RegistryKey MachineGUID = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography", true);
            MachineGUID.SetValue("MachineGuid", Guid.NewGuid().ToString());
        }

        public void SpoofMachineID()
        {
            using RegistryKey MachineId = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\SQMClient", true);
            MachineId.SetValue("MachineId", $"{{{Guid.NewGuid()}}}");
        }


        // SYS INFO / Not Recommended, nor included in menu for now


    }
}
