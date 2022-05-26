using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 字典
    ///</summary>
    [SugarTable("sys_dictionary")]
    public class SysDictionary
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
        /// 字典id 
        ///</summary>
        [SugarColumn(ColumnName = "dict_id", IsPrimaryKey = true, IsIdentity = true)]
        public int dictId { get; set; }
        /// <summary>
        /// 字典标识 
        ///</summary>
        [SugarColumn(ColumnName = "dict_code")]
        public string dictCode { get; set; }
        /// <summary>
        /// 字典名称 
        ///</summary>
        [SugarColumn(ColumnName = "dict_name")]
        public string dictName { get; set; }
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
        /// 创建时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "create_time",IsOnlyIgnoreUpdate =true)]
        public DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "update_time", IsOnlyIgnoreInsert = true)]
        public DateTime updateTime { get; set; }
    }
}
