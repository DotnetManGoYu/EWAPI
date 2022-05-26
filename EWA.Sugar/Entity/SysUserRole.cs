using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 用户角色
    ///</summary>
    [SugarTable("sys_user_role")]
    public class SysUserRole
    {
        /// <summary>
        /// 主键id 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        /// <summary>
        /// 用户id 
        ///</summary>
        [SugarColumn(ColumnName = "user_id")]
        public int userId { get; set; }
        /// <summary>
        /// 角色id 
        ///</summary>
        [SugarColumn(ColumnName = "role_id")]
        public int roleId { get; set; }
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
