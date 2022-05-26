using System;
using System.Collections.Generic;

namespace EWA.Sugar
{
    public class Menu
    {
        public int menuId { get; set; }

        public int parentId { get; set; }

        public string title { get; set; }

        public string path { get; set; }

        public string component { get; set; }
        public int menuType { get; set; }

        public string sortNumber { get; set; }

        public string authority { get; set; }
        public string target { get; set; }

        public string icon { get; set; }

        public string color { get; set; }
        public int hide { get; set; }

        public string active { get; set; }

        public string meta { get; set; }

        public int deleted { get; set; }

        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }

        public string parentTitle { get; set; }

        public bool? @checked { get; set; }
        public string open { get; set; }

        public List<Menu> children { get; set; }

    }

    public class role
    {
        public int roleId { get; set; }
        public string roleCode { get; set; }

        public string roleName { get; set; }
        public string comments { get; set; }

        public string createTime { get; set; }

        public string updateTime { get; set; }
        public int deleted { get; set; }

        public int? userId { get; set; }
    }
}
