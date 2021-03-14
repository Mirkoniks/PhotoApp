using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class Report
    {
        public string Id { get; set; }

        public string Descripton { get; set; }

        public int ReportedSubjectId { get; set; }

        public DateTime ReportedOn { get; set; }

        public bool IsResolved { get; set; }
    }
}
