using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPLinkAPI;
using TPLinkAPI.Entity;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            LoginResponse response = TPLinkApi.Login("W8MRvPNw9TefbwK");

        }

        [TestMethod]
        public void TestGetDeviceInfo()
        {
            string token = "n)%7BoftEd%24F%7D%3E.D%3CsxxXtWmIsBX)heMsW";
            TPLinkApi.Token = token;
            List<DeviceInfo> response = TPLinkApi.GetDeviceStatus();
        }
    }
}
