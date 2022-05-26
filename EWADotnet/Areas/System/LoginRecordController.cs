using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/login-record")]
    public class LoginRecordController : BaseController
    {
        public LoginRecordController(ISqlSugarClient _db) : base(_db)
        {
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PreAuthorize("sys:login-record:view"), OperLog("登录日志", "分页查询登录日志")]
        public async Task<CommonPageResult> GetPage([FromQuery] SysLoginRecordParam param)
        {
            var usernames = new List<string>();
            if (!string.IsNullOrEmpty(param.nickname))
            {
                usernames = db.Queryable<SysUser>()
                    .Where(x => x.tenantId == LoginUser.tenantId && x.nickname.Contains(param.nickname.Trim()))
                    .ToList()
                    .Select(x => x.username)
                    .ToList();
            }
            var exp = Expressionable.Create<SysLoginRecord>();
            exp.And(x => x.tenantId == LoginUser.tenantId);
            exp.AndIF(!string.IsNullOrEmpty(param.username), x => x.username.Contains(param.username.Trim()));
            exp.AndIF(!string.IsNullOrEmpty(param.nickname), x => usernames.Contains(x.username));
            exp.AndIF(param.createTimeStart != null, x => x.createTime >= param.createTimeStart.Value);
            exp.AndIF(param.createTimeEnd != null, x => x.createTime <= param.createTimeEnd.Value);
            RefAsync<int> total = 0;
            var list = await db.Queryable<SysLoginRecord>()
                .Where(exp.ToExpression())
                .OrderBy(x => x.createTime, OrderByType.Desc)
                .ToPageListAsync(param.page.Value, param.limit.Value, total);
            list?.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.username))
                    return;
                var user = db.Queryable<SysUser>().Where(y => y.username == x.username && y.tenantId == LoginUser.tenantId).First();
                if (user == null)
                    return;
                x.userId = user.userId;
                x.nickname = user.nickname;
            });
            return Result.PageSuccess(list, total);
        }

        /// <summary>
        /// 导出查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PreAuthorize("sys:login-record:view"), OperLog("登录日志", "查询全部登录日志")]
        public async Task<CommonResult> Get([FromQuery] SysLoginRecordParam param)
        {
            var usernames = new List<string>();
            if (!string.IsNullOrEmpty(param.nickname))
            {
                usernames = db.Queryable<SysUser>()
                    .Where(x => x.tenantId == LoginUser.tenantId && x.nickname.Contains(param.nickname.Trim()))
                    .ToList()
                    .Select(x => x.username)
                    .ToList();
            }
            var exp = Expressionable.Create<SysLoginRecord>();
            exp.And(x => x.tenantId == LoginUser.tenantId);
            exp.AndIF(!string.IsNullOrEmpty(param.username), x => x.username.Contains(param.username.Trim()));
            exp.AndIF(!string.IsNullOrEmpty(param.nickname), x => usernames.Contains(x.username));
            exp.AndIF(param.createTimeStart != null, x => x.createTime >= param.createTimeStart.Value);
            exp.AndIF(param.createTimeEnd != null, x => x.createTime <= param.createTimeEnd.Value);
            var list = await db.Queryable<SysLoginRecord>()
                .Where(exp.ToExpression())
                .OrderBy(x => x.createTime, OrderByType.Desc)
                .ToListAsync();
            list?.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.username))
                    return;
                var user = db.Queryable<SysUser>().Where(y => y.username == x.username && y.tenantId == LoginUser.tenantId).First();
                if (user == null)
                    return;
                x.userId = user.userId;
                x.nickname = user.nickname;
            });
            return Result.Success(list);
        }
    }
}
