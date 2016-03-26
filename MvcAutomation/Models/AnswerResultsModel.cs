using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAutomation.Models
{
    public class AnswerResultsModel
    {
        public int AnswerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Faculty { get; set; }
        public string Speciality { get; set; }
        public string Course { get; set; }
        public string Group { get; set; }
        public double Mark { get; set; }
    }
}