using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlyOtp.Storage.Abstractions;
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

        [TestMethod]
        public void Clear_Should_RemoveAllEntries_When_Called()
        {
            IOtpStorage otpStorage = new InMemoryOtpStorage();
            string otp = "12345";
            string token = otpStorage.PutOtp(otp);

            otpStorage.Clear();
            string otpFromStorageAfterClear = otpStorage.GetOtp(token)?.Otp;

            Assert.IsNull(otpFromStorageAfterClear);

        }
        [TestMethod]
        public void Remove_Should_RemoveSpecificOtp_When_Called()
        {
            IOtpStorage otpStorage = new InMemoryOtpStorage();
            string otp = "12345";
            string token = otpStorage.PutOtp(otp);

            otpStorage.Remove(token);
            string otpFromStorageAfterClear = otpStorage.GetOtp(token)?.Otp;

            Assert.IsNull(otpFromStorageAfterClear);

        }

        [TestMethod]
        public void Remove_ShouldNot_RemoveOtherOtps_When_Called()
        {
            IOtpStorage otpStorage = new InMemoryOtpStorage();
            string otp1 = "12345";
            string token1 = otpStorage.PutOtp(otp1);

            string otp2 = "23456";
            string token2 = otpStorage.PutOtp(otp2);

            otpStorage.Remove(token1);
            string otp1FromStorageAfterClear = otpStorage.GetOtp(token1)?.Otp;
            Assert.IsNull(otp1FromStorageAfterClear);
            string otp2FromStorageAfterClear = otpStorage.GetOtp(token2)?.Otp;
            Assert.AreEqual(otp2, otp2FromStorageAfterClear);

        }
    }
}
