

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Commons
{
    public static class Constancs
    {
        public static String SALT = @"zrRu^Wq>NI7?=]e1Y`@PjX/]+Kjl\)POEgIIl(B5%J:%ow&;<87e]2;Ske3>&+7[";
        public static String USER_SESSION = @"USER_SESSION";
        public static String COOKIE_NAME = "m";
        public static String SMTP_SERVER = "smtp.gmail.com";
        public static int MAIL_ID_CHANGE_PASSWORD = 2;
        public static int MAIL_ID_RESET_PASSWORD = 1;
        public static int SYSTEM_CONFIG_STATUS_LOCK = 1; // id config điều kiện control lock login
        public static int SYSTEM_CONFIG_COUNT_LOCK = 2; // id config số lần được login tối đa
        public static int SYSTEM_CONFIG_TIME_LOCK = 3; // id config khoảng thời lock account
        public static int SYSTEM_CONFIG_EXPIRES_COOKIE = 21; // id config khoảng thời lock account
    }

}
