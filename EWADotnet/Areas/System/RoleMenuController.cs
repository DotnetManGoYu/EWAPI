using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/role-menu")]
    public class RoleMenuController : BaseController
    {
        public RoleMenuController(ISqlSugarClient _db) : base(_db)
        {

        }


        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [PreAuthorize("sys:role:list")]

        public async Task<CommonResult> Get([FromRoute] int roleId)
        {
            var menus = await db.Queryable<SysMenu>().Where(x => x.tenantId == LoginUser.tenantId).OrderBy(x => x.sortNumber, OrderByType.Desc).ToListAsync();
            var roleMenus = await db.Queryable<SysRoleMenu>().Where(x => x.roleId == roleId && x.tenantId == LoginUser.tenantId).ToListAsync();
            menus.ForEach(menu =>
            {
                menu.@checked = false;
                roleMenus?.ForEach(roleMenu =>
                {
                    if (menu.menuId.Equals(roleMenu.menuId))
                    {
                        menu.@checked = true;
                    }
                });
            });
            return Result.Success(menus);
        }



        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        [PreAuthorize("sys:role:update")]
        public async Task<CommonResult> Put([FromRoute] int roleId, [FromBody] List<int> menuIds)
        {
            var roleMenuList = new List<SysRoleMenu>();
            foreach (int menuId in menuIds ?? new List<int>())
            {
                SysRoleMenu roleMenu = new SysRoleMenu();
                roleMenu.roleId = roleId;
                roleMenu.menuId = menuId;
                roleMenu.createTime = DateTime.Now;
                roleMenu.updateTime = DateTime.Now;
                roleMenu.tenantId = LoginUser.tenantId;
                roleMenuList.Add(roleMenu);
            }
            try
            {
                db.Ado.BeginTran();
                await db.Deleteable<SysRoleMenu>().Where(x => x.roleId == roleId).ExecuteCommandAsync();
                await db.Insertable(roleMenuList).ExecuteCommandAsync();
                db.Ado.CommitTran();
                return Result.Success();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                return Result.Error(ex.Message);
            }
        }

    }
}
