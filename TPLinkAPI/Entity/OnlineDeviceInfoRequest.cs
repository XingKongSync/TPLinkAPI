using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPLinkAPI.Entity
{
    public class OnlineDeviceInfoRequest : CommonRequest
    {
        public HostsInfoEntity hosts_info;

        public OnlineDeviceInfoRequest()
        {
            method = "get";
            hosts_info = new HostsInfoEntity();
        }
    }

    public class HostsInfoEntity
    {
        public string table = "online_host";
    }
}
