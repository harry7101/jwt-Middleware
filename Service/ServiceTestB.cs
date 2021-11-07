using IService;
using System;

namespace Service
{
    public class ServiceTestB : IServcieTestB
    {
        public readonly IServcieTestA servcieTestA1;
        public  ServiceTestB(IServcieTestA servcieTestA) {
            Console.WriteLine("ServiceTestB构造函数");
            servcieTestA1 = servcieTestA;
        }
        public string show(string msg)
        {
            return "ServiceTestB.show";
        }

        public string showServiceA(string msg)
        {
            return servcieTestA1.show(msg);
        }
    }
}
