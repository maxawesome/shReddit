using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using RedditSharp;
using RedditSharp.Things;

namespace shReddit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Reddit _reddit;
        private AuthenticatedUser _redditUser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShredButton_Click(object sender, RoutedEventArgs e)
        {
            if (_redditUser == null)
            {
                MessageBox.Show("You're not logged in. Try again.", "Not logged in!", MessageBoxButton.OK, MessageBoxImage.Error);
                ShredButton.IsEnabled = false;
                return;
            }

            var sure = MessageBox.Show("Are you certain you want to shred your reddit history?", "Shred for real?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (sure != MessageBoxResult.Yes) return;

            sure = MessageBox.Show("Shredding is irreversible. Are you really, really sure?", "No, seriously...", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (sure != MessageBoxResult.Yes) return;

            ToggleShredImage(true);

            var writeGarbage = WriteGarbage.Text == "Yes";
            int numberOfPasses;
            int.TryParse(PassNumber.Text, out numberOfPasses);
            var deletePostsQty = int.Parse(DeletePostsQuantity.Text);
            var deleteCommentsQty = int.Parse(DeleteCommentsQuantity.Text);

            OutputTextBlock.Text = ShredEngine.Shred(_redditUser, writeGarbage, numberOfPasses, deletePostsQty, deleteCommentsQty) ? "Shredding completed successfully!" : "Shredding failed.";

            ToggleShredImage(false);

        }

        private void ToggleShredImage(bool shredding)
        {
            LogoImage.Source = shredding ? new BitmapImage(new Uri("Images/shreddit_on.png", UriKind.Relative)) : new BitmapImage(new Uri("Images/shreddit_off.png", UriKind.Relative));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessLogin(UserNameText.Text, PasswordText.Password);
        }

        private void ProcessLogin(string userName, string password)
        {
            _reddit = new Reddit(WebAgent.RateLimitMode.Pace);

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return;

            _redditUser = _reddit.LogIn(userName, password);

            if (_redditUser == null) return;
            LoginButton.IsEnabled = false;

            var posts = _redditUser.Posts.Take(100).ToList();
            var comments = _redditUser.Comments.Take(100).ToList();

            OutputTextBlock.Text =
                $"Logged in as {_redditUser.Name}. You have >= {posts.Count} posts and >= {comments.Count} comments waiting to be shredded.";
            ShredButton.IsEnabled = true;            
        }
    }


}
