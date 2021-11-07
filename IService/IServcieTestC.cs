using System;

namespace IService
{
    public interface IServcieTestC
    {
        public IServcieTestA servcieTestA { get; set; }
        public IServcieTestB servcieTestB { get; set; }

        public string show(string msg);
    }
}
