using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelUtil;
namespace TravelRepository.Table
{
    [Table("Customer_UserAccount")]
    public class Customer_UserAccount : _BaseTable
    {
        [ExplicitKey]
        public virtual string? CustomerId { get; set; }
        public virtual string? CustomerEmail { get; set; }
        public virtual string? CustomerPassword { get; set; }
        public bool? Active { get; set; }
        public static string EncPassword(string? account, string? plainPwd)
        {
            if (account == null || plainPwd == null)
                return "";

            return ("everfun" + account.ToLower() + plainPwd + "travel").ToMD5();
        }
    }
}
