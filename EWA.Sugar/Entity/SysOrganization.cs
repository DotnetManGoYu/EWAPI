using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 组织机构
    ///</summary>
    [SugarTable("sys_organization")]
    public class SysOrganization
    {
        /// <summary>
        /// 机构id 
        ///</summary>
        [SugarColumn(ColumnName = "organization_id", IsPrimaryKey = true, IsIdentity = true)]
        public int organizationId { get; set; }
        /// <summary>
        /// 上级id, 0是顶级 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "parent_id")]
        public int parentId { get; set; }
        /// <summary>
        /// 机构名称 
        ///</summary>
        [SugarColumn(ColumnName = "organization_name")]
        public string organizationName { get; set; }
        /// <summary>
        /// 机构全称 
        ///</summary>
        [SugarColumn(ColumnName = "organization_full_name")]
        public string organizationFullName { get; set; }
        /// <summary>
        /// 机构代码 
        ///</summary>
        [SugarColumn(ColumnName = "organization_code")]
        public string organizationCode { get; set; }
        /// <summary>
        /// 机构类型 
        ///</summary>
        [SugarColumn(ColumnName = "organization_type")]
        public string organizationType { get; set; }
        /// <summary>
        /// 负责人id 
        ///</summary>
        [SugarColumn(ColumnName = "leader_id")]
        public int? leaderId { get; set; }
        /// <summary>
        /// 排序号 
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "sort_number")]
        public int sortNumber { get; set; }
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
    }


    public class SysOrganizationParam
    {
        /// <summary>
        /// 页面码数
        /// </summary>
        public int page { get; set; }


        /// <summary>
        /// 行数
        /// </summary>
        public int limit { get; set; }

        public int? organizationId { get; set; }

    }
}
