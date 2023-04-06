using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EverFunTravel.Attributes
{
    public enum RegRule
    {
        無,
        非特殊字元,
        字母,
        數字,
        字母和數字,
        手機號,
        電話,
        中文,
        帳號,
        家長帳號,
        密碼,
        電郵,
    }
    public class ReuniteAttribute : RegularExpressionAttribute
    {
        private const string _defaultPattern = @".*";
        public ReuniteAttribute()
            : base(_defaultPattern)
        {
        }
        public ReuniteAttribute(RegRule rule)
            : base(RuleToPattern(rule))
        {
            this.Rule = rule;
        }
        public ReuniteAttribute(string pattern)
            : base(pattern)
        {
        }
        /// <summary>
        /// 必填项
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        ///  字符串长度(最小)
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// 字符串长度(最大)
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public object MinValue { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public object MaxValue { get; set; }
        /// <summary>
        /// [>=字段]
        /// </summary>
        public string? Compare { get; set; }
        private RegRule Rule { get; set; }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            string name = validationContext.DisplayName;
            if (Required == true && (value == null || string.IsNullOrEmpty(value.ToString())))
            {
                return new ValidationResult(string.Format("請輸入[{0}]", name));
            }
            if (value == null)
            {
                return ValidationResult.Success;
            }
            string v = value.ToString();
            if (MinLength > 0 && value is string && v.Length < MinLength)
            {
                return new ValidationResult(string.Format("[{0}]不可少於{1}個字元", name, MinLength));
            }
            if (MaxLength > 0 && value is string && v.Length > MaxLength)
            {
                return new ValidationResult(string.Format("[{0}]不可超過{1}個字元", name, MaxLength));
            }
            if (!string.IsNullOrEmpty(Compare) && value is IComparable)
            {
                #region Compare
                string targetID = Compare.Substring(1).Replace("=", "").Trim();
                var targetProperty = validationContext.ObjectInstance.GetType().GetProperty(targetID);
                var targetValue = targetProperty.GetValue(validationContext.ObjectInstance, null);
                var targetContext = new ValidationContext(validationContext.ObjectInstance, null, null) { MemberName = targetID };

                string targetName = targetContext.DisplayName;
                if (targetValue == null)
                {
                    return new ValidationResult(string.Format("[{0}]不可為空", targetName));
                }
                int diff = ((IComparable)value).CompareTo(Convert.ChangeType(targetValue, value.GetType()));
                var c = Compare.Substring(1, 1) == "=" ? Compare.Substring(0, 2) : Compare.Substring(0, 1);
                if (c.Equals(">=") && diff < 0)
                {
                    return new ValidationResult(string.Format("[{0}]必須大於等於[{1}]", name, targetName));
                }
                else if (c.Equals("<=") && diff > 0)
                {
                    return new ValidationResult(string.Format("[{0}]必須小於等於[{1}]", name, targetName));
                }
                else if (c.Equals(">") && diff <= 0)
                {
                    return new ValidationResult(string.Format("[{0}]必須大於[{1}]", name, targetName));
                }
                else if (c.Equals("<") && diff >= 0)
                {
                    return new ValidationResult(string.Format("[{0}]必須小於[{1}]", name, targetName));
                }
                else if (c.Equals("=") && diff != 0)
                {
                    return new ValidationResult(string.Format("[{0}]和[{1}]必須相同", name, targetName));
                }
                #endregion
            }
            if (MinValue != null && value is IComparable)
            {
                int diff = ((IComparable)value).CompareTo(Convert.ChangeType(MinValue, value.GetType()));
                if (diff < 0)
                {
                    return new ValidationResult(string.Format("[{0}]最小值{1}", name, MinValue));
                }
            }
            if (MaxValue != null && value is IComparable)
            {
                int diff = ((IComparable)value).CompareTo(Convert.ChangeType(MaxValue, value.GetType()));
                if (diff > 0)
                {
                    return new ValidationResult(string.Format("[{0}]最大值{1}", name, MaxValue));
                }
            }
            if (this.Pattern == _defaultPattern)
            {
                return ValidationResult.Success;
            }
            ErrorMessage = ErrorMessage ?? RuleToErrorMessage(this.Rule);
            validationContext.DisplayName = name;
            return base.IsValid(value, validationContext);
        }
        public static string RuleToErrorMessage(RegRule rule)
        {
            string rs = string.Empty;
            switch (rule)
            {
                case RegRule.無:
                    break;
                case RegRule.非特殊字元:
                    rs = "[{0}]包含有特殊符號";
                    break;
                case RegRule.字母:
                    rs = "請輸入英文字母";
                    break;
                case RegRule.數字:
                    rs = "請輸入有效數字";
                    break;
                case RegRule.字母和數字:
                    rs = "請輸入英文字母或數字";
                    break;
                case RegRule.手機號:
                    rs = "[{0}]請輸入正確的手機號碼格式";
                    break;
                case RegRule.電話:
                    rs = "[{0}]請輸入正確的電話號碼格式";
                    break;
                case RegRule.中文:
                    rs = "[{0}]請輸入中文";
                    break;
                case RegRule.密碼:
                    rs = "[{0}]請輸入6位元以上英文字母或數字";
                    break;
                case RegRule.電郵:
                    rs = "[{0}]請輸入正確的信箱格式";
                    break;
                default:
                    break;
            }
            return rs;
        }
        public static string RuleToPattern(RegRule rule)
        {
            string rs = string.Empty;
            switch (rule)
            {
                case RegRule.無:
                    break;
                case RegRule.非特殊字元:
                    rs = @"^[0-9a-zA-Z\u4e00-\u9fa5\s\.]+(·[\u4e00-\u9fa5]+)*$";
                    break;
                case RegRule.字母:
                    rs = @"^[a-zA-Z]+$";
                    break;
                case RegRule.數字:
                    rs = @"^[-+]?[0-9]+$";
                    break;
                case RegRule.字母和數字:
                    rs = @"^[a-zA-Z0-9]+$";
                    break;
                case RegRule.手機號:
                    rs = @"^^[0]?(1[3-9])[\d]{9}$|^((\+?886\-?)9|09)[\d]{8}$";
                    break;
                case RegRule.電話:
                    rs = @"(\d{2,3}-?|\(\d{2,3}\))\d{3,4}-?\d{4}";
                    break;
                case RegRule.中文:
                    rs = @"^[0-9\u4e00-\u9fa5]+(·[\u4e00-\u9fa5]+)*$";
                    break;
                case RegRule.帳號:
                    rs = @"^[A-Z]{3}[0-9]{6}$";
                    break;
                    break;
                case RegRule.密碼:
                    rs = @"(?=.*[A-Z]+)(?=.*[a-z]+)(?=.*[0-9]+).{6,30}";
                    break;
                case RegRule.電郵:
                    rs = @"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$";
                    break;
                default:
                    break;
            }
            return rs;
        }
    }
}
