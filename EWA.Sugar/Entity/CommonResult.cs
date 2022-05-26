using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EWA.Sugar
{
    public enum ResultCode
    {
        Success = 0,
        Error = 1
    }

    public static class Result
    {
        /// <summary>
        /// 判断返回
        /// </summary>
        /// <param name="success"></param>
        /// <param name="_msg"></param>
        /// <returns></returns>
        public static CommonResult Judge(bool success, string _msg = "")
        {
            return success ? new CommonResult(ResultCode.Success, null, _msg) : new CommonResult(ResultCode.Error, null, _msg);
        }
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_msg"></param>
        /// <returns></returns>
        public static CommonResult Success(object _data = null, string _msg = "")
        {
            return new CommonResult(ResultCode.Success, _data, _msg);
        }
        /// <summary>
        /// 失败返回
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_msg"></param>
        /// <returns></returns>
        public static CommonResult Error(string _msg = "", object _data = null)
        {
            return new CommonResult(ResultCode.Error, _data, _msg);
        }

        public static CommonPageResult PageSuccess(object _data, int _count = 0, string _msg = "")
        {
            return new CommonPageResult(ResultCode.Success, _data, _msg, _count);
        }
        public static CommonPageResult PageError(string _msg = "", object _data = null)
        {
            return new CommonPageResult(ResultCode.Error, _data, _msg, 0);
        }
    }

    public class CommonResult
    {
        public CommonResult(ResultCode _code, object _data = null, string _msg = "")
        {
            message = !string.IsNullOrEmpty(_msg) ? _msg : _code == ResultCode.Success ? "操作成功" : "操作失败";
            code = (int)_code;
            data = _data;
        }
        /// <summary>
        /// 1为正确，0为错误
        /// </summary>
        public int code { get; set; }

        public string message { get; set; }

        public object data { get; set; }
    }

    public class CommonPageResult
    {
        public CommonPageResult(ResultCode _code, object _list = null, string _msg = "", int _count = 0)
        {
            message = !string.IsNullOrEmpty(_msg) ? _msg : _code == ResultCode.Success ? "操作成功" : "操作失败";
            code = (int)_code;
            data = new PageResultData();
            data.list = _list;
            data.count = _count;
            if (_count == 0 && data != null)
            {
                data.count = JArray.Parse(JsonConvert.SerializeObject(_list)).Count;
            }
        }
        /// <summary>
        /// 1为正确，0为错误
        /// </summary>
        public int code { get; set; }

        public string message { get; set; }

        public PageResultData data { get; set; }
    }

    public class PageResultData
    {
        public object list { get; set; }

        public int count { get; set; }
    }

}
