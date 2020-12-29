using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPLinkAPI.Entity
{
    public class WanStatusReponse : CommonResponse
    {
        public NetworkStatusEntity network;
    }

    public class NetworkStatusEntity
    {
        public WanStatusEntity wan_status;
    }

    public class WanStatusEntity
    {
        public int down_speed;
        public int up_speed;
    }
}
