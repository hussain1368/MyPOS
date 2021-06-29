using System.Globalization;
using System.Threading;
using System.Windows;

namespace HandyCalendar
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var culture = new CultureInfo("prs-AF");
            culture.NumberFormat = new CultureInfo("en-US").NumberFormat;
            Thread.CurrentThread.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            base.OnStartup(e);
        }
    }
}
