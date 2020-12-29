using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPLinkAPI.Entity;

namespace TPLinkAPI
{
    public static class TPLinkApi
    {
        private static string LOGIN_URL = "http://192.168.1.1/";
        private static string ONLINE_DEVICEINFO_URL = "http://192.168.1.1/stok={0}/ds";

        private static string host = "192.168.1.1";
        private static string token = string.Empty;

        public static string Host { get => host; set { host = value; LOGIN_URL = $"http://{value}/"; } }
        public static string Token { get => token; set { token = value; ONLINE_DEVICEINFO_URL = $"http://{Host}/stok={value}/ds"; } }

        private static OnlineDeviceInfoRequest _onlineDeviceInfoRequest = new OnlineDeviceInfoRequest();
        private static GlobalSpeedRequest _globalSpeedRequest = new GlobalSpeedRequest();

        public static LoginResponse Login(string password)
        {
            LoginRequest request = new LoginRequest();
            request.login = new LoginEntiry() { password = password };

            string responseStr = HttpUtils.Post(LOGIN_URL, request);
            return JsonConvert.DeserializeObject<LoginResponse>(responseStr);
        }

        public static List<DeviceInfo> GetDeviceStatus()
        {
            string responseStr = HttpUtils.Post(ONLINE_DEVICEINFO_URL, _onlineDeviceInfoRequest);
            List<DeviceInfo> devices = DeviceInfo.AnalysHostInfoJson(responseStr);
            foreach (var device in devices)
            {
                device.mac = device.mac?.ToUpper()?.Replace("-", ":");
            }
            return devices;
        }

        public static WanStatusReponse GetDeviceNetSpeed()
        {
            string responseStr = HttpUtils.Post(ONLINE_DEVICEINFO_URL, _globalSpeedRequest);
            return JsonConvert.DeserializeObject<WanStatusReponse>(responseStr);
        }
    }
}
