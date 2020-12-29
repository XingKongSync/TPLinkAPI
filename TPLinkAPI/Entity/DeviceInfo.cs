using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPLinkAPI.Entity
{
    public class DeviceInfo
    {
        public string mac;
        public string ip;
        public string hostname;
        public string up_speed;
        public string down_speed;

        public int online;

        /// <summary>
        /// 解析匿名json对象HostInfo
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<DeviceInfo> AnalysHostInfoJson(string json)
        {
            List<DeviceInfo> hostsInfo = new List<DeviceInfo>();
            string jsonArrayStr = GetJsonArray(json);

            var jsonArray = JArray.Parse(jsonArrayStr);
            foreach (var jtoken in jsonArray)
            {
                JToken jobj = jtoken.First.First;
                hostsInfo.Add(new DeviceInfo
                {
                    hostname = System.Web.HttpUtility.UrlDecode(jobj["hostname"].ToString()),
                    ip = jobj["ip"].ToString(),
                    down_speed = jobj["down_speed"].ToString(),
                    mac = jobj["mac"].ToString(),
                    up_speed = jobj["up_speed"].ToString(),

                    online = 1
                });
            }

            return hostsInfo;
        }
        /// <summary>
        /// 截取json数组
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string GetJsonArray(string json)
        {
            string startStr = "[";
            string endStr = "]";
            int start = json.IndexOf(startStr);
            int end;
            if (start != -1)
            {
                end = json.LastIndexOf(endStr);
                if (end != -1)
                {
                    string result = json.Substring(start, end - start + 1);
                    return result;
                }
            }
            return string.Empty;
        }

    }

    public class DeviceInfoEx
    {
        /// <summary>
        /// 设备的连线、断线状态变化通知事件
        /// </summary>
        public event Action<DeviceInfoEx> DeviceOnlineStatusChanged;

        private bool _online = false;
        public bool Online 
        {
            get => _online;
            private set 
            {
                if (value != _online)
                {
                    _online = value;
                    if (!_online)
                    {
                        LastOfflineTime = DateTime.Now;
                    }
                    DeviceOnlineStatusChanged?.Invoke(this);
                }
            }
        }

        public DateTime LastOfflineTime { get; private set; }

        public DeviceInfo DevInfo { get; private set; }

        public int Tag;

        public void Update(DeviceInfo deviceInfo, bool online)
        {
            if (deviceInfo != null)
            {
                DevInfo = deviceInfo;
            }
            if (DevInfo != null)
                DevInfo.online = online ? 1 : 0;
            Online = online;
        }

    }

}
