using System;

namespace OnlyOtp
{
    public class OtpOptions
    {
        private OtpContents _otpContents;
        private int _length;
        public int Length
        {
            get
            {
                return _length <= 0 ? DefaultOtpLength : _length;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Length should be a positive number");
                }

                if (value > 10)
                {
                    throw new ArgumentException("Only 10 digit number are supported");
                }
                else
                {
                    _length = value;
                }
            }
        }
        public bool ShouldBeCryptographicallyStrong { get; set; } = DefaultCryptographicOption;
        public OtpContents OtpContents
        {
            get
            {
                return _otpContents;
            }

            set
            {
                if (value.HasFlag(OtpContents.Alphabets) || value.HasFlag(OtpContents.SpecialCharacters))
                {
                    throw new NotImplementedException();
                }
                _otpContents = value;
            }
        }

        public const int DefaultOtpLength = 6;
        public const bool DefaultCryptographicOption = false;
        public const OtpContents DefaultOtpContents = OtpContents.Number;
    }
}
