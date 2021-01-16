<p align="center">
  <img src="https://raw.githubusercontent.com/OnlyOTP/OnlyOtpAssets/master/images/facebook_profile_image.png" alt="OnlyOTP Logo" width="200" />
</p>

# An OTP generation, storage, and validation solution. 

## 1. Install Nuget Package.
Install Nuget Package [https://www.nuget.org/packages/OnlyOTP](https://www.nuget.org/packages/OnlyOTP)

## 2. Generate OTP

````CSharp
var otpProvider = new Otp();
//Generates 6 digit OTP by default
var otp = otpProvider.GenerateOtp();
````

## Advanced Options

## Generate OTP with specified length

````CSharp
var otpWithSpecifiedLength = otpProvider.GenerateOtp(new OtpOptions { Length = 10 });
````

## Gerenate OTP and Store In-Memory
### Install InMemory OTP Storage Nuget
Install additional Nuget Package [https://www.nuget.org/packages/OnlyOtp.Storage.InMemory](https://www.nuget.org/packages/OnlyOtp.Storage.InMemory)
### Instantiate `Otp` with `InMemoryOtpStorage`

````CSharp
public void SomeMethod()
{
    // Pass InMemoryOtpStorage to Otp Provider.
    var otpProvider = new Otp(new InMemoryOtpStorage());

    // Returns OTP and OTP Verification Token
    // Save this token to retreive generated OTP later

    (string myOtp, string myToken) = otpProvider.GenerateAndStoreOtp();
}
public void SomeOtherMethod(string otpEnteredByUser)
{
    // Match the stored OTP with OTP entered by user
    var isMatched = otpProvider.IsOtpMached(otpEnteredByUser, myToken);
}
````
## Generate OTP and Store in SQL Server

### Install InMemory OTP Storage Nuget
Install additional Nuget Package [https://www.nuget.org/packages/OnlyOtp.Storage.SqlServer](https://www.nuget.org/packages/OnlyOtp.Storage.SqlServer)
### Create table and schema
`SqlServerOtpStorage` uses an `Sql Server` to store OTPs. Use this script to generate necessary table and schema.
````sql
GO
CREATE SCHEMA [OnlyOtp]
GO
CREATE TABLE [OnlyOtp].[Otp](
	[Id] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Otp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)) ON [PRIMARY]
GO


````

### Create `DbContext`
#### In an `ASP.NET Core` application
If you're using `ASP.NET Core`, you can use `Dependency Injection` to set-up `DbContext` used by `OnlyOtp`.
````csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddDbContext<OnlyOtpContext>(options =>
    {        
        options.UseSqlServer(@"Data Source=DATABASE_SERVER_NAME;Initial Catalog=DATABASE_NAME;Integrated Security=True;");
    });
    ...
}
````
And in `Controller` or any other class, inject this `DbContext`:
````csharp
public class HelloController : Controller
{
    private readonly OnlyOtpContext _onlyOtpContext;

    public HelloController(OnlyOtpContext onlyOtpContext)
    {
        _onlyOtpContext = onlyOtpContext;
    }
    
}
````
#### In any other application
You can instantiate the `DbContext` manually.
````csharp
var options = new DbContextOptionsBuilder<OnlyOtpContext>()
                .UseSqlServer(@"Data Source=DATABASE_SERVER_NAME;Initial Catalog=DATABASE_NAME;Integrated Security=True;")
                .Options;
var dbContext = new OnlyOtpContext(options);
````
### Use `Otp` with `SqlServerOtpStorage`
````csharp
public void SomeMethod()
{
    // Pass SqlServerOtpStorage to Otp Provider.
    var otpProvider = new Otp(new SqlServerOtpStorage(_onlyOtpContext));
    // Get OTP token and generated OTP
    (string otp, string token) = otpProvider.GenerateAndStoreOtp();
    //Save this token to retreive generated OTP later
    ...
}
public void SomeOtherActionMethod(string otpEnteredByUser)
{
    ...
    // Match the stored OTP with OTP entered by user
    bool isOtpMatched = otpProvider.IsOtpMached(otpEnteredByUser, token);
    ...
}
````
