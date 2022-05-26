using EWA.Sugar;
using Furion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SqlSugar;
using System.Diagnostics;
using System.Security.Claims;

namespace EWADotnet.Authorize
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class OperLog : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">控制器名</param>
        /// <param name="description">方法名</param>
        /// <param name="param">是否记录请求参数</param>
        /// <param name="result">是否保存请求结果</param>
        public OperLog(string model, string description, bool param = false, bool result = false)
        {
            Model = model;
            Description = description;
            Param = param;
            Result = result;
        }
        string Model { get; set; }
        string Description { get; set; }
        bool Param { get; set; }
        bool Result { get; set; }

        private Stopwatch timer;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.timer = new Stopwatch();
            this.timer.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.timer.Stop();
            if (filterContext != null && !string.IsNullOrEmpty(Model) && !string.IsNullOrEmpty(Description))
            {
                var openrecord = new SysOperationRecord();
                var userid = App.User?.FindFirstValue("UserId");
                var tenantId = App.User?.FindFirstValue("tenantId");
                openrecord.userId = string.IsNullOrEmpty(userid) ? 0 : Convert.ToInt32(userid);
                openrecord.module = Model;
                openrecord.description = Description;
                openrecord.url = filterContext.HttpContext.Request.Host.Host + ":" + filterContext.HttpContext.Request.Host.Port?.ToString();
                openrecord.requestMethod = filterContext.HttpContext.Request.Method;
                if (Param)
                {
                    openrecord.@params = filterContext.HttpContext.Request?.QueryString.ToString();
                }
                if (Result)
                {
                    openrecord.result = GetActionExcutedResponseBody(filterContext); //返回结果
                }
                openrecord.spendTime = (int)this.timer.ElapsedMilliseconds;
                openrecord.createTime = DateTime.Now;
                openrecord.updateTime = DateTime.Now;
                openrecord.ip = NetHelper.Ip;
                openrecord.tenantId = string.IsNullOrEmpty(tenantId) ? 0 : Convert.ToInt32(tenantId);
                //保存到数据库
                App.GetService<ISqlSugarClient>().Insertable(openrecord).ExecuteCommand();
            }
            base.OnActionExecuted(filterContext);
        }
        private string GetActionExcutedResponseBody(ActionExecutedContext context)
        {
            string responseBody = "";
            if (context.Result != null)
            {
                if (context.Result is ObjectResult)
                {
                    responseBody = JsonConvert.SerializeObject(((ObjectResult)context.Result).Value);
                }
                if (context.Result is JsonResult)
                {
                    responseBody = JsonConvert.SerializeObject(((JsonResult)context.Result).Value);
                }
            }
            return responseBody;
        }
    }
}
