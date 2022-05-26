using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 字典项
    ///</summary>
    [SugarTable("sys_dictionary_data")]
    public class SysDictionaryData
    {

        /// <summary>
        /// 字典id 
        ///</summary>
        [SugarColumn(ColumnName = "dict_id")]
        public int dictId { get; set; }

        /// <summary>
        /// 字典项id 
        ///</summary>
        [SugarColumn(ColumnName = "dict_data_id", IsPrimaryKey = true, IsIdentity = true)]
        public int dictDataId { get; set; }

        /// <summary>
        /// 字典项标识 
        ///</summary>
        [SugarColumn(ColumnName = "dict_data_code")]
        public string dictDataCode { get; set; }
        /// <summary>
        /// 字典项名称 
        ///</summary>
        [SugarColumn(ColumnName = "dict_data_name")]
        public string dictDataName { get; set; }
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
        [SugarColumn(ColumnName = "create_time", IsOnlyIgnoreUpdate = true)]
        public DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "update_time", IsOnlyIgnoreInsert = true)]
        public DateTime updateTime { get; set; }
    }


    public class SysDictionaryDataParam
    {
        public int page { get; set; }

        public int limit { get; set; }

        public int? dictId { get; set; }

        public string keywords { get; set; }
    }
}
