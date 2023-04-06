using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelRepository.Table
{
    [Table("Customer_UserMstr")]
    public class Customer_UserMstr : _BaseTable
	{
        [ExplicitKey]
        public virtual string? CustomerId { get; set; }
		public virtual string? CustomerName { get; set; }
		public virtual string? CustomerEName { get; set; }
		public virtual string? CustomerPhone { get; set; }
		public virtual bool? CustomerSex { get; set; }
		public virtual DateTime? CustomerBirth { get; set; }
		public virtual string? CustomerAddress { get; set; }
		public virtual string? CustomerHeadImg { get; set; }
        public virtual bool? IsSubscribe { get; set; }

    }
	public static class UserMstrExtensions
    {
        public static async Task<Customer_UserMstr?> GetUserAsync(this IDatabaseHelper dbHelper, string? account, string? plainPwd)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(plainPwd))
                return null;
            var sql = @"
select * from [Customer_UserMstr] CM where
exists (select 1 from Customer_UserAccount UA where UA.CustomerId=CM.CustomerId 
and UA.CustomerEmail=@account and UA.CustomerPassword=@password and UA.active=1)";
            var aaa = Customer_UserAccount.EncPassword(account, plainPwd);
            return await dbHelper.QueryAsync<Customer_UserMstr>(sql, new { account, password = Customer_UserAccount.EncPassword(account, plainPwd) });
        }
        public static Customer_UserMstr? GetUserMstrById(this IDatabaseHelper dbHelper, string? uid, int ExpiredSec = 86400)
        {
            if (string.IsNullOrEmpty(uid))
                return null;
            var result = dbHelper.Get<Customer_UserMstr>(uid);
            return result;

        }
    }
}
