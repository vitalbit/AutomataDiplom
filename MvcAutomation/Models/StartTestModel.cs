using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAutomation.Models
{
    public class StartTestModel
    {
        public int TestId { get; set; }
        public string TypeName { get; set; }
        public string TestName { get; set; }
        public string JsFileName { get; set; }
        public string CssFileName { get; set; }
        public string DllFilePath { get; set; }
        public string ResolveDllType { get; set; }
        public int TestFileNumber { get; set; }
    }
}