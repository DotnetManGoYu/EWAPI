using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;

namespace EWADotnet.Authorize
{
    public class JwtHandler : AppAuthorizeHandler
    {
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            // 自动刷新 token
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
            {
                await AuthorizeHandleAsync(context);
            }
        }
        public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 这里写您的授权判断逻辑，授权通过返回 true，否则返回 false           
            // 检查权限，如果方法时异步的就不用 Task.FromResult 包裹，直接使用 async/await 即可
            return Task.FromResult(CheckAuthorzie(httpContext));

        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private bool CheckAuthorzie(DefaultHttpContext httpContext)
        {
            //访问用户信息
            var claims = httpContext.User.Claims.ToArray();
            var user = claims.Where(it => it.Type == "username").FirstOrDefault();
            return user != null;
        }

    }
}
