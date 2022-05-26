using EWA.Sugar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using EWA.Sugar.Entity.Input;
using Furion.DataEncryption;
using System.Drawing;
using System.Drawing.Imaging;
using EWADotnet.Authorize;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("api/")]
    public class MainController : BaseController
    {
        public MainController(ISqlSugarClient db) : base(db)
        {

        }

        [HttpGet("captcha")]
        [AllowAnonymous]
        public CommonResult Captcha()
        {
            var code = "";
            var imagecode = CreateImageCode(out code);
            var base64code = "data:image/png;base64," + Convert.ToBase64String(imagecode.ToArray());
            return Result.Success(new { base64 = base64code, text = code.ToLower() });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="lg"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<CommonResult> PostLogin(LoginInput lg)
        {
            var user = await db.Queryable<SysUser>()
                .FirstAsync(x => x.username == lg.username
                && x.tenantId == lg.tenantId
                && x.deleted == 0);
            var message = "";
            if (user == null)
            {
                message = "账号不存在";
                SaveLoginLog(lg.username, 1, message, lg.tenantId);
                return Result.Error(message);
            }
            if (user.status == 1)
            {
                message = "账户已冻结";
                SaveLoginLog(lg.username, 1, message, lg.tenantId);
                return Result.Error(message);
            }
            if (!UtilHelper.BCryptPasswordMatches(lg.password, user.password))
            {
                message = "密码错误";
                SaveLoginLog(lg.username, 1, message, lg.tenantId);
                return Result.Error(message);
            }
            user = SelectRoleAndAuth(user);
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>(){
                     { "userid",user.userId},
                     { "tenantId", user.tenantId },
                     { "username",user.username },
                     });
            SaveLoginLog(user.username, 0, "登陆成功", lg.tenantId);
            //第二个参数是刷新 token 的有效期（分钟），默认30分钟
            var refresh_token = JWTEncryption.GenerateRefreshToken(accessToken, 30);
            var data = new { access_token = "Bearer " + accessToken, user = user, refresh_token = refresh_token };
            return Result.Success(data, "登录成功");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/auth/user"), OperLog("登录认证", "获取登录用户信息")]
        public CommonResult UserInfo()
        {
            var user = SelectRoleAndAuth(LoginUser);
            return Result.Success(user);
        }


        /// <summary>
        /// 操作类型,0登录成功,1登录失败,2退出登录,3刷新token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="operType"></param>
        protected void SaveLoginLog(string username, int loginType, string comments, int tenantId)
        {
            var model = new SysLoginRecord();
            model.username = username;
            model.os = NetHelper.GetOSVersion();
            model.device = "";
            model.browser = NetHelper.Browser;
            model.ip = NetHelper.Ip;
            model.loginType = loginType;
            model.createTime = DateTime.Now;
            model.updateTime = DateTime.Now;
            model.tenantId = tenantId;
            model.comments = comments;
            db.Insertable(model).ExecuteCommand();
        }


        /// <summary>
        /// 获取登录用户菜单
        /// </summary>
        [OperLog("登录认证", "获取登录用户菜单")]
        public CommonResult GetMenu()
        {
            var list = ListByUserId(LoginUser.userId, 0);
            var menu = ToMenuTree(list, 0);
            return Result.Success(menu);
        }


        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>

        [OperLog("登录认证", "获取登录用户信息")]
        public async Task<CommonResult> GetUser()
        {
            var umodel = await db.Queryable<SysUser>()
                 .LeftJoin<SysOrganization>((a, b) => a.organizationId == b.organizationId)
                 .LeftJoin<SysDictionaryData>((a, b, c) => a.sex == c.dictDataCode)
                 .Where((a, b, c) => a.userId == LoginUser.userId)
                 .Select((a, b, c) => new SysUser
                 {
                     userId = a.userId,
                     username = a.username,
                     nickname = a.nickname,
                     avatar = a.avatar,
                     sex = a.sex,
                     phone = a.phone,
                     email = a.email,
                     emailVerified = a.emailVerified,
                     //truena = a.trueName,
                     idCard = a.idCard,
                     birthday = a.birthday,
                     introduction = a.introduction,
                     organizationId = a.organizationId,
                     status = a.status,
                     deleted = a.deleted,
                     createTime = a.createTime,
                     updateTime = a.updateTime,
                     organizationName = b.organizationName,
                     sexName = c.dictDataName
                 })
                 .FirstAsync();
            var user = SelectRoleAndAuth(umodel);
            return Result.Success(user);
        }




    }
}
