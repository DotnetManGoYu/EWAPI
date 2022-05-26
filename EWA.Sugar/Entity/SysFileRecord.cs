using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 文件列表
    ///</summary>
    [SugarTable("sys_file_record")]
    public class SysFileRecord
    {
        /// <summary>
        /// 主键id 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        /// <summary>
        /// 文件名称 
        ///</summary>
        [SugarColumn(ColumnName = "name")]
        public string name { get; set; }
        /// <summary>
        /// 文件存储路径 
        ///</summary>
        [SugarColumn(ColumnName = "path")]
        public string path { get; set; }
        /// <summary>
        /// 文件大小 
        ///</summary>
        [SugarColumn(ColumnName = "length")]
        public int? length { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "comments")]
        public string comments { get; set; }
        /// <summary>
        /// 创建人 
        ///</summary>
        [SugarColumn(ColumnName = "create_user_id")]
        public int? createUserId { get; set; }
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
