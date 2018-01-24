using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Log
    {
        /// <summary>
        /// 打印到记事本
        /// </summary>
        /// <param name="LogName">日志文件夹名称</param>
        /// <param name="title1">标题1</param>
        /// <param name="strMessage1">内容1</param>
        /// <param name="title2">标题2</param>
        /// <param name="strMessage2">内容2</param>
        public static void WriteTextLog(string LogName, string title1, string strMessage1, string title2, string strMessage2)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + @"Logss\" + LogName + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + DateTime.Now.ToString() + "\r\n");
            str.Append("title1:    " + title1 + "\r\n");
            str.Append("strMessage1:    " + strMessage1 + "\r\n");
            str.Append("title2:    " + title2 + "\r\n");
            str.Append("strMessage2:    " + strMessage2 + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
            sw.Dispose();
        }
    }
}
