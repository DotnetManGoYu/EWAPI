using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq.Expressions;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/user")]
    public class UserController : BaseController
    {
        public UserController(ISqlSugarClient _db) : base(_db)
        {

        }

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CommonPageResult> GetPage([FromQuery] SysUser input)
        {
            var express = Expressionable.Create<SysUser, SysDictionaryData, SysDictionary>();
            express.And((x, y, z) => x.userId != LoginUser.userId);//排除下自身，防止自己把自己禁用了
            express.And((x, y, z) => x.deleted == 0 && x.tenantId == LoginUser.tenantId && z.dictCode == "sex");
            express.AndIF(!string.IsNullOrEmpty(input.nickname), (x, y, z) => x.nickname.Contains(input.nickname.Trim()));
            express.AndIF(!string.IsNullOrEmpty(input.username), (x, y, z) => x.username.Contains(input.username.Trim()));
            express.AndIF(!string.IsNullOrEmpty(input.sex), (x, y, z) => x.sex == input.sex);
            express.AndIF(input.organizationId != null, (x, y, z) => x.organizationId == input.organizationId.Value);
            RefAsync<int> total = 0;
            var list = await db.Queryable<SysUser>()
                .LeftJoin<SysDictionaryData>((x, y) => x.sex == y.dictDataCode)
                .LeftJoin<SysDictionary>((x, y, z) => y.dictId == z.dictId)
                .Where(express.ToExpression())
                .OrderBy((x, y, z) => x.createTime, OrderByType.Desc)
                .Select((x, y, z) => new SysUser
                {
                    userId = x.userId.SelectAll(),
                    sexName = y.dictDataName
                })
                .ToPageListAsync(input.page, input.limit, total);
            list.ForEach(x =>
            {
                x.roles = db.Queryable<SysUserRole>()
                .LeftJoin<SysRole>((a, b) => a.roleId == b.roleId)
                .Where((a, b) => a.userId == x.userId)
                .Select((a, b) => new SysRole()
                {
                    roleId = b.roleId,
                    roleName = b.roleName,
                    roleCode = b.roleCode,
                    comments = b.comments,
                    deleted = b.deleted,
                    createTime = b.createTime,
                    updateTime = b.updateTime,
                    userId = x.userId,
                    tenantId = b.tenantId
                })
                .ToList();
                if (!string.IsNullOrEmpty(x.birthday))
                {
                    x.birthday = DateTime.Parse(x.birthday).ToString("yyyy-MM-dd");
                }
            });
            return Result.PageSuccess(list, total);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:user:update")]
        public async Task<CommonResult> PutPassword(SysUser input)
        {
            var rows = await db.Updateable(input).UpdateColumns(x => x.password).ExecuteCommandAsync();
            return Result.Judge(rows > 0);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:user:save")]
        public async Task<CommonResult> Post(SysUser input)
        {
            input.status = 0;
            input.createTime = DateTime.Now;
            input.password = UtilHelper.BCryptPasswordEncoder(input.password);
            input.tenantId = LoginUser.tenantId;

            var exp = Expressionable.Create<SysUser>();
            exp.And(x => x.userId != input.userId
            && x.tenantId == LoginUser.tenantId
            && x.deleted == 0
            && x.username == input.username);

            var exist = await db.Queryable<SysUser>().Where(exp.ToExpression()).AnyAsync();
            if (exist)
            {
                return Result.Error("账号已存在！");
            }
            var userRoles = new List<SysUserRole>();
            input.roles?.ForEach(x =>
            {
                userRoles.Add(new SysUserRole()
                {
                    userId = input.userId,
                    roleId = x.roleId,
                    tenantId = input.tenantId,
                    createTime = DateTime.Now,
                    updateTime = DateTime.Now
                });
            });
            try
            {
                db.Ado.BeginTran();
                db.Insertable(input).InsertColumns(x => new
                {
                    x.email,
                    x.introduction,
                    x.nickname,
                    x.password,
                    x.phone,
                    x.sex,
                    x.username,
                    x.tenantId
                }).ExecuteCommand();//新增用户信息
                db.Insertable(userRoles).ExecuteCommand();
                db.Ado.CommitTran();
                return Result.Success();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                return Result.Error(ex.Message);
            }
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:user:update")]
        public async Task<CommonResult> PutStatus(SysUser input)
        {
            var rows = await db.Updateable(input).UpdateColumns(x => x.status).ExecuteCommandAsync();
            return Result.Judge(rows > 0);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:user:update")]
        public async Task<CommonResult> Put(SysUser input)
        {
            var exp = Expressionable.Create<SysUser>();
            exp.And(x => x.userId != input.userId && x.tenantId == LoginUser.tenantId);
            exp.And(x => x.username == input.username || x.phone == input.phone || x.email == input.email);
            var exist = await db.Queryable<SysUser>().Where(exp.ToExpression()).AnyAsync();
            if (exist)
            {
                return Result.Error("账号、手机号或邮箱号已存在！");
            }
            var userRoles = new List<SysUserRole>();
            input.roles?.ForEach(x =>
            {
                userRoles.Add(new SysUserRole()
                {
                    userId = input.userId,
                    roleId = x.roleId,
                    tenantId = x.tenantId,
                    createTime = DateTime.Now,
                    updateTime = DateTime.Now
                });
            });
            try
            {
                db.Ado.BeginTran();
                db.Updateable(input).UpdateColumns(x => new
                {
                    x.birthday,
                    x.email,
                    x.introduction,
                    x.nickname,
                    x.phone,
                    x.sex,
                    x.username

                }).ExecuteCommand();//更新用户信息
                db.Deleteable<SysUserRole>().Where(x => x.userId == input.userId).ExecuteCommand();//删除原来的权限
                db.Insertable(userRoles).ExecuteCommand();
                db.Ado.CommitTran();
                return Result.Success();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                return Result.Error(ex.Message);
            }
        }

        /// <summary>
        /// 查询用户是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:user:list")]
        public async Task<CommonResult> GetExistence([FromQuery] SysUserExistenceModel input)
        {
            var exp = Expressionable.Create<SysUser>();
            exp.And(x => x.tenantId == LoginUser.tenantId);
            exp.And(x => SqlFunc.MappingColumn(default(string), input.field) == input.value);
            exp.AndIF(input.id != null, x => x.userId != input.id);
            var exist = await db.Queryable<SysUser>().Where(exp.ToExpression()).AnyAsync();
            if (!exist)
            {
                return Result.Error(input.value + "不存在！");
            }
            return Result.Success(input.value + "已存在！");
        }


        [PreAuthorize("sys:user:list")]
        public async Task<CommonResult> Get([FromRoute] int id)
        {
            var model = await db.Queryable<SysUser>().Where(x => x.tenantId == LoginUser.tenantId && x.userId == id).FirstAsync();
            model = SelectRoleAndAuth(model);
            return Result.Success(model);
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [PreAuthorize("sys:role:remove")]
        public async Task<CommonResult> DeleteBatch([FromBody] List<int> ids)
        {
            var rows = await db.Updateable<SysUser>()
                .SetColumns(x => new SysUser() { deleted = 1, updateTime = DateTime.Now })
                .Where(x => x.tenantId == LoginUser.tenantId && ids.Contains(x.userId))
                .ExecuteCommandAsync();
            return Result.Judge(rows > 0);
        }

    }
}
