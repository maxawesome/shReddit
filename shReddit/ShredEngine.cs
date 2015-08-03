using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RedditSharp.Things;


namespace shReddit
{
    public class ShredEngine
    {
        public string GenerateRandomString(int length)
        {
            var randBuffer = new byte[length];
            RandomNumberGenerator.Create().GetBytes(randBuffer);
            return Convert.ToBase64String(randBuffer).Remove(length);
        }


        public static bool Shred(AuthenticatedUser redditUser, bool writeGarbage, int numberOfPasses, bool deletePosts, bool deleteComments)
        {
         
            
            return false;
        }
    }
}
