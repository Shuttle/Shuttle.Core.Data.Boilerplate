using System;
using System.Data.Common;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Shuttle.Core.Data.Boilerplate
{
    [SupportedOSPlatform("Windows")]
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

            ApplicationConfiguration.Initialize();
            Application.Run(new MainView());
        }
    }
}
