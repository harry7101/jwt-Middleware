using IService;
using System;

namespace Service
{
    public class ServiceTestA : IServcieTestA
    {
        public ServiceTestA() {
            Console.WriteLine("ServiceTestA构造函数");
        }
        public string show(string msg)
        {
            return msg;
        }
    }
}
