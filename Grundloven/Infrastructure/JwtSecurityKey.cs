using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Grundloven.Infrastructure
{
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(StretchKey(secret));
        }

        private static byte[] StretchKey(string key)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA512 with 10_000 iterations)
            return KeyDerivation.Pbkdf2(
                password: key,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10_000,
                numBytesRequested: 256 / 8);
        }
    }
}
