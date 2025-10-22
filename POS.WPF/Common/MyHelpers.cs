using System;
using System.Text;

namespace POS.WPF.Common
{
    public static class MyHelpers
    {
        public static string ToMessage(this Exception ex)
        {
            var sb = new StringBuilder();
            do
            {
                sb.AppendLine(ex.Message);
                ex = ex.InnerException;
            }
            while (ex != null);
            return sb.ToString().Trim();
        }
    }
}
