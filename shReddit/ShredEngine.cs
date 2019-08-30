using System;
using System.Collections.Generic;
using System.Linq;
using RedditSharp.Things;

namespace shReddit
{
    public class ShredEngine
    {
        public int ShreddedPosts;
        public int ShreddedComments;
        public int ErrorCount;

        private string GenerateRandomString()
        {                        
            return Guid.NewGuid().ToString("N").Substring(0, 32);
        }


        public bool Shred(AuthenticatedUser redditUser, ShredCommand sc)
        {
            var postsToShred = redditUser.Posts.Take(sc.DeletePostsQty).ToList();
            var commentsToShred = redditUser.Comments.Take(sc.DeleteCommentsQty).ToList();            
            
            ShreddedPosts = 0;
            ShreddedComments = 0;

            foreach (var post in postsToShred)
            {
                if (sc.WriteGarbage)
                {
                    for (var i = 0; i < sc.NumberOfPasses; i++)
                    {
                        if (!post.IsSelfPost) continue;
                        try
                        {
                            post.EditText(GenerateRandomString());
                        }
                        catch (Exception ex)
                        {
                            var msg = ex.Message;
                            ErrorCount++;
                        }
                    }
                }

                if (sc.DeletePostsQty == 0) continue;
                try
                {
                    post.Del();
                    ShreddedPosts++;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    ErrorCount++;
                }
            }



            foreach (var comment in commentsToShred)
            {
                if (sc.WriteGarbage)
                {
                    for (var i = 0; i < sc.NumberOfPasses; i++)
                    {
                        try
                        {
                            comment.EditText(GenerateRandomString());
                        }
                        catch (Exception ex)
                        {
                            var msg = ex.Message;
                            ErrorCount++;
                        }
                    }
                }

                if (sc.DeleteCommentsQty == 0) continue;
                try
                {
                    comment.Del();
                    ShreddedComments++;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    ErrorCount++;
                }
            }

            return true;
        }
    }
}
