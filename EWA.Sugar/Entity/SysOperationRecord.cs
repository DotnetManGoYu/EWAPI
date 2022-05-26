using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 操作日志
    ///</summary>
    [SugarTable("sys_operation_record")]
    public class SysOperationRecord
    {
        [SugarColumn(IsIgnore = true)]
        public string username { get; set; }


        [SugarColumn(IsIgnore = true)]
        public string nickname { get; set; }
        /// <summary>
        /// 主键 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        /// <summary>
        /// 用户id 
        ///</summary>
        [SugarColumn(ColumnName = "user_id")]
        public int? userId { get; set; }
        /// <summary>
        /// 操作模块 
        ///</summary>
        [SugarColumn(ColumnName = "module")]
        public string module { get; set; }
        /// <summary>
        /// 操作功能 
        ///</summary>
        [SugarColumn(ColumnName = "description")]
        public string description { get; set; }
        /// <summary>
        /// 请求地址 
        ///</summary>
        [SugarColumn(ColumnName = "url")]
        public string url { get; set; }
        /// <summary>
        /// 请求方式 
        ///</summary>
        [SugarColumn(ColumnName = "request_method")]
        public string requestMethod { get; set; }
        /// <summary>
        /// 调用方法 
        ///</summary>
        [SugarColumn(ColumnName = "method")]
        public string method { get; set; }
        /// <summary>
        /// 请求参数 
        ///</summary>
        [SugarColumn(ColumnName = "params")]
        public string @params { get; set; }
        /// <summary>
        /// 返回结果 
        ///</summary>
        [SugarColumn(ColumnName = "result")]
        public string result { get; set; }
        /// <summary>
        /// 异常信息 
        ///</summary>
        [SugarColumn(ColumnName = "error")]
        public string error { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "comments")]
        public string comments { get; set; }
        /// <summary>
        /// 消耗时间, 单位毫秒 
        ///</summary>
        [SugarColumn(ColumnName = "spend_time")]
        public int? spendTime { get; set; }
        /// <summary>
        /// 操作系统 
        ///</summary>
        [SugarColumn(ColumnName = "os")]
        public string os { get; set; }
        /// <summary>
        /// 设备名称 
        ///</summary>
        [SugarColumn(ColumnName = "device")]
        public string device { get; set; }
        /// <summary>
        /// 浏览器类型 
        ///</summary>
        [SugarColumn(ColumnName = "browser")]
        public string browser { get; set; }
        /// <summary>
        /// ip地址 
        ///</summary>
        [SugarColumn(ColumnName = "ip")]
        public string ip { get; set; }
        /// <summary>
        /// 状态, 0成功, 1异常 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public int status { get; set; }
        /// <summary>
        /// 租户id 
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "tenant_id")]
        public int tenantId { get; set; }
        /// <summary>
        /// 操作时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime updateTime { get; set; }
    }


    public class SysOperationRecordParam
    {
        /// <summary>
        /// 页面码数
        /// </summary>

        public int? page { get; set; }


        /// <summary>
        /// 行数
        /// </summary>
        public int? limit { get; set; }


        public string username { get; set; }

        public string module { get; set; }

        public DateTime? createTimeStart { get; set; }

        public DateTime? createTimeEnd { get; set; }
    }
}
