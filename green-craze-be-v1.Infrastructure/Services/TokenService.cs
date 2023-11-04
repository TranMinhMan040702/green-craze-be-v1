using green_craze_be_v1.Application.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private static readonly ThreadLocal<System.Security.Cryptography.RandomNumberGenerator> crng = new(System.Security.Cryptography.RandomNumberGenerator.Create);
        private static readonly ThreadLocal<byte[]> bytes = new(() => new byte[sizeof(int)]);

        private static int NextInt()
        {
            crng.Value.GetBytes(bytes.Value);
            return BitConverter.ToInt32(bytes.Value, 0) & int.MaxValue;
        }

        public string GenerateOTP(int digitNumber = 6)
        {
            long number = long.Parse("1".PadRight(digitNumber + 1, '0'));
            return (NextInt() % number).ToString(string.Concat(Enumerable.Repeat("0", digitNumber)));
        }
    }
}