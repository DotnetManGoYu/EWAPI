using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/role")]
    public class RoleController : BaseController
    {
        public RoleController(ISqlSugarClient _db) : base(_db)
        {


        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:role:list")]
        public async Task<CommonPageResult> GetPage([FromQuery] SysRole input)
        {
            var exp = Expressionable.Create<SysRole>();
            exp.And(x => x.deleted == 0 && x.tenantId == LoginUser.tenantId);
            exp.AndIF(!string.IsNullOrEmpty(input.roleName), x => x.roleName.Contains(input.roleName.Trim()));
            exp.AndIF(!string.IsNullOrEmpty(input.roleCode), x => x.roleCode.Contains(input.roleCode.Trim()));
            exp.AndIF(!string.IsNullOrEmpty(input.comments), x => x.comments.Contains(input.comments.Trim()));
            RefAsync<int> total = 0;
            var list = await db.Queryable<SysRole>().Where(exp.ToExpression()).OrderBy(x => x.createTime, OrderByType.Desc).ToPageListAsync(input.page, input.limit, total);
            return Result.PageSuccess(list, total);
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:role:update")]
        public async Task<CommonResult> Put(SysRole input)
        {
            var any = await db.Queryable<SysRole>()
                .Where(x => x.deleted == 0 && x.tenantId == LoginUser.tenantId && (x.roleName == input.roleName.Trim() || x.roleCode == input.roleCode.Trim()) && x.roleId != input.roleId)
                .AnyAsync();
            if (any)
            {
                return Result.Error("角色名称或角色标识已存在！");
            }
            var rows = await db.Updateable(input).UpdateColumns(x => new { x.roleName, x.roleCode, x.comments }).ExecuteCommandAsync();
            return Result.Judge(rows > 0);
        }


        /// <summary>
        /// 删除指定角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PreAuthorize("sys:role:remove")]
        public async Task<CommonResult> Delete([FromRoute] int id)
        {
            var model = await db.Queryable<SysRole>().Where(x => x.tenantId == LoginUser.tenantId && x.roleId == id && x.deleted == 0).FirstAsync();
            if (model == null)
            {
                return Result.Error("角色不存在！");
            }
            model.updateTime = DateTime.Now;
            model.deleted = 1;
            var row = await db.Updateable(model).ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<CommonResult> DeleteBatch([FromBody] List<int> ids)
        {
            var rows = await db.Updateable<SysRole>()
                .SetColumns(x => new SysRole() { deleted = 1, updateTime = DateTime.Now })
                .Where(x => x.tenantId == LoginUser.tenantId && ids.Contains(x.roleId))
                .ExecuteCommandAsync();
            return Result.Judge(rows > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<CommonResult> Get()
        {
            var list = await db.Queryable<SysRole>().Where(x => x.deleted == 0).ToListAsync();
            return Result.Success(list);
        }
    }
}
