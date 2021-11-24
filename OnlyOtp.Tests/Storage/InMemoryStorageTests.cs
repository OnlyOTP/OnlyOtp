using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlyOtp.Storage.InMemory;
using System;

namespace OnlyOtp.Tests.Storage
{
    [TestClass]
    public class InMemoryStorageTests
    {
        #region GetOtp arguments check
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOtp_Should_ThrowArgumentException_When_NullOtpVerificationTokenPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();
            var token = otpStorage.GetOtp(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOtp_Should_ThrowArgumentException_When_EmptyOtpVerificationTokenPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();
            var token = otpStorage.GetOtp(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOtp_Should_ThrowArgumentException_When_WhiteSpaceOtpVerificationTokenPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();
            var token = otpStorage.GetOtp("   ");
        }

        #endregion
        #region PutOtp arguments Check
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutOtp_Should_ThrowArgumentException_When_NullOtpPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();
            var token = otpStorage.PutOtp(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutOtp_Should_ThrowArgumentException_When_EmptyOtpPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();
            var token = otpStorage.PutOtp(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutOtp_Should_ThrowArgumentException_When_WhiteSpaceOtpPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();
            var token = otpStorage.PutOtp("   ");
        }
        #endregion

        [TestMethod]
        public void PutOtp_Should_StoreOtpsAndReturnNotNullToken_When_NotNullOtpVerificationTokenPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();

            var token = otpStorage.PutOtp("12345");

            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void PutOtpAndGetOtp_Should_StoreAndReturnCorrectOtp_When_NotNullOtpPassed()
        {
            //Arrange
            var otpStorage = new InMemoryOtpStorage();

            var otp = "12345";
            var token = otpStorage.PutOtp(otp);

            var otpUnderTest = otpStorage.GetOtp(token)?.Otp;

            Assert.AreEqual(otpUnderTest, otp);
        }

    }
}
