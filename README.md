<p align="center">
  <img src="https://raw.githubusercontent.com/OnlyOTP/OnlyOtpAssets/master/images/facebook_profile_image.png" alt="OnlyOTP Logo" width="200" />
</p>

# An OTP generation, storage, and validation solution. 

## 1. Install Nuget Package.
Install Nuget Package https://www.nuget.org/packages/OnlyOTP

## 2. Generate OTP

````CSharp
var otpProvieder = new Otp();
//Generates 6 digit OTP by default
var otp = otpProvider.GenerateOtp();
````

## 3. Generate OTP with specified length
````CSharp
otpProvider.GenerateOtp(new OtpOptions { Length = 10 })
````
## TODO: Advance Options - Storage, and Validation
