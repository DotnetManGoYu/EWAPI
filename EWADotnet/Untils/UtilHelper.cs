namespace EWADotnet
{
    public static class UtilHelper
    {
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        public static string MD5Encrypt16(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(str);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }

        private static string GetSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(10, 'a');
        }

        /// <summary>
        /// 密码比较
        /// </summary>
        /// <param name="inputpassword"></param>
        /// <param name="dbpassword"></param>
        /// <returns></returns>
        public static bool BCryptPasswordMatches(string inputpassword, string dbpassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputpassword, dbpassword);
        }

        /// <summary>
        /// 加密明文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string BCryptPasswordEncoder(string str)
        {
            var salt = GetSalt();
            return BCrypt.Net.BCrypt.HashPassword(str, salt);
        }
    }
}
