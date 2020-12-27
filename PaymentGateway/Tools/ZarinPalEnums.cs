﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.PaymentGateway.Tools
{
    public class ZarinPalEnums
    {
        public static Dictionary<string, string> VerifyStatusList
        {
            get
            {
                var dictionary = new Dictionary<string, string>
                {
                    {"-1", "اطلاعات ارسال شده ناقص است."},
                    {"-2", "IP و يا مرچنت كد پذيرنده صحيح نيست."},
                    {"-3", "با توجه به محدوديت هاي شاپرك امكان پرداخت با رقم درخواست شده ميسر نمي باشد."},
                    {"-4", "سطح تاييد پذيرنده پايين تر از سطح نقره اي است."},
                    {"-11", "درخواست مورد نظر يافت نشد."},
                    {"-12", "امكان ويرايش درخواست ميسر نمي باشد."},
                    {"-21", "هيچ نوع عمليات مالي براي اين تراكنش يافت نشد."},
                    {"-22", "تراكنش نا موفق ميباشد."},
                    {"-33", "رقم تراكنش با رقم پرداخت شده مطابقت ندارد."},
                    {"-34", "سقف تقسيم تراكنش از لحاظ تعداد يا رقم عبور نموده است"},
                    {"-40", "اجازه دسترسي به متد مربوطه وجود ندارد."},
                    {"-41", "غيرمعتبر ميباشد. AdditionalData اطلاعات ارسال شده مربوط به"},
                    {"-42", "مدت زمان معتبر طول عمر شناسه پرداخت بايد بين 30 دقيه تا 45 روز مي باشد."},
                    {"-54", "درخواست مورد نظر آرشيو شده است."},
                    {"100", "عمليات با موفقيت انجام گرديده است."},
                    {"101", "تراكنش انجام شده است. PaymentVerification عمليات پرداخت موفق بوده و قبلا"},
                    {"404", "انصراف توسط کاربر یا عدم موفقیت پرداخت"}
                };
                return dictionary;
            }
        }
    }
}
