using Microsoft.EntityFrameworkCore;
using OnlyOtp.Storage.Abstractions;
using System;

namespace OnlyOtp.Storage.SqlServer
{
    public class SqlServerOtpStorage : IOtpStorage
    {
        private readonly OnlyOtpContext _context;       
        public SqlServerOtpStorage(OnlyOtpContext context)
        {
            _context = context;
        }
        public string GetOtp(string otpVerificationToken)
        {
            otpVerificationToken = otpVerificationToken?.Trim();
            if (string.IsNullOrEmpty(otpVerificationToken))
            {
                throw new ArgumentException(nameof(otpVerificationToken));
            }
            var otp = _context.Otps.Find(otpVerificationToken);
            if (otp != null)
                return otp.Value;
            else
                return null;
        }

        public string PutOtp(string otp)
        {
            otp = otp?.Trim();
            if (string.IsNullOrEmpty(otp))
            {
                throw new ArgumentException(nameof(otp));
            }
            var token = Guid.NewGuid().ToString();
            _context.Otps.Add(new Otp { Id = token, Value = otp });
            _context.SaveChanges();
            return token;
        }
    }
}
