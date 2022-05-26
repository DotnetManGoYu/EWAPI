using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 登录日志
    ///</summary>
    [SugarTable("sys_login_record")]
    public class SysLoginRecord
    {

        [SugarColumn(IsIgnore =true)]
        public int userId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string nickname { get; set; }


        /// <summary>
        /// 主键 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        /// <summary>
        /// 用户账号 
        ///</summary>
        [SugarColumn(ColumnName = "username")]
        public string username { get; set; }
        /// <summary>
        /// 操作系统 
        ///</summary>
        [SugarColumn(ColumnName = "os")]
        public string os { get; set; }
        /// <summary>
        /// 设备名 
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
        /// 操作类型, 0登录成功, 1登录失败, 2退出登录, 3续签token 
        ///</summary>
        [SugarColumn(ColumnName = "login_type")]
        public int loginType { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "comments")]
        public string comments { get; set; }
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


    public class SysLoginRecordParam
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

        public string nickname { get; set; }

        public DateTime? createTimeStart { get; set; }

        public DateTime? createTimeEnd { get; set; }


    }
}
