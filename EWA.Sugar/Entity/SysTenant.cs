using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 租户
    ///</summary>
    [SugarTable("sys_tenant")]
    public class SysTenant
    {
        /// <summary>
        /// 租户id 
        ///</summary>
        [SugarColumn(ColumnName = "tenant_id", IsPrimaryKey = true, IsIdentity = true)]
        public int tenantId { get; set; }
        /// <summary>
        /// 租户名称 
        ///</summary>
        [SugarColumn(ColumnName = "tenant_name")]
        public string tenantName { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "comments")]
        public string comments { get; set; }
        /// <summary>
        /// 是否删除, 0否, 1是 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "deleted")]
        public int deleted { get; set; }
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
