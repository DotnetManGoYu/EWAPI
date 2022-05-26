using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWA.Sugar.Entity.Input
{
    public class LoginInput
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        public string code { get; set; }


        /// <summary>
        /// 多租户Id
        /// </summary>
        [Required]
        public int tenantId { get; set; }


        public bool remember { get; set; }

    }
}
