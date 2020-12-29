using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPLinkAPI.Entity
{
    public class LoginRequest : CommonRequest
    {
        public LoginEntiry login;
    }

    public class LoginEntiry
    {
        public string password;
    }
}
