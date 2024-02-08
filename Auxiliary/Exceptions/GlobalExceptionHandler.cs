using System;
using System.Threading;
using System.Windows.Forms;

public static class GlobalExceptionHandler
{
    public static void Initialize()
    {
        Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
    }

    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        Exception ex = e.Exception;
        MessageBox.Show(ex.Message, $"Error: (unhandled exception:{ex.GetType().Name})", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //write stack trace somewhere? and/or add logging?
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception ex = e.ExceptionObject as Exception;
        if (ex != null)
        {
            MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
