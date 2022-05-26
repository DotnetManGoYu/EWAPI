using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/menu")]
    public class MenuController : BaseController
    {
        public MenuController(ISqlSugarClient _db) : base(_db)
        {

        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [PreAuthorize("sys:menu:list"), OperLog("菜单管理", "分页查询菜单")]
        public async Task<CommonResult> Get([FromQuery] SysMenu input)
        {
            var exp = Expressionable.Create<SysMenu>();
            exp.And(x => x.deleted == 0 && x.tenantId == LoginUser.tenantId);
            exp.AndIF(!string.IsNullOrEmpty(input.title), x => x.title.Contains(input.title.Trim()));
            exp.AndIF(!string.IsNullOrEmpty(input.path), x => x.path.Contains(input.path.Trim()));
            exp.AndIF(!string.IsNullOrEmpty(input.authority), x => x.authority.Contains(input.authority.Trim()));
            var list = await db.Queryable<SysMenu>().Where(exp.ToExpression()).OrderBy(x => x.createTime, OrderByType.Desc).ToListAsync();
            return Result.Success(list);
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <returns></returns>
        [PreAuthorize("sys:menu:save"), OperLog("菜单管理", "添加菜单")]
        public async Task<CommonResult> Post(SysMenu menu)
        {
            menu.tenantId = LoginUser.tenantId;
            menu.createTime = DateTime.Now;
            var rows = await db.Insertable(menu).ExecuteCommandAsync();
            return Result.Success(rows > 0);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [PreAuthorize("sys:menu:update"), OperLog("菜单管理", "修改菜单")]
        public async Task<CommonResult> Put(SysMenu menu)
        {
            menu.updateTime = DateTime.Now;
            var row = await db.Updateable(menu).ExecuteCommandAsync();
            return Result.Success(row > 0);
        }

        /// <summary>
        /// 删除单个菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PreAuthorize("sys:menu:remove"), OperLog("菜单管理", "删除菜单")]
        public async Task<CommonResult> Delete([FromRoute] int id)
        {
            var row = await db.Updateable<SysMenu>().SetColumns(x => new SysMenu() { deleted = 1 }).Where(x => x.menuId == id).ExecuteCommandAsync();
            return Result.Success(row > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [PreAuthorize("sys:menu:remove"), OperLog("菜单管理", "批量删除菜单")]
        public async Task<CommonResult> DeleteBatch([FromBody] List<int> ids)
        {
            var rows = await db.Updateable<SysMenu>()
                .SetColumns(x => new SysMenu() { deleted = 1 })
                .Where(x => ids.Contains(x.menuId))
                .ExecuteCommandAsync();
            return Result.Success(rows > 0);
        }


    }
}
