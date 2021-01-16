using System;
using System.ComponentModel.DataAnnotations;

namespace OnlyOtp.Storage.SqlServer
{
    public class Otp
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
    }
}