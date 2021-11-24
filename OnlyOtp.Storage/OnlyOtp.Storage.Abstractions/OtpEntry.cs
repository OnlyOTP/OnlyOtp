using System;
using System.Collections.Generic;
using System.Text;

namespace OnlyOtp.Storage.Abstractions
{
    public class OtpEntry
    {
        public string Otp { get; set; }
        public DateTime? Expiry { get; set; }
    }
}
