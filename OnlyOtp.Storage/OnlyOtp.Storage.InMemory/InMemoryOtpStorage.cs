using OnlyOtp.Storage.Abstractions;
using System;
using System.Collections.Concurrent;

namespace OnlyOtp.Storage.InMemory
{
    public class InMemoryOtpStorage : IOtpStorage
    {
        private static readonly ConcurrentDictionary<Guid, OtpEntry> _otpStorage = new ConcurrentDictionary<Guid, OtpEntry>();
        public string PutOtp(string otp, DateTime? expiry = null)
        {
            otp = otp?.Trim();
            if (string.IsNullOrEmpty(otp))
            {
                throw new ArgumentException();
            }

            var token = Guid.NewGuid();
            if (_otpStorage.TryAdd(token, new OtpEntry { Otp = otp, Expiry = expiry }))
            {
                return token.ToString();
            }
            else
            {
                return null;
            }
        }

        public OtpEntry GetOtp(string otpVerificationToken)
        {
            otpVerificationToken = otpVerificationToken?.Trim();
            if (string.IsNullOrEmpty(otpVerificationToken))
            {
                throw new ArgumentException();
            }

            if (!Guid.TryParse(otpVerificationToken, out Guid token))
            {
                //throw new ArgumentException($"Invalid {nameof(otpVerificationToken)}");
                return null;
            }
            if (!_otpStorage.TryGetValue(token, out OtpEntry otp))
            {
                //throw new ArgumentException($"No OTP found corresponding to {otpVerificationToken}", nameof(otpVerificationToken));
                return null;
            }
            return otp;
        }

    }
}
