using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlyOtp.Tests
{
    [TestClass]    
    public class OtpOptionsTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void OtpContents_Should_OnlyBeRetricted_ToAlphabetsAsOfNow()
        {
            var otpOptions1 = new OtpOptions { OtpContents = OtpContents.Alphabets };
            var otpOptions2 = new OtpOptions { OtpContents = OtpContents.SpecialCharacters};            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OtpContents_Should_ThrowArgumetException_WhenLengthIsNegative()
        {
            var otpOptions = new OtpOptions { Length = -1 };            
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OtpContents_Should_ThrowArgumetException_WhenLengthIsMoreThan10()
        {
            var randomLength = new Random().Next(11, int.MaxValue);
            var otpOptions = new OtpOptions { Length = randomLength };
        }
    }
}
