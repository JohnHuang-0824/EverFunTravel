using EverFunTravel.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TravelRepository;
using TravelRepository.Table;

namespace EverFunTravel.Models
{
    public class AccountManageModel: Customer_UserMstr
    {

        [Reunite(Required = true), Display(Name = "大頭貼")]
        public override string CustomerHeadImg { get; set; }
        [Reunite(Required = true), Display(Name = "電子信箱")]
        public string CustomerEmail { get; set; }
        public static async Task<AccountManageModel> GetCustomerInfo(IDatabaseHelper helper,string customerId)
        {
            string sql = @"select CM.*,UA.CustomerEmail from [Customer_UserMstr] CM 
LEFT JOIN [Customer_UserAccount] UA ON CM.CustomerId = UA.Customer
WHERE UA.Acitve = 1 and CM.CustomerId = @customerId";
            AccountManageModel model = await helper.QueryAsync<AccountManageModel>(sql, new { customerId });
            return model;
        }
    }
}
