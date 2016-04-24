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
        private ShredEngine _shredEngine;
        private bool _shredResult;
        private static System.Timers.Timer _intervalTimer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShredThreadProc(object stateInfo)
        {
            _intervalTimer = new System.Timers.Timer(10000);
            _intervalTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _intervalTimer.Start();

            var sc = (ShredCommand)stateInfo;
            _shredEngine = new ShredEngine();
            _shredResult = _shredEngine.Shred(_redditUser, sc);

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<bool>(UpdateUIThreadWithShredResult), _shredResult);
            _intervalTimer.Stop();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<int, int>(UpdateUIThreadWithProgress), _shredEngine.ShreddedComments, _shredEngine.ShreddedPosts);
            if (_shredResult) _intervalTimer.Stop();
        }

        private void UpdateUIThreadWithProgress(int commentQty, int postQty)
        {
            OutputTextBlock.Text += $"\r\n{DateTime.Now} | {commentQty} Comments and {postQty} Posts shredded so far.";
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

            OutputTextBlock.Text += $"\r\n{DateTime.Now} | Calculating Post, Comment, and PM counts.";
            var posts = _redditUser.Posts.Take(1000).ToList();
            var comments = _redditUser.Comments.Take(1000).ToList();
            var pms = _redditUser.PrivateMessages.Take(1000).ToList();

            OutputTextBlock.Text +=
                $"\r\n{DateTime.Now} | Found {posts.Count} Posts, {comments.Count} Comments, and {pms.Count} PMs waiting to be shredded.";
            ShredButton.IsEnabled = true;

        }
    }


}
