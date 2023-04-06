using EverFunTravel.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using TravelRepository;
using TravelRepository.Table;

namespace EverFunTravel.Models
{
    public class AccountCreateModel : Customer_UserMstr
    {
        [Reunite(RegRule.電郵), Display(Name = "Email")]
        public string CustomerEmail { get; set; }
        [Reunite(RegRule.密碼, Required = true), Display(Name = "密碼")]
        public string CustomerPassword { get; set; }
        [Reunite(Required = true, Compare = "=CustomerPassword"), Display(Name = "確認密碼")]
        public string? CheckPassword { get; set; }
        [Reunite(Required = true), Display(Name = "姓名")]
        public override string? CustomerName { get; set; }
        public bool Add(IDatabaseHelper dbHelper, out string err)
        {
            err = ""; var param = new { this.CustomerEmail, this.CustomerPhone};
            try
            {
                var used = dbHelper.Query<bool>("SELECT 1 FROM Customer_UserAccount WHERE CustomerEmail = @CustomerEmail", param);
                if (used)
                {
                    err = "信箱已被使用";
                    return false;
                }
                used = dbHelper.Query<bool>("SELECT 1 FROM Customer_UserMstr WHERE CustomerPhone = @CustomerPhone", param);
                if (used)
                {
                    err = "手機已被使用";
                    return false;
                }
                    
                this.CustomerId = NewId();
                this.CustomerPassword = Customer_UserAccount.EncPassword(this.CustomerEmail, this.CustomerPassword);
                Customer_UserAccount Account = new Customer_UserAccount()
                {
                    CustomerId = this.CustomerId,
                    CustomerEmail = this.CustomerEmail,
                    CustomerPassword = this.CustomerPassword,
                    Active = true
                };
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var trx = conn.BeginTransaction())
                    {
                        dbHelper.Insert((Customer_UserMstr)this, "@Admin", conn, trx);
                        dbHelper.Insert(Account, "@Admin", conn, trx);
                        trx.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return false;
        }
    }
}
