using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlyOtp.Storage.SqlServer
{
    public class OnlyOtpContext: DbContext
    {
        public OnlyOtpContext(DbContextOptions<OnlyOtpContext> options)
            : base(options)
        {

        }
        internal DbSet<Otp> Otps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Otp>().ToTable("Otp", "OnlyOtp");
        }

    }
}
