using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            var writeGarbage = WriteGarbage.SelectedValue.ToString() == "Yes";
            int numberOfPasses;
            Int32.TryParse(PassNumber.SelectedValue.ToString(), out numberOfPasses);
            var deletePosts = DeletePosts.SelectedValue.ToString() == "Yes";
            var deleteComments = DeleteComments.SelectedValue.ToString() == "Yes";

            OutputTextBlock.Text = ShredEngine.Shred(_redditUser, writeGarbage, numberOfPasses, deletePosts, deleteComments) ? "Shredding completed successfully!" : "Shredding failed.";

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
            _reddit = new Reddit(WebAgent.RateLimitMode.Burst);
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password)) return;

            _redditUser = _reddit.LogIn(userName, password);

            if (_redditUser == null) return;

            var posts = _redditUser.Posts;
            var comments = _redditUser.Comments;

            var postCount = posts.Count();
            var commentCount = comments.Count();

            OutputTextBlock.Text =
                String.Format("Logged in as {0}. You have {1} posts and {2} comments waiting to be shredded.", _redditUser.Name, postCount, commentCount);
            ShredButton.IsEnabled = true;
        }
    }


}
