using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SqlSugar;
namespace EWA.Sugar
{
    /// <summary>
    /// 用户
    ///</summary>
    [SugarTable("sys_user")]
    public class SysUser
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
        /// 用户id 
        ///</summary>
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true, IsIdentity = true)]
        public int userId { get; set; }
        /// <summary>
        /// 账号 
        ///</summary>
        [SugarColumn(ColumnName = "username")]
        public string username { get; set; }
        /// <summary>
        /// 密码 
        ///</summary>
        [SugarColumn(ColumnName = "password")]
        public string password { get; set; }
        /// <summary>
        /// 昵称 
        ///</summary>
        [SugarColumn(ColumnName = "nickname")]
        public string nickname { get; set; }
        /// <summary>
        /// 头像 
        ///</summary>
        [SugarColumn(ColumnName = "avatar")]
        public string avatar { get; set; }
        /// <summary>
        /// 性别 
        ///</summary>
        [SugarColumn(ColumnName = "sex")]
        public string sex { get; set; }
        /// <summary>
        /// 手机号 
        ///</summary>
        [SugarColumn(ColumnName = "phone")]
        public string phone { get; set; }
        /// <summary>
        /// 邮箱 
        ///</summary>
        [SugarColumn(ColumnName = "email")]
        public string email { get; set; }
        /// <summary>
        /// 邮箱是否验证, 0否, 1是 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "email_verified")]
        public int emailVerified { get; set; }
        /// <summary>
        /// 真实姓名 
        ///</summary>
        [SugarColumn(ColumnName = "real_name")]
        public string realName { get; set; }
        /// <summary>
        /// 身份证号 
        ///</summary>
        [SugarColumn(ColumnName = "id_card")]
        public string idCard { get; set; }
        /// <summary>
        /// 出生日期 
        ///</summary>
        [SugarColumn(ColumnName = "birthday")]
        public string birthday { get; set; }
        /// <summary>
        /// 个人简介 
        ///</summary>
        [SugarColumn(ColumnName = "introduction")]
        public string introduction { get; set; }
        /// <summary>
        /// 机构id 
        ///</summary>
        [SugarColumn(ColumnName = "organization_id")]
        public int? organizationId { get; set; }
        /// <summary>
        /// 状态, 0正常, 1冻结 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public int? status { get; set; }
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
        /// 注册时间 
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
        /// 机构名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string organizationName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string sexName { get; set; }


        /// <summary>
        /// 权限
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysRole> roles { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysMenu> authorities { get; set; }


        /// <summary>
        /// 账户是否到期
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool accountNonExpired
        {
            get { return true; }
        }

        /// <summary>
        /// 账户是否锁定
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool accountNonLocked
        {
            get
            {
                return status != null && status == 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool credentialsNonExpired
        {
            get { return true; }
        }

        /// <summary>
        /// 账户是否可用
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool enabled
        {
            get { return true; }
        }
    }

    public class SysUserExistenceModel
    {
        [Required]
        public string field { get; set; }

        [Required]
        public string value { get; set; }

        public int? id { get; set; }
    }

}
