using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Helpers
{
    public class ReportDataModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public int TotalWorkingHours { get; set; }
    }
}
