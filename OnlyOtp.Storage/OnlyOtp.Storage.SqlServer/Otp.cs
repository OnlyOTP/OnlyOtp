using System;
using System.ComponentModel.DataAnnotations;

namespace OnlyOtp.Storage.SqlServer
{
    internal class Otp
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
        public DateTime? Expiry { get; set; }
    }
}