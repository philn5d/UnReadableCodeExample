using ReadableCodeServices;
using System;
using System.Windows.Forms;

namespace QuotingWindowsApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UnityContainerHelper.Configure();
            Application.Run(new QuotingForm("test", UnityContainerHelper.GetInstance<IQuotingService>()));
        }
    }
}
