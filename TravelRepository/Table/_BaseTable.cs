using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelRepository.Table
{
    public class _BaseTable
    {
        public string? CreateId { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? ModifyId { get; set; }
        public DateTime? ModifyTime { get; set; }

        public static string NewId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
