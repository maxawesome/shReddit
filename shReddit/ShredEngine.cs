using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace shReddit
{
    class ShredEngine
    {
        public string GenerateRandomString(int length)
        {
            var randBuffer = new byte[length];
            RandomNumberGenerator.Create().GetBytes(randBuffer);
            return Convert.ToBase64String(randBuffer).Remove(length);
        }

    }
}
