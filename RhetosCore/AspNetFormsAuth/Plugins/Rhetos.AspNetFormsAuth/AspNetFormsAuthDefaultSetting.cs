using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.AspNetFormsAuth
{
    public static class AspNetFormsAuthDefaultSetting
    {
        #region PASSWORD
        public const bool PASSWORD_REQUIRE_UPPER_CASE = false;

        public const bool PASSWORD_REQUIRE_NON_ALPHANUMERIC = false;

        public const bool PASSWORD_REQUIRE_LOWER_CASE = false;

        public const int PASSWORD_REQUIRED_LENGTH = 4;

        public const int PASSWORD_REQUIRED_UNIQUE_CHARS = 1;

        public const bool PASSWORD_REQUIRED_DIGITS = false;
        #endregion

        #region USER
        public const bool USER_REQUIRE_UNIQUE_EMAIL = true;

        public const string USER_ALLOWED_USERNAME_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
        #endregion

        #region OTHER
        public const int LOCKOUT_MAX_FAILED_ACCESS_ATTEMPTS = int.MaxValue;

        public const bool SIGIN_IN_REQUIRE_CONFIRMED_EMAIL = false;

        public const bool RUN_ADMIN_ACCOUNT_MIGRATION = false;

        public const string ADMIN_PASSWORD = "Test.123";

        public const string ADMIN_EMAIL = "admin@gmail.com";
        #endregion
    }
}
