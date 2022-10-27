using System;
using System.Collections.Generic;
using System.Text;

namespace OnlyOtp
{
    public interface IOtp
    {
        string GenerateOtp();
        string GenerateOtp(OtpOptions otpOptions);        
        (string Otp, string OtpVerificationToken) GenerateAndStoreOtp();
        (string Otp, string OtpVerificationToken) GenerateAndStoreOtp(OtpOptions otpOptions);
        bool IsOtpMached(string otpUnderTest, string OtpVerificationToken);
        void Remove(string OtpVerificationToken);
    }
}
