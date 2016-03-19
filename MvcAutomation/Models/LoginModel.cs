using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAutomation.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}