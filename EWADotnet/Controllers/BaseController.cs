﻿using EWA.Sugar;
using Furion;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;

namespace EWADotnet
{
    public class BaseController : IDynamicApiController
    {
        protected SqlSugar.ISqlSugarClient db;
        public BaseController(ISqlSugarClient _db)
        {
            db = _db;
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected SysUser LoginUser
        {
            get
            {
                var userid = App.User?.FindFirstValue("userid");
                if (string.IsNullOrEmpty(userid))
                {
                    throw new Exception("需要重新登录");
                    // return null;
                }
                var user = db.Queryable<SysUser>().InSingle(Convert.ToInt64(userid));
                return user;
            }
        }
        #region Find User Menu
        /// <summary>
        /// 通过id获取权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuType"></param>
        protected List<SysMenu> ListByUserId(int userid, int? menuType)
        {
            //创建动态条件
            var userRole = db.Queryable<SysUserRole>().Where(x => x.userId == userid).Select(x => x.roleId).ToList();
            var exp = Expressionable.Create<SysRoleMenu, SysMenu, SysRole>();
            exp.AndIF(menuType != null, (a, b, c) => b.menuType == menuType.Value);
            exp.And((a, b, c) => userRole.Contains(a.roleId) && b.deleted == 0 && c.deleted == 0);
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
        protected List<SysMenu> ToMenuToMenuTreeTree(List<SysMenu> menus, int parentId)
        {
            var list = new List<SysMenu>();
            foreach (SysMenu menu in menus)
            {
                if (parentId.Equals(menu.parentId))
                {
                    menu.children = ToMenuTree(menus, menu.menuId);
                    list.Add(menu);
                }
            }
            return list;
        }

        protected List<SysMenu> ToMenuTree(List<SysMenu> menus, int parentId)
        {
            var list = new List<SysMenu>();
            foreach (SysMenu menu in menus)
            {
                if (parentId.Equals(menu.parentId))
                {
                    menu.children = ToMenuTree(menus, menu.menuId);
                    list.Add(menu);
                }
            }
            return list;
        }


        protected SysUser SelectRoleAndAuth(SysUser user)
        {
            //获取菜单
            user = SelectUserAuth(user);
            if (user.roles == null)
            {
                user.roles = new List<SysRole>();
            }
            var roles = db.Queryable<SysUserRole>()
               .LeftJoin<SysRole>((SUR, SR) => SUR.roleId == SR.roleId)
               .Where((SUR, SR) => SUR.userId == user.userId && SR.deleted == 0)
               .Select((SUR, SR) => SR)
               .ToList();
            user.roles = roles;//获取权限
            return user;
        }
        protected SysUser SelectUserAuth(SysUser user)
        {
            var menus = ListByUserId(user.userId, null);
            //var authList = new List<SysMenu>();
            //foreach (SysMenu menu in menus)
            //{
            //    if (IsNotBlank(menu.authority))
            //    {
            //        authList.Add(menu);
            //    }
            //}
            user.authorities = menus;
            return user;
        }
        protected static bool IsNotBlank(string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        protected static MemoryStream CreateImageCode(out string code, int numbers = 5)
        {
            code = RndNum(numbers);
            //Bitmap img = null;
            //Graphics g = null;
            MemoryStream ms = null;
            Random random = new Random();
            //验证码颜色集合 
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            using (var img = new Bitmap(code.Length * 18, 32))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.Clear(Color.White);//背景设为白色
                    //在随机位置画背景点 
                    for (int i = 0; i < 100; i++)
                    {
                        int x = random.Next(img.Width);
                        int y = random.Next(img.Height);
                        g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
                    }
                    //验证码绘制在g中 
                    for (int i = 0; i < code.Length; i++)
                    {
                        int cindex = random.Next(7);//随机颜色索引值 
                        int findex = random.Next(5);//随机字体索引值 
                        Font f = new Font(fonts[findex], 15, FontStyle.Bold);//字体 
                        Brush b = new SolidBrush(c[cindex]);//颜色 
                        int ii = 4;
                        if ((i + 1) % 2 == 0)//控制验证码不在同一高度 
                        {
                            ii = 2;
                        }
                        g.DrawString(code.Substring(i, 1), f, b, 3 + (i * 12), ii);//绘制一个验证字符 
                    }
                    ms = new MemoryStream();//生成内存流对象 
                    img.Save(ms, ImageFormat.Jpeg);//将此图像以Png图像文件的格式保存到流中 
                }
            }

            return ms;
        }
        protected static string RndNum(int VcodeNum)
        {
            //验证码可以显示的字符集合 
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
              ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
              ",R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组  
            string code = "";//产生的随机数 
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数 
            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同 
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类 
                }
                int t = rand.Next(61);//获取随机数 
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);//如果获取的随机数重复，则递归调用 
                }
                temp = t;//把本次产生的随机数记录起来 
                code += VcArray[t];//随机数的位数加一 
            }
            return code;
        }
        #endregion
    }
}
