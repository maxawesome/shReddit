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

        private string GenerateRandomString()
        {
            const int length = 32;
            var randomText = Guid.NewGuid().ToString("N").Substring(0, length); //Guids are cool!
            return randomText;
        }


        public bool Shred(AuthenticatedUser redditUser, ShredCommand sc)
        {
            var encounteredErrors = false;

            var postsToShred = redditUser.Posts.Take(sc.DeletePostsQty).ToList();
            var commentsToShred = redditUser.Comments.Take(sc.DeleteCommentsQty).ToList();

            var shreddedPosts = new List<Post>();
            var shreddedComments = new List<Comment>();

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
                            encounteredErrors = true;
                        }
                    }
                }

                if (sc.DeletePostsQty == 0) continue;
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
                            encounteredErrors = true;
                        }
                    }
                }

                if (sc.DeleteCommentsQty == 0) continue;
                try
                {
                    comment.Del();
                    shreddedComments.Add(comment);
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    encounteredErrors = true;
                }
            }

            ShreddedPosts = shreddedPosts.Count;
            ShreddedComments = shreddedComments.Count;

            return encounteredErrors;
        }
    }
}
