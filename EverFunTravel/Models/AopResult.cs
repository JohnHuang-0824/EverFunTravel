namespace EverFunTravel.Models
{
    public class AopResult<T>
    {
        /// <summary>
        /// 狀態，true：報表有有效數據返回；false：訪問網絡失敗或者無有效數據
        /// </summary>
        public bool? Success { get; set; }
        /// <summary>
        /// 當success=false，message有值，說明返回無效的原因
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 返回的實體數據
        /// </summary>
        public T? Content { get; set; }
        public string? Redirecturl { get; set; }
    }
}
