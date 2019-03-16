using OnlyOtp.Storage;
using OnlyOtp.Storage.Abstractions;
//using OnlyOtp.Storage.InMemory;
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
            if (otpStorage == null)
                throw new ArgumentNullException(nameof(otpStorage));
            _otpStorage = otpStorage;
        }

        public string GenerateOtp()
        {
            return GenerateOtp(new OtpOptions { Length = OtpOptions.DefaultOtpLength, ShouldBeCryptographicallyStrong = OtpOptions.DefaultCryptographicOption });
        }
        public string GenerateOtp(OtpOptions otpOptions)
        {
            return GenerateOtpInternal(otpOptions);
        }

        public (string Otp, string OtpVerificationToken) GenerateAndStoreOtp()
        {
            return GenerateAndStoreOtp(new OtpOptions { Length = OtpOptions.DefaultOtpLength, ShouldBeCryptographicallyStrong = OtpOptions.DefaultCryptographicOption });
        }
        public (string Otp, string OtpVerificationToken) GenerateAndStoreOtp(OtpOptions otpOptions)
        {
            var otp = GenerateOtp(otpOptions);
            if (_otpStorage == null)
                throw new ArgumentNullException(nameof(_otpStorage), $"No OTP Storage provided for storing OTP");
            var token = _otpStorage.PutOtp(otp);
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
         
            if(_otpStorage == null)
            {
                throw new ArgumentNullException(nameof(_otpStorage), "No OTP Storage provided for retreiving OTP.");
            }

            return otpUnderTest.Equals(_otpStorage.GetOtp(otpVerificationToken));
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
    }
}
