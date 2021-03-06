using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;

namespace The_Best_Breastfeeding_Tracker.Services
{
    public static class Constants
    {
        public static string DatabaseFilename = "BreastFeedRecords.db3";
        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
