using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Utility.Collection
{
    public sealed class LogHelper
    {
        private static Object synchronizerLogObj = new Object();
        private static string LogDirectory = string.Empty;

        public static async Task<bool> WriteLogAsync(string logDirectory, string exceptionSourceName, Exception ex)
        {
            LogDirectory = Path.GetFullPath(logDirectory);
            if (!SafeTypeHelper.SafeString(LogDirectory[LogDirectory.Length - 1]).Equals("/"))
                LogDirectory = LogDirectory + "/";

            return await WriteAsync(exceptionSourceName, ex);
        }

        public static async Task<bool> WriteLogAsync(string logDirectory, string customLogMessage)
        {
            LogDirectory = Path.GetFullPath(logDirectory);
            if (!SafeTypeHelper.SafeString(LogDirectory[LogDirectory.Length - 1]).Equals("/"))
                LogDirectory = LogDirectory + "/";

            return await WriteAsync(customLogMessage, null);
        }

        public static bool WriteLog(string logDirectory, string exceptionSourceName, Exception ex)
        {
            LogDirectory = Path.GetFullPath(logDirectory);
            if (!SafeTypeHelper.SafeString(LogDirectory[LogDirectory.Length - 1]).Equals("/"))
                LogDirectory = LogDirectory + "/";

            return Write(exceptionSourceName, ex);
        }

        public static bool WriteLog(string logDirectory, string customLogMessage)
        {
            LogDirectory = Path.GetFullPath(logDirectory);
            if (!SafeTypeHelper.SafeString(LogDirectory[LogDirectory.Length - 1]).Equals("/"))
                LogDirectory = LogDirectory + "/";

            return Write(customLogMessage, null);
        }

        public static string GetExceptionString(Exception ex, string exceptionSource)
        {
            StringBuilder logMessage = new StringBuilder();
            if (ex != null)
            {
                logMessage.Append(Environment.NewLine);
                logMessage.Append("Methods Name: ");
                logMessage.Append(exceptionSource);
                logMessage.Append(Environment.NewLine);
                logMessage.Append("Error Message: ");
                logMessage.Append(ex.Message);
                logMessage.Append(Environment.NewLine);
                logMessage.Append("Stack Trace: ");
                logMessage.Append(ex.StackTrace);
                logMessage.Append(Environment.NewLine);
                logMessage.Append("Error Type: ");
                logMessage.Append(ex.GetType().ToString());
                logMessage.Append(Environment.NewLine);
            }

            return logMessage.ToString();
        }

        private static Task<bool> WriteAsync(string exceptionSourceName, Exception ex)
        {
            return Task.Run<bool>(() =>
            {
                return Write(exceptionSourceName, ex);
            });
        }

        private static bool Write(string logContent, Exception ex, bool splitBySize = false, long splitSize = 5000000)
        {
            bool Status = false;

            DateTime CurrentDateTime = DateTime.Now;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);

            StringBuilder logMessage = new StringBuilder();
            if (ex != null)
                logMessage.Append(GetExceptionString(ex, logContent));
            else
                logMessage.Append(logContent);

            if (!string.IsNullOrEmpty(SafeTypeHelper.SafeString(logMessage)))
            {
                string logLine = BuildLogLine(CurrentDateTime, SafeTypeHelper.SafeString(logMessage));

                string fileName = string.Empty;
                if (!splitBySize)
                    fileName = DateTime.Now.ToString("ddMMyyyy") + ".txt";
                else
                    fileName = "LOGS.txt";

                if (File.Exists(LogDirectory + fileName))
                {
                    FileInfo fileInfo = new FileInfo(LogDirectory + fileName);
                    if (splitBySize && fileInfo.Length > splitSize)
                        System.IO.File.Move(LogDirectory + fileName, LogDirectory + "Logs_" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".txt");
                }

                lock (synchronizerLogObj)
                {
                    StreamWriter oStreamWriter = null;
                    try
                    {
                        oStreamWriter = new StreamWriter(LogDirectory + fileName, true);
                        oStreamWriter.WriteLine(logLine);
                        Status = true;
                    }
                    catch
                    { }
                    finally
                    {
                        if (oStreamWriter != null)
                        {
                            oStreamWriter.Close();
                        }
                    }
                }
            }

            //Delete 2 days before logs
            //DirectoryInfo di = new DirectoryInfo(LOG_DIRECTORY);
            //if (di.GetFiles().Length > 0)
            //    foreach (FileInfo fileInfo in di.GetFiles())
            //    {
            //        if (fileInfo.CreationTime <= DateTime.Now.AddDays(-2))
            //            fileInfo.Delete();
            //    }

            return Status;
        }

        private static string LogFileName(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd_MM_yyyy");
        }

        private static string BuildLogLine(DateTime CurrentDateTime, string LogMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));
            loglineStringBuilder.Append(Environment.NewLine);
            loglineStringBuilder.Append(LogMessage);
            return loglineStringBuilder.ToString();
        }

        private static bool CheckCreateLogDirectory(string LogPath)
        {
            bool loggingDirectoryExists = false;
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(LogPath);
            if (oDirectoryInfo.Exists)
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(LogPath);
                    loggingDirectoryExists = true;
                }
                catch
                { }
            }
            return loggingDirectoryExists;
        }

        private static string LogFileEntryDateTime(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
}