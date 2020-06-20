namespace AuctionSystem.Infrastructure.Identity
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using static System.String;

    internal sealed class SecurityToken
    {
        private readonly byte[] data;

        public SecurityToken(byte[] data)
        {
            this.data = (byte[])data.Clone();
        }

        internal byte[] GetDataNoClone() => this.data;
    }

    internal static class Rfc6238AuthenticationService
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan TimeSpan = TimeSpan.FromMinutes(3);
        private static readonly Encoding Encoding = new UTF8Encoding(false, true);

        private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timeSpan, string modifier, int numberOfDigits = 6)
        {
            // # of 0's = length of pin
            //const int mod = 1000000;
            var mod = (int)Math.Pow(10, numberOfDigits);

            // See https://tools.ietf.org/html/rfc4226
            // We can add an optional modifier
            var timeSpanAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timeSpan));
            var hash = hashAlgorithm.ComputeHash(ApplyModifier(timeSpanAsBytes, modifier));

            // Generate DT string
            var offset = hash[^1] & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            var binaryCode = (hash[offset] & 0x7f) << 24
                             | (hash[offset + 1] & 0xff) << 16
                             | (hash[offset + 2] & 0xff) << 8
                             | (hash[offset + 3] & 0xff);

            var code =  binaryCode % mod;
            return code;
        }

        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (IsNullOrEmpty(modifier))
            {
                return input;
            }

            var modifierBytes = Encoding.GetBytes(modifier);
            var combined = new byte[checked(input.Length + modifierBytes.Length)];
            Buffer.BlockCopy(input, 0, combined, 0, input.Length);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);
            return combined;
        }

        // More info: https://tools.ietf.org/html/rfc6238#section-4
        private static ulong GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - UnixEpoch;
            return (ulong)(delta.Ticks / TimeSpan.Ticks);
        }

        public static int GenerateCode(SecurityToken securityToken, string modifier = null, int numberOfDigits = 6)
        {
           
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than 90 seconds in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using var hashAlgorithm = new HMACSHA1(securityToken.GetDataNoClone());
            var code =  ComputeTotp(hashAlgorithm, currentTimeStep, modifier, numberOfDigits);
            return code;
        }

        public static bool ValidateCode(SecurityToken securityToken, int code, string modifier = null, int numberOfDigits = 6)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than 90 seconds in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using var hashAlgorithm = new HMACSHA1(securityToken.GetDataNoClone());
            for (var i = -2; i <= 2; i++)
            {
                var computedTotp = ComputeTotp(hashAlgorithm, (ulong)((long)currentTimeStep + i), modifier, numberOfDigits);
                if (computedTotp == code)
                {
                    return true;
                }
            }

            // No match
            return false;
        }
    }
}