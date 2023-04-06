using EverFunTravel.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EverFunTravel.Models
{
    public class AccountLoginModel
    {
        [Reunite(Required = true), Display(Name = "帳號")]
        public string? Account { get; set; }

        [Reunite(Required = true), Display(Name = "密碼")]
        public string? Password { get; set; }

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
