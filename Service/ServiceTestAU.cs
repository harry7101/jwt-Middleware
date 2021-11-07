using IService;
using System;

namespace Service
{
    public class ServiceTestAU : IServcieTestA
    {
        public ServiceTestAU() {
            Console.WriteLine("ServiceTestAU构造函数");
        }
        public string show(string msg)
        {
            return msg;
        }
    }
}
