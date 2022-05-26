using EWA.Sugar;
using Furion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SqlSugar;
using System.Security.Claims;

namespace EWADotnet.Authorize
{
    public class PreAuthorize : ActionFilterAttribute
    {
        private string Authority { get; set; }

        public PreAuthorize(string authority)
        {
            Authority = authority;
        }

        /// <summary>
        /// 判断用户是否有权限
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userid = App.User?.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(Authority) && filterContext != null)
            {
                bool isexist = listByUserId(Convert.ToInt32(userid)).Where(x => x.authority == Authority).FirstOrDefault() != null;
                if (!isexist)
                {
                    filterContext.Result = new JsonResult(Result.Error("无权限"));
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 获取用户有的权限和菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<SysMenu> listByUserId(int userid)
        {
            var db = App.GetService<ISqlSugarClient>();
            var userRole = db.Queryable<SysUserRole>().Where(x => x.userId == userid).Select(x => x.roleId).ToList();
            var exp = Expressionable.Create<SysRoleMenu, SysMenu, SysRole>();
            exp.And((a, b, c) => b.authority != null && userRole.Contains(a.roleId) && b.deleted == 0 && c.deleted == 0);
            var list = db.Queryable<SysRoleMenu>()
                .LeftJoin<SysMenu>((a, b) => a.menuId == b.menuId)
                .LeftJoin<SysRole>((a, b, c) => a.roleId == c.roleId)
                .Where(exp.ToExpression())
                .OrderBy((a, b, c) => b.sortNumber)
                .Select((a, b, c) => b)
                .Distinct()
                .ToList();
            return list;
        }
    }
}
