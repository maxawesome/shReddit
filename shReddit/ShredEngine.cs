using System;
using RedditSharp.Things;

namespace shReddit
{
    public class ShredEngine
    {
        private static string GenerateRandomString()
        {
            var length = 32;
            var randomText = Guid.NewGuid().ToString("N").Substring(0, length);
            return randomText;
        }


        public static bool Shred(AuthenticatedUser redditUser, bool writeGarbage, int numberOfPasses, bool deletePosts, bool deleteComments)
        {
            var encounteredErrors = false;

            foreach (var post in redditUser.Posts)
            {
                if (writeGarbage)
                {
                    for (var i = 0; i < numberOfPasses; i++)
                    {
                        if (post.IsSelfPost)
                        {
                            try
                            {
                                post.EditText(GenerateRandomString());
                            }
                            catch (Exception ex)
                            {
                                var msg = ex.Message;
                                encounteredErrors = true;
                            }
                        }
                    }
                }

                if (deletePosts)
                {
                    try
                    {
                        post.Del();
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                        encounteredErrors = true;
                    }
                }
            }



            foreach (var comment in redditUser.Comments)
            {
                if (writeGarbage)
                {
                    for (var i = 0; i < numberOfPasses; i++)
                    {
                        try
                        {
                            comment.EditText(GenerateRandomString());
                        }
                        catch (Exception ex)
                        {
                            var msg = ex.Message;
                            encounteredErrors = true;
                        }
                    }
                }

                if (deleteComments)
                {
                    try
                    {
                        //comment.Del(); //This won't work until RedditSharp implements the Del method, which it doesn't yet.
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                        encounteredErrors = true;
                    }
                }

            }


            return encounteredErrors;
        }
    }
}
