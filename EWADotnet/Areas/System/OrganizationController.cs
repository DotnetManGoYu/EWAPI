using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/organization")]
    public class OrganizationController : BaseController
    {
        public OrganizationController(ISqlSugarClient _db) : base(_db)
        {

        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        [PreAuthorize("sys:org:list")]
        public async Task<CommonResult> Get()
        {
            var list = await db.Queryable<SysOrganization>()
                .Where(x => x.deleted == 0 && x.tenantId == LoginUser.tenantId)
                .ToListAsync();
            return Result.Success(list);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PreAuthorize("sys:org:list")]
        public async Task<CommonPageResult> GetPage(SysOrganizationParam param)
        {
            var exp = Expressionable.Create<SysOrganization>();
            exp.And(x => x.deleted == 0 && x.tenantId == LoginUser.tenantId);
            exp.AndIF(param.organizationId != null, x => x.organizationId == param.organizationId);
            RefAsync<int> total = 0;
            var list = await db.Queryable<SysOrganization>()
                .Where(exp.ToExpression())
                .OrderBy(x => x.sortNumber)
                .ToPageListAsync(param.page, param.limit, total);
            return Result.PageSuccess(list, total);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [PreAuthorize("sys:org:remove")]
        public async Task<CommonResult> Delete([FromRoute] int id)
        {
            //判断组织下是否存在用户
            var anyuser = await db.Queryable<SysUser>()
                .Where(x => x.deleted == 0 && x.tenantId == LoginUser.tenantId && x.organizationId == id)
                .AnyAsync();
            if (anyuser)
            {
                return Result.Error("机构下存在用户，不可直接删除！");
            }
            var row = await db.Updateable<SysOrganization>()
                .SetColumns(x => new SysOrganization() { deleted = 1, updateTime = DateTime.Now })
                .Where(x => x.organizationId == id && x.tenantId == LoginUser.tenantId)
                .ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }
    }
}
