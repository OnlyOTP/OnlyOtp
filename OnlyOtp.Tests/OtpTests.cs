using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlyOtp.Storage.InMemory;
using System;

namespace OnlyOtp.Tests
{
    [TestClass]
    public class OtpTests
    {
        private IOtp _otpProvider;
        public OtpTests()
        {
            _otpProvider = new Otp(new InMemoryOtpStorage());
        }

        [TestMethod]
        public void DefaultOtp_Should_NotDependOnAnyProvider()
        {
            _otpProvider = new Otp();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OtpStorageShouldNotBeNull()
        {
            var otp = new Otp(null);
        }
        [TestMethod]
        public void GenerateOtp_Should_Return6DigitNumericOtp_When_NoOptionsArePassed()
        {
            string testOtp = _otpProvider.GenerateOtp();
            foreach (var test in testOtp.ToCharArray())
            {
                Assert.IsTrue(char.IsDigit(test));
            }
        }

        [TestMethod]
        public void GenerateOtp_Should_ReturnNumbers_When_NumberOptionIsPassed()
        {
            var number = new Random().Next(1, 10);
            string testOtp = _otpProvider.GenerateOtp(new OtpOptions { Length = number, OtpContents = OtpContents.Number, ShouldBeCryptographicallyStrong = false });
            Assert.AreEqual(number, testOtp.Length);
            foreach (var test in testOtp.ToCharArray())
            {
                Assert.IsTrue(char.IsDigit(test));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateAndStoreOtp_Should_ThrowArgumetNullException_When_NoOtpStorageIsPassed()
        {
            _otpProvider = new Otp();
            (string testOtp, string token) = _otpProvider.GenerateAndStoreOtp();
        }

        [TestMethod]
        public void GenerateAndStoreOtp_Should_Return6DigitNumericOtpAndNotNullToken_When_NoOptionsArePassed()
        {
            (string testOtp, string token) = _otpProvider.GenerateAndStoreOtp();
            Assert.IsNotNull(token);
            foreach (var test in testOtp.ToCharArray())
            {
                Assert.IsTrue(char.IsDigit(test));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOtpMatched_Should_ThrowArgumentNullException_When_NoOtpStorageIsPassed()
        {
            _otpProvider = new Otp();
            _otpProvider.IsOtpMached("123", "sdasfasdf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOtpMatched_Should_ThrowArgumentException_WhenOtpIsNull()
        {
            _otpProvider.IsOtpMached(null, "sdasfasdf");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOtpMatched_Should_ThrowArgumentException_WhenOtpIsEmpty()
        {
            var otp = _otpProvider.IsOtpMached(string.Empty, "sdasfasdf");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOtpMatched_Should_ThrowArgumentException_WhenOtpIsWhitespaces()
        {
            var otp = _otpProvider.IsOtpMached("    ", "sdasfasdf");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOtpMatched_Should_ThrowArgumentException_WhenOtpVerificationTokenIsNull()
        {
            var otp = _otpProvider.IsOtpMached("asdfasfd", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOtpMatched_Should_ThrowArgumentException_WhenOtpVerificationTokenIsEmpty()
        {
            var otp = _otpProvider.IsOtpMached("asdfasfd", string.Empty);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsOtpMatched_Should_ThrowArgumentException_WhenOtpVerificationTokenIsWhitespaces()
        {
            var otp = _otpProvider.IsOtpMached("asdfasdf", "  ");
        }

        [TestMethod]
        public void GenerateStoreAndMatching_Should_BeSuccessfull_WhenDefaultOptionsArePassed()
        {
            (string otp, string token) = _otpProvider.GenerateAndStoreOtp();
            Assert.IsTrue(_otpProvider.IsOtpMached(otp, token));
        }

        [TestMethod]
        public void GenerateOtp_Should_NotBeNegative_WhenCyptoFlagIsTrue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var otp = _otpProvider.GenerateOtp(new OtpOptions { ShouldBeCryptographicallyStrong = true });
                Assert.IsTrue(long.Parse(otp) > 0);
            }

        }

        [TestMethod]
        public void GenerateStoreAndMatching_Should_BeSuccessfull_WhenRandomLengthWithCryptoCheckedArePassed()
        {
            var length = new Random().Next(1, 10);
            (string otp, string token) = _otpProvider.GenerateAndStoreOtp(new OtpOptions { Length = length, ShouldBeCryptographicallyStrong = true });
            foreach (var test in otp.ToCharArray())
            {
                Assert.IsTrue(char.IsDigit(test));
            }
            Assert.IsTrue(_otpProvider.IsOtpMached(otp, token));
        }

        [TestMethod]
        public void GenerateOtp_Should_NotReturnOutOfRangeValues_WithoutCrypto()
        {
            var random = new Random();
            for (int i = 0; i < (int)Math.Pow(10, 6); i++)
            {
                var length = random.Next(1, 10);
                var otp = _otpProvider.GenerateOtp(new OtpOptions { Length = length });
                Assert.IsTrue(otp.Length == length);
            }

        }

        [TestMethod]
        public void GenerateOtp_Should_NotReturnOutOfRangeValues_WithCrypto()
        {
            var random = new Random();
            for (int i = 0; i < (int)Math.Pow(10, 6); i++)
            {
                var length = random.Next(1, 10);
                var otp = _otpProvider.GenerateOtp(new OtpOptions { Length = length, ShouldBeCryptographicallyStrong = true });
                Assert.IsTrue(otp.Length == length);
            }

        }

        [TestMethod]
        public void GenerateStoreAndMatching_Should_Fail_WhenOtpIsExpired()
        {
            (string otp, string token) = _otpProvider.GenerateAndStoreOtp(new OtpOptions { Expiry = DateTime.Now.AddMinutes(-1) });
            Assert.IsFalse(_otpProvider.IsOtpMached(otp, token));
        }

        [TestMethod]
        public void GenerateStoreAndMatching_Should_Fail_WhenOtpIsNotExpired()
        {
            (string otp, string token) = _otpProvider.GenerateAndStoreOtp(new OtpOptions { Expiry = DateTime.Now.AddMinutes(5) });
            Assert.IsTrue(_otpProvider.IsOtpMached(otp, token));
        }

        [TestMethod]
        public void Remove_Should_RemoveOtp_When_Called()
        {
            (string Otp, string OtpVerificationToken) = _otpProvider.GenerateAndStoreOtp();
            _otpProvider.IsOtpMached(Otp, OtpVerificationToken);
            Assert.IsTrue(_otpProvider.IsOtpMached(Otp, OtpVerificationToken));
            _otpProvider.Remove(OtpVerificationToken);
            Assert.IsFalse(_otpProvider.IsOtpMached(Otp, OtpVerificationToken));
        }
    }
}
