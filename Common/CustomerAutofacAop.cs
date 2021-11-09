using System;
using Castle.DynamicProxy;
namespace Common
{
    public class CustomerAutofacAop : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"aop开始之前");
            invocation.Proceed();
            Console.WriteLine($"aop开始之后");
        }
    }
}
