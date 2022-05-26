using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/operation-record")]
    public class OperationRecordController : BaseController
    {
        public OperationRecordController(ISqlSugarClient _db) : base(_db)
        {
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PreAuthorize("sys:operation-record:view")]
        public async Task<CommonPageResult> GetPage([FromQuery] SysOperationRecordParam param)
        {
            var userids = new List<int>();
            if (!string.IsNullOrEmpty(param.username))
            {
                userids = db.Queryable<SysUser>().Where(x => x.tenantId == LoginUser.tenantId && x.username.Contains(param.username.Trim()))
                    .ToList()
                    .Select(x => x.userId)
                    .ToList();
            }

            var exp = Expressionable.Create<SysOperationRecord, SysUser>();
            exp.And((x, y) => x.tenantId == LoginUser.tenantId);
            exp.AndIF(!string.IsNullOrEmpty(param.username), (x, y) => userids.Contains(x.userId.Value));
            exp.AndIF(!string.IsNullOrEmpty(param.module), (x, y) => x.module.Contains(x.module));
            exp.AndIF(param.createTimeStart != null, (x, y) => x.createTime >= param.createTimeStart.Value);
            exp.AndIF(param.createTimeEnd != null, (x, y) => x.createTime <= param.createTimeEnd.Value);
            RefAsync<int> total = 0;
            var list = await db.Queryable<SysOperationRecord>()
                .LeftJoin<SysUser>((x, y) => x.userId == y.userId)
                .Where(exp.ToExpression())
                .OrderBy(x => x.createTime, OrderByType.Desc)
                .Select((x, y) => new SysOperationRecord()
                {
                    id = x.id.SelectAll(),
                    nickname = y.nickname,
                    username = y.username
                })
                .ToPageListAsync(param.page.Value, param.limit.Value, total);
            return Result.PageSuccess(list, total);
        }

        /// <summary>
        /// 导出查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PreAuthorize("sys:operation-record:view")]
        public async Task<CommonResult> Get([FromQuery] SysOperationRecordParam param)
        {
            var userids = new List<int>();
            if (!string.IsNullOrEmpty(param.username))
            {
                userids = db.Queryable<SysUser>().Where(x => x.tenantId == LoginUser.tenantId && x.username.Contains(param.username.Trim()))
                    .ToList()
                    .Select(x => x.userId)
                    .ToList();
            }
            var exp = Expressionable.Create<SysOperationRecord, SysUser>();
            exp.And((x, y) => x.tenantId == LoginUser.tenantId);
            exp.AndIF(!string.IsNullOrEmpty(param.username), (x, y) => userids.Contains(x.userId.Value));
            exp.AndIF(!string.IsNullOrEmpty(param.module), (x, y) => x.module.Contains(x.module));
            exp.AndIF(param.createTimeStart != null, (x, y) => x.createTime >= param.createTimeStart.Value);
            exp.AndIF(param.createTimeEnd != null, (x, y) => x.createTime <= param.createTimeEnd.Value);
            var list = await db.Queryable<SysOperationRecord>()
                .LeftJoin<SysUser>((x, y) => x.userId == y.userId)
                .Where(exp.ToExpression())
                .OrderBy(x => x.createTime, OrderByType.Desc)
                 .Select((x, y) => new SysOperationRecord()
                 {
                     id = x.id.SelectAll(),
                     nickname = y.nickname,
                     username = y.username
                 })
                .ToListAsync();
            return Result.Success(list);
        }
    }
}
