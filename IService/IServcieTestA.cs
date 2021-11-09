using System;
using Common;
using Autofac.Extras.DynamicProxy;
namespace IService
{
    [Intercept(typeof(CustomerAutofacAop))]
    public interface IServcieTestA
    {

        public string show(string msg);
    }
}
