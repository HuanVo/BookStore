using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BookStore.Commons
{
    public class ErrorLog
    {
        public static void WriteLog(String MessError)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + MessError);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }

        }
        public static void WriteLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }

        }
    }
}