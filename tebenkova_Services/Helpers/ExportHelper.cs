using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Services.Helpers
{
    public static class ExportHelper
    {
        public static byte[] ExportToCsv<T>(IEnumerable<T> data, string[] headers, Func<T, string[]> rowSelector)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Join(";", headers));

            foreach (var item in data)
            {
                sb.AppendLine(string.Join(";", rowSelector(item)));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public static string FormatCurrency(decimal amount)
        {
            return $"{amount:N2} ₽";
        }

        public static string FormatPercentage(decimal value)
        {
            return $"{value:F1}%";
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy HH:mm");
        }
    }
}
