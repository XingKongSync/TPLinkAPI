using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPLinkAPI.Entity;

namespace TPLinkAPI
{
    public class TPLinkService
    {
        private string _password;
        private string _host;

        /// <summary>
        /// 发送速度（字节/秒）
        /// </summary>
        public long UploadSpeed { get; private set; }

        /// <summary>
        /// 接收速度（字节/秒）
        /// </summary>
        public long DownloadSpeed { get; private set; }

        public Dictionary<string, DeviceInfoEx> DeviceMap { get; private set; } = new Dictionary<string, DeviceInfoEx>();

        /// <summary>
        /// 设备的连线、断线状态变化通知事件
        /// </summary>
        public event Action<DeviceInfoEx> DeviceOnlineStatusChanged;

        public TPLinkService(string host, string pwd)
        {
            _host = host;
            _password = pwd;

            Login();
            InitDeviceMap();
        }

        private void Login()
        {
            TPLinkApi.Host = _host;
            LoginResponse loginResult = TPLinkApi.Login(_password);
            if (loginResult.error_code != 0)
            {
                throw new Exception("登陆失败");
            }
            TPLinkApi.Token = loginResult.stok;
        }

        private void InitDeviceMap()
        {
            List<DeviceInfo> devices = TPLinkApi.GetDeviceStatus();
            foreach (DeviceInfo device in devices)
            {
                DeviceInfoEx devEx = new DeviceInfoEx();
                devEx.Update(device, true);
                DeviceMap[device.mac] = devEx;
                devEx.DeviceOnlineStatusChanged += DevEx_DeviceOnlineStatusChanged;
            }
        }

        public void Relogin() => Login();

        /// <summary>
        /// 心跳函数，依赖外部触发
        /// 触发后自动更新设备信息，并且计算网速
        /// </summary>
        public void DoHeartBeat()
        {
            foreach (DeviceInfoEx devEx in DeviceMap.Values)
            {
                devEx.Tag = 0;
            }

            //更新每个设备的在线状态
            List<DeviceInfo> devices = TPLinkApi.GetDeviceStatus();
            foreach (DeviceInfo device in devices)
            {
                if (DeviceMap.TryGetValue(device.mac, out DeviceInfoEx devEx))
                {
                    devEx.Update(device, true);
                }
                else
                {
                    devEx = new DeviceInfoEx();
                    devEx.Update(device, true);
                    DeviceMap[device.mac] = devEx;
                    devEx.DeviceOnlineStatusChanged += DevEx_DeviceOnlineStatusChanged;
                }
                devEx.Tag = 1;
            }

            foreach (DeviceInfoEx devEx in DeviceMap.Values)
            {
                if (devEx.Tag == 0)
                {
                    devEx.Update(null, false);
                }
            }

            var speed = TPLinkApi.GetDeviceNetSpeed();
            UploadSpeed = speed.network.wan_status.up_speed;
            DownloadSpeed = speed.network.wan_status.down_speed;
        }

        private void DevEx_DeviceOnlineStatusChanged(DeviceInfoEx obj)
        {
            DeviceOnlineStatusChanged?.Invoke(obj);
        }
    }
}
