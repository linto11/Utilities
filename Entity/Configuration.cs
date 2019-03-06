using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Utility.Collection.Entity
{
    public class Configuration
    {
        public List<ConnectionString> ConnectionStrings { get; set; }
        public List<AppSetting> AppSettings { get; set; }
    }

    public class ConnectionString
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AppSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}