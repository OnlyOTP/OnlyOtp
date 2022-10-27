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
        public OtpEntry GetOtp(string otpVerificationToken)
        {
            otpVerificationToken = otpVerificationToken?.Trim();
            if (string.IsNullOrEmpty(otpVerificationToken))
            {
                throw new ArgumentException(nameof(otpVerificationToken));
            }
            var otp = _context.Otps.Find(otpVerificationToken);
            if (otp != null)
            {
                return new OtpEntry
                {
                    Expiry = otp.Expiry,
                    Otp = otp.Value
                };
            }
            else
                return null;
        }

        public string PutOtp(string otp, DateTime? expiry = null)
        {
            otp = otp?.Trim();
            if (string.IsNullOrEmpty(otp))
            {
                throw new ArgumentException(nameof(otp));
            }
            var token = Guid.NewGuid().ToString();
            _context.Otps.Add(new Otp { Id = token, Value = otp, Expiry = expiry });
            _context.SaveChanges();
            return token;
        }
        public void Clear()
        {
            throw new NotImplementedException("Removing all OTPs is not supported yet. Please remove a single OTP using Remove method.");
        }

        public void Remove(string otpVerificationToken)
        {
            _context.Otps.Remove(_context.Otps.Find(otpVerificationToken));
            _context.SaveChanges();
        }
    }
}
