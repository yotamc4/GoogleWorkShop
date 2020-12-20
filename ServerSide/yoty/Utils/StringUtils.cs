// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Utils
{
    using System;

    public static class StringUtils
    {
        public static bool IsValidString(this string str)
        {
            if (str == null || str.Length ==0)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidId(this string id)
        {
            if (IsValidString(id) && Guid.TryParse(id, out Guid guidOutput)) {
                return true;
            }
            return false;
        }
    }
}
