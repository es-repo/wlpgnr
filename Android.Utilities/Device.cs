using System.Text;
using Android.OS;

namespace Android.Utilities
{
    public static class Device
    {
        public static string Info
        {
            get
            {
                StringBuilder info = new StringBuilder();
                info.AppendFormat("Brand: {0}\n", Build.Brand);
                info.AppendFormat("Device: {0}\n", Build.Device);
                info.AppendFormat("Model: {0}\n", Build.Model);
                info.AppendFormat("Id: {0}\n", Build.Id);
                info.AppendFormat("Product: {0}\n", Build.Product);
                info.AppendFormat("CpuAbi: {0}\n", Build.CpuAbi);
                info.AppendFormat("CpuAbi2: {0}\n", Build.CpuAbi2);
                info.AppendFormat("SDK: {0}\n", Build.VERSION.Sdk);
                info.AppendFormat("Release: {0}\n", Build.VERSION.Release);
                info.AppendFormat("Incremental: {0}\n", Build.VERSION.Incremental);
                return info.ToString();
            }
        }
    }
}
