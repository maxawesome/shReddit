using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using RedditSharp;
using RedditSharp.Things;

namespace shReddit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Reddit _reddit;
        private static AuthenticatedUser _redditUser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShredThreadProc(object stateInfo)
        {
            var sc = (ShredCommand)stateInfo;
            var engine = new ShredEngine();
            var result = engine.Shred(_redditUser, sc);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<bool>(UpdateUIThreadWithShredResult), result);
        }

        private void UpdateUIThreadWithShredResult(bool result)
        {
            OutputTextBlock.Text += result ? $"\r\n{DateTime.Now} | Shredding failed." : $"\r\n{DateTime.Now} | Shredding completed successfully!";
            ToggleShredImage(false);
            ShredButton.IsEnabled = true;
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
            ShredButton.IsEnabled = false;

            var writeGarbage = WriteGarbage.Text == "Yes";
            int numberOfPasses;
            int.TryParse(PassNumber.Text, out numberOfPasses);
            var deletePostsQty = int.Parse(DeletePostsQuantity.Text);
            var deleteCommentsQty = int.Parse(DeleteCommentsQuantity.Text);

            var sc = new ShredCommand(writeGarbage, numberOfPasses, deletePostsQty, deleteCommentsQty);

            ThreadPool.QueueUserWorkItem(ShredThreadProc, sc);
            OutputTextBlock.Text += $"\r\n{DateTime.Now} | Shredding in progress.";

            Thread.Sleep(1000);
        }

        private void ToggleShredImage(bool shredding)
        {
            LogoImage.Source = shredding ? new BitmapImage(new Uri("Images/shreddit_on.png", UriKind.Relative)) : new BitmapImage(new Uri("Images/shreddit_off.png", UriKind.Relative));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBlock.Text += $"\r\n{DateTime.Now} | Attempting to login as {UserNameText.Text}.";
            ProcessLogin(UserNameText.Text, PasswordText.Password);
        }

        private void ProcessLogin(string username, string password)
        {
            _reddit = new Reddit(WebAgent.RateLimitMode.Pace);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;

            try
            {
                _redditUser = _reddit.LogIn(username, password);
                OutputTextBlock.Text += $"\r\n{DateTime.Now} | Logged in as {username}.";
            }
            catch (Exception ex)
            {
                OutputTextBlock.Text += $"\r\n{DateTime.Now} | Login failed. Exception encountered: {ex.Message}.";
            }

            if (_redditUser == null) return;
            LoginButton.IsEnabled = false;


            var deletePostsQty = int.Parse(DeletePostsQuantity.Text);
            var deleteCommentsQty = int.Parse(DeleteCommentsQuantity.Text);

            OutputTextBlock.Text += $"\r\n{DateTime.Now} | Calculating Post and Comment counts.";
            var posts = _redditUser.Posts.Take(deletePostsQty).ToList();
            var comments = _redditUser.Comments.Take(deleteCommentsQty).ToList();

            OutputTextBlock.Text +=
                $"\r\n{DateTime.Now} | More than {posts.Count} Posts and more than {comments.Count} Comments waiting to be shredded.";
            ShredButton.IsEnabled = true;

        }
    }


}
