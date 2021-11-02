using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace jwt
{
    public class info
    {
        public string req { get; set; }
        public string res { get; set; }
        public string code { get; set; }
    }
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class IPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public IPMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<IPMiddleware>();
        }

       

        public async Task InvokeAsync(HttpContext context)
        {
            var info = new info();

            _logger.LogInformation($"Client Ip:{context.Connection.RemoteIpAddress.ToString()}");
            // Call the next delegate/middleware in the pipeline
            context.Request.EnableBuffering();//启用倒带功能，就可以让 Request.Body 可以再次读取
            string body = "";
            using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024 * 1024,
                    leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
            }
            

             context.Request.Body.Position = 0;// 重置读取位置


            body = Regex.Unescape(body); //处理返回的字符比如unicode转为中文
            _logger.LogInformation(body);
            if (!body.Contains("sa")&&body!="")
            {
                await context.Response.WriteAsync("不是sa不能登入");
                return;
            }
            info.req = body;

            var res = "";
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
             
            

           
       
                responseBody.Seek(0, SeekOrigin.Begin);

                var responesInfo = await new StreamReader(responseBody).ReadToEndAsync();

                //设置当前流中的位置为起点
                responseBody.Seek(0, SeekOrigin.Begin);

                // 编码转换，解决中文乱码
                res = Regex.Unescape(responesInfo);

             

                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
                info.res = res;
                info.code = context.Response.StatusCode.ToString();
            }
            _logger.LogInformation(JsonConvert.SerializeObject(info));
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class IPMiddlewareExtensions
    {
        public static IApplicationBuilder UseIP(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPMiddleware>();
        }
    }
}