using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SQLite;

namespace The_Best_Breastfeeding_Tracker.Models
{
    public class FeedingRecord
    {
        private Color _colorCode;
        private string _breast;
        private string _minutes;
        private DateTime _date;
        private int _id;
        [PrimaryKey, AutoIncrement]
        public int ID { get => _id; set => _id = value; }
        [Ignore]
        public Color ColorCode { get => _colorCode; set => _colorCode = value; }
        public string Breast { get => _breast; set => _breast = value; }
        public string Minutes { get => _minutes; set => _minutes = value; }
        public DateTime Date { get => _date; set => _date = value; }

        public FeedingRecord(string breast, string min, DateTime date)
        {
            Breast = breast;
            Minutes = min;
            if (!string.IsNullOrEmpty(min)) Minutes += " Minutes";
            Date = date;
            if (Breast == "Left") ColorCode = Color.LightSkyBlue;
            if (Breast == "Right") ColorCode = Color.LightPink;
            if (Breast == "Both") ColorCode = Color.LightGreen;
        }
        public FeedingRecord()
        {
            if (Breast == "Left") ColorCode = Color.LightSkyBlue;
            if (Breast == "Right") ColorCode = Color.LightPink;
            if (Breast == "Both") ColorCode = Color.LightGreen;
        }
    }
}
