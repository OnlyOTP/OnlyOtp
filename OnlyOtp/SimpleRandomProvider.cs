using System;
using System.Linq;

namespace OnlyOtp
{
    internal class SimpleRandomProvider : IRandomProvider
    {
        private static readonly Random _random = new Random();
        public string GetRandom(int length, char[] charset)
        {
            var chars = new string(charset);
            bool shouldUseLinq = false;
            if (shouldUseLinq)
            {
                return new string(Enumerable.Repeat(charset, length).Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            else
            {
                var min = (int)Math.Pow(10, length - 1);
                var max = (int)Math.Pow(10, length) - 1;
                return _random.Next(min, max).ToString();

            }
        }
    }
}
