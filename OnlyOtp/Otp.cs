using OnlyOtp.Storage.Abstractions;
using System;
using System.Linq;

namespace OnlyOtp
{
    public class Otp : IOtp
    {
        private readonly IOtpStorage _otpStorage;
        private IRandomProvider _randomProvider;
        public Otp()
        {
        }
        public Otp(IOtpStorage otpStorage)
        {
            _otpStorage = otpStorage ?? throw new ArgumentNullException(nameof(otpStorage));
        }

        public string GenerateOtp()
        {
            return GenerateOtp(new OtpOptions { });
        }
        public string GenerateOtp(OtpOptions otpOptions)
        {
            return GenerateOtpInternal(otpOptions);
        }

        public (string Otp, string OtpVerificationToken) GenerateAndStoreOtp()
        {
            return GenerateAndStoreOtp(new OtpOptions { });
        }
        public (string Otp, string OtpVerificationToken) GenerateAndStoreOtp(OtpOptions otpOptions)
        {
            var otp = GenerateOtp(otpOptions);
            if (_otpStorage == null)
            {
                throw new ArgumentNullException(nameof(_otpStorage), $"No OTP Storage provided for storing OTP");
            }

            var token = _otpStorage.PutOtp(otp, otpOptions.Expiry);
            return (otp, token);
        }
        public bool IsOtpMached(string otpUnderTest, string otpVerificationToken)
        {
            otpUnderTest = otpUnderTest?.Trim();
            otpVerificationToken = otpVerificationToken?.Trim();
            if (string.IsNullOrEmpty(otpUnderTest))
            {
                throw new ArgumentNullException(nameof(otpUnderTest));
            }

            if (string.IsNullOrEmpty(otpVerificationToken))
            {
                throw new ArgumentNullException(nameof(otpVerificationToken));
            }

            if (_otpStorage == null)
            {
                throw new ArgumentNullException(nameof(_otpStorage), "No OTP Storage provided for retreiving OTP.");
            }
            var otpEntryFromStorage = _otpStorage.GetOtp(otpVerificationToken);
            if (otpEntryFromStorage == null)
                return false;
            if (otpEntryFromStorage.Expiry != null && otpEntryFromStorage.Expiry.HasValue && otpEntryFromStorage.Expiry.Value < DateTime.Now)
                return false;
            return otpUnderTest.Equals(otpEntryFromStorage.Otp);
        }

        private string GenerateOtpInternal(OtpOptions otpOptions)
        {
            if (!otpOptions.ShouldBeCryptographicallyStrong)
            {
                _randomProvider = new SimpleRandomProvider();
            }
            else
            {
                _randomProvider = new CryptoRandomProvider();
            }
            char[] charset = new char[] { };
            if (otpOptions.OtpContents.HasFlag(OtpContents.Number))
            {
                charset = Enumerable.Range(0, 9).Select(x => char.Parse(x.ToString())).ToArray();
            }
            return _randomProvider.GetRandom(otpOptions.Length, charset);
        }

        public void Remove(string OtpVerificationToken)
        {
            _otpStorage.Remove(OtpVerificationToken);
        }
    }
}
