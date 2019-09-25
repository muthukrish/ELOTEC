namespace ELOTEC.Models
{
    using System;

    public class Log
    {
        public long LogId { get; set; }
        public Guid LogGuid { get; set; }
        public DateTime LogDate { get; set; }
        public string Machine { get; set; }
        public string Application { get; set; }
        public string ProcessName { get; set; }
        public string LogText { get; set; }
    }
}
