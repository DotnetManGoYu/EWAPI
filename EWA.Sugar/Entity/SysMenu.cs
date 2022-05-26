using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 菜单
    ///</summary>
    [SugarTable("sys_menu")]
    public class SysMenu
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

        [SugarColumn(IsIgnore = true)]
        public bool @checked { get; set; }


        /// <summary>
        /// 菜单id 
        ///</summary>
        [SugarColumn(ColumnName = "menu_id", IsPrimaryKey = true, IsIdentity = true)]
        public int menuId { get; set; }
        /// <summary>
        /// 上级id, 0是顶级 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "parent_id")]
        public int parentId { get; set; }
        /// <summary>
        /// 菜单名称 
        ///</summary>
        [SugarColumn(ColumnName = "title")]
        public string title { get; set; }
        /// <summary>
        /// 菜单路由地址 
        ///</summary>
        [SugarColumn(ColumnName = "path")]
        public string path { get; set; }
        /// <summary>
        /// 菜单组件地址, 目录可为空 
        ///</summary>
        [SugarColumn(ColumnName = "component")]
        public string component { get; set; }
        /// <summary>
        /// 类型, 0菜单, 1按钮 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "menu_type")]
        public int? menuType { get; set; }
        /// <summary>
        /// 排序号 
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "sort_number")]
        public int sortNumber { get; set; }
        /// <summary>
        /// 权限标识 
        ///</summary>
        [SugarColumn(ColumnName = "authority")]
        public string authority { get; set; }
        /// <summary>
        /// 打开位置 
        /// 默认值: _self
        ///</summary>
        [SugarColumn(ColumnName = "target")]
        public string target { get; set; }
        /// <summary>
        /// 菜单图标 
        ///</summary>
        [SugarColumn(ColumnName = "icon")]
        public string icon { get; set; }
        /// <summary>
        /// 图标颜色 
        ///</summary>
        [SugarColumn(ColumnName = "color")]
        public string color { get; set; }
        /// <summary>
        /// 是否隐藏, 0否, 1是(仅注册路由不显示在左侧菜单) 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "hide")]
        public int hide { get; set; }
        /// <summary>
        /// 菜单侧栏选中的path 
        ///</summary>
        [SugarColumn(ColumnName = "active")]
        public string active { get; set; }
        /// <summary>
        /// 其它路由元信息 
        ///</summary>
        [SugarColumn(ColumnName = "meta")]
        public string meta { get; set; }
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


        /// <summary>
        /// 子菜单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysMenu> children { get; set; }
    }
}
