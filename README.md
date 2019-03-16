<p align="center">
  <img src="https://raw.githubusercontent.com/OnlyOTP/OnlyOtpAssets/master/images/facebook_profile_image.png" alt="OnlyOTP Logo" width="200" />
</p>

# An OTP generation, storage, and validation solution. 

## 1. Install Nuget Package.
Install Nuget Package https://www.nuget.org/packages/OnlyOTP

## 2. Generate OTP

````CSharp
var otpProvider = new Otp();
//Generates 6 digit OTP by default
var otp = otpProvider.GenerateOtp();
````

## Advanced Options

## 1. Generate OTP with specified length

````CSharp
otpProvider.GenerateOtp(new OtpOptions { Length = 10 })
````

## 2. Gerenate OTP and Store In-Memory
### 2.1  Install InMemory OTP Storage Nuget
Install Nuget Package https://www.nuget.org/packages/OnlyOtp.Storage.InMemory
### 2.2 Instantiate `Otp` with `InMemoryOtpStorage`

````CSharp
var otpProvieder = new Otp(new InMemoryOtpStorage());
//returns Otp and OtpVerificationToken
var otpAndToken = otpProvider.GenerateAndStoreOtp();
//Check if OTP matched with stored against OtpVerificationToken
var isMatched = otpProvider.IsOtpMached(otpAndToken.Otp, otpAndToken.OtpVerificationToken)
````
## TODO: BYOP - Bring Your Own Provider
