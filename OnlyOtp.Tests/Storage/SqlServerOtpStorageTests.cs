using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlyOtp.Storage.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlyOtp.Tests.Storage
{
    [TestClass]
    public class SqlServerOtpStorageTests
    {
        private readonly SqlServerOtpStorage _storage;

        public SqlServerOtpStorageTests()
        {
            var options = new DbContextOptionsBuilder<OnlyOtpContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            var context = new OnlyOtpContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            _storage = new SqlServerOtpStorage(context);

        }
        #region GetOtp arguments check
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOtp_Should_ThrowArgumentException_When_NullOtpVerificationTokenPassed()
        {
            _ = _storage.GetOtp(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOtp_Should_ThrowArgumentException_When_EmptyOtpVerificationTokenPassed()
        {           
            _ = _storage.GetOtp(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOtp_Should_ThrowArgumentException_When_WhiteSpaceOtpVerificationTokenPassed()
        {            
            _ = _storage.GetOtp("   ");
        }

        #endregion
        #region PutOtp arguments Check
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutOtp_Should_ThrowArgumentException_When_NullOtpPassed()
        {
          
          _ = _storage.PutOtp(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutOtp_Should_ThrowArgumentException_When_EmptyOtpPassed()
        {
           _ = _storage.PutOtp(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutOtp_Should_ThrowArgumentException_When_WhiteSpaceOtpPassed()
        {
            //Arrange
            _storage.PutOtp("   ");
        }
        #endregion

        [TestMethod]
        public void PutOtp_Should_StoreOtpsAndReturnNotNullToken_When_NotNullOtpVerificationTokenPassed()
        {
            var token = _storage.PutOtp("12345");

            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void PutOtpAndGetOtp_Should_StoreAndReturnCorrectOtp_When_NotNullOtpPassed()
        {
            //Arrange           
            var otp = "12345";
            var token = _storage.PutOtp(otp);

            var otpUnderTest = _storage.GetOtp(token)?.Otp;

            Assert.AreEqual(otpUnderTest, otp);
        }
    }
}
