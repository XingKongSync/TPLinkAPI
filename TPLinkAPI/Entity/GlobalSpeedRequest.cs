using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPLinkAPI.Entity
{
    public class GlobalSpeedRequest : CommonRequest
    {
        public Network network = new Network();

        public GlobalSpeedRequest()
        {
            method = "get";
        }
    }

    public class Network
    {
        public List<string> name = new List<string> { "wan_status" };
    }

}
