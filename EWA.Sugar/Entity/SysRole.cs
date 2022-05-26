using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 角色
    ///</summary>
    [SugarTable("sys_role")]
    public class SysRole
    {

        /// <summary>
        /// 页面码数
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int page { get; set; }


        /// <summary>
        /// 行数
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int limit { get; set; }


        /// <summary>
        /// 角色id 
        ///</summary>
        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true, IsIdentity = true)]
        public int roleId { get; set; }
        /// <summary>
        /// 角色名称 
        ///</summary>
        [SugarColumn(ColumnName = "role_name")]
        public string roleName { get; set; }
        /// <summary>
        /// 角色标识 
        ///</summary>
        [SugarColumn(ColumnName = "role_code")]
        public string roleCode { get; set; }
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


        [SugarColumn(IsIgnore = true)]
        public int? userId { get; set; }
    }
}
