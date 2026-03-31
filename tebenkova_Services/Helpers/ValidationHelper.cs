using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tebenkova_Services.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidINN(string inn)
        {
            if (string.IsNullOrEmpty(inn))
                return true;

            return Regex.IsMatch(inn, @"^\d{10}$|^\d{12}$");
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return true;

            return Regex.IsMatch(phone, @"^[\d\s\+\-\(\)]+$");
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
