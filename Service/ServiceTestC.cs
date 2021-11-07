using IService;
using System;

namespace Service
{
    public class ServiceTestC : IServcieTestC
    {
        public IServcieTestA servcieTestA { get; set; }
        public IServcieTestB servcieTestB{ get; set; }
        public ServiceTestC() {
            Console.WriteLine("ServiceTestC构造函数");
        }
        public string show(string msg)
        {
            return msg;
        }
    }
}
