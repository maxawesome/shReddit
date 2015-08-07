using System;
using System.Collections.Generic;
using System.Linq;
using RedditSharp.Things;

namespace shReddit
{
    public static class ShredEngine
    {
        private static string GenerateRandomString()
        {
            const int length = 32;
            var randomText = Guid.NewGuid().ToString("N").Substring(0, length);
            return randomText;
        }


        public static bool Shred(AuthenticatedUser redditUser, bool writeGarbage, int numberOfPasses, int deletePostsQty, int deleteCommentsQty)
        {
            var encounteredErrors = false;

            var postsToShred = redditUser.Posts.Take(deletePostsQty).ToList();
            var commentsToShred = redditUser.Comments.Take(deleteCommentsQty).ToList();

            var shreddedPosts = new List<Post>();
            var shreddedComments = new List<Comment>();

            foreach (var post in postsToShred)
            {
                if (writeGarbage)
                {
                    for (var i = 0; i < numberOfPasses; i++)
                    {
                        if (!post.IsSelfPost) continue;
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

                if (deletePostsQty == 0) continue;
                try
                {
                    post.Del();
                    shreddedPosts.Add(post);
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    encounteredErrors = true;
                }
            }



            foreach (var comment in commentsToShred)
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

                if (deleteCommentsQty == 0) continue;
                try
                {
                    comment.Del(); //This won't work until RedditSharp implements the Del method, which it doesn't yet.
                    shreddedComments.Add(comment);
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    encounteredErrors = true;
                }
            }

            var postShredCount = shreddedPosts.Count;
            var commentShredCount = shreddedComments.Count;

            return encounteredErrors;
        }
    }
}
