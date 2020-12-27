﻿using System.Collections.Generic;
using System.ComponentModel;

namespace Radyn.PaymentGateway.Tools
{
    public class IranKishEnums
    {
        public static Dictionary<string, string> TarnsactionList
        {
            get
            {
                var dictionary = new Dictionary<string, string>
                {
                    {"100", "تراکنش با موفقیت انجام شد"},
                    {"110", "انصراف دارنده کارت"},
                    {"120", "موجودی حساب کافی نیست"},
                    {"130", "اطلاعات کارت اشتباه است"},
                    {"131", "رمز کارت اشتباه است"},
                    {"132", "کارت مسدود شده است"},
                    {"133", "کارت منقضی شده است"},
                    {"140", "زمان مورد نظر به پایان رسیده است"},
                    {"150", "خطای داخلی بانک"},
                    {"160", "خطا در اطلاعات CVV2 یا ExpDate"},
                    {"166", "بانک صادر کننده کارت شما مجوز انجام تراکنش را صادر نکرده است"},
                    {"200", "مبلغ تراکنش بیشتر از سقف مجاز برای هر تراکنش است"},
                    {"201", "مبلغ تراکنش بیشتر از سقف مجاز در روز است"},
                    {"202", "مبلغ تراکنش بیشتر از سقف مجاز در ماه است"},

                };
                return dictionary;
            }
        }

        public static Dictionary<string, string> VerifyStatusList
        {
            get
            {
                var dictionary = new Dictionary<string, string>
                {
                    {"1000", "تراکنش با موفقیت انجام شد"},
                    {"-20", "وجود کاراکترهای غیر مجاز در درخواست"},
                    {"-30", "تراکنش قبلا برگشت خورده است"},
                    {"-50", "طول رشته درخواست غیر مجاز است"},
                    {"-51", "خطا در درخواست"},
                    {"-80", "تراکنش مورد نظر یافت نشد"},
                    {"-81", "خطای داخلی بانک"},
                    {"-90", "تراکنش قبلا تائید شده است"},
                };
                return dictionary;
            }
        }

    }
}
