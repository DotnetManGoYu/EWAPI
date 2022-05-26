using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 邮件记录
    ///</summary>
    [SugarTable("sys_email_record")]
    public class SysEmailRecord
    {
        /// <summary>
        /// 主键id 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        /// <summary>
        /// 邮件标题 
        ///</summary>
        [SugarColumn(ColumnName = "title")]
        public string title { get; set; }
        /// <summary>
        /// 邮件内容 
        ///</summary>
        [SugarColumn(ColumnName = "content")]
        public string content { get; set; }
        /// <summary>
        /// 收件邮箱 
        ///</summary>
        [SugarColumn(ColumnName = "receiver")]
        public string receiver { get; set; }
        /// <summary>
        /// 发件邮箱 
        ///</summary>
        [SugarColumn(ColumnName = "sender")]
        public string sender { get; set; }
        /// <summary>
        /// 创建人 
        ///</summary>
        [SugarColumn(ColumnName = "create_user_id")]
        public int? createUserId { get; set; }
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
        /// 创建时间 
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
}
