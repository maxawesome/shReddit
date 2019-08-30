using RedditSharp;
using RedditSharp.Things;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
        private Logger _logger;

        private int _shreddedComments;
        private int _shreddedPosts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShredThreadProc(object stateInfo)
        {
            _logger = new Logger();
            _intervalTimer = new System.Timers.Timer(1000);
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
            //Only update the UI when new posts/comments have been shredded.
            if (_shredEngine.ShreddedComments > _shreddedComments || _shredEngine.ShreddedPosts > _shreddedPosts)
            {
                _shreddedComments = _shredEngine.ShreddedComments;
                _shreddedPosts = _shredEngine.ShreddedPosts;
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<int, int>(UpdateUIThreadWithProgress), _shredEngine.ShreddedComments, _shredEngine.ShreddedPosts);
            }
            if (_shredResult) _intervalTimer.Stop();
        }

        private void UpdateUIThreadWithProgress(int commentQty, int postQty)
        {
            _logger = new Logger();
            OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, $"{commentQty} Comments and {postQty} Posts shredded so far.", false);
        }

        private void UpdateUIThreadWithShredResult(bool result)
        {
            _logger = new Logger();
            OutputTextBlock.Text = result ? _logger.LogEntry(OutputTextBlock.Text, "Shredding complete.", false) : _logger.LogEntry(OutputTextBlock.Text, "Shredding failed.", false);
            ToggleShredImage(false);
            ShredButton.IsEnabled = true;
        }

        private void ShredButton_Click(object sender, RoutedEventArgs e)
        {
            _shreddedComments = 0;
            _shreddedPosts = 0;
            _logger = new Logger();
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
            OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, "Shredding in progress.", false);

            Thread.Sleep(1000);
        }

        private void ToggleShredImage(bool shredding)
        {
            LogoImage.Source = shredding ? new BitmapImage(new Uri("Images/shreddit_on.png", UriKind.Relative)) : new BitmapImage(new Uri("Images/shreddit_off.png", UriKind.Relative));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _logger = new Logger();
            OutputDock.Visibility = Visibility.Visible;
            OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, $"Attempting to log in as {UserNameText.Text}.", false);

            if (ProcessLogin(UserNameText.Text, PasswordText.Password))
            {
                OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, "Login successful. Looking for your posts and comments.", false);
                CalculateItemCounts();
                LoginButton.IsEnabled = false;
            }
            else
            {
                OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, "Unable to log in at this time. Check your login info and try again.", false);
            }

        }

        private bool ProcessLogin(string username, string password)
        {
            _logger = new Logger();
            _reddit = new Reddit(WebAgent.RateLimitMode.Pace);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            try
            {
                OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, $"Logging in as {username}", false);
                _redditUser = _reddit.LogIn(username, password);
            }
            catch (Exception ex)
            {
                OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, $"Login failed. Exception encountered: {ex.Message}. Is Reddit down?", false);
            }

            if (_redditUser != null) return true;
            return false;
        }

        private void CalculateItemCounts()
        {
            _logger = new Logger();
            OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, "Calculating post and comment counts.", false);

            var postCount = _redditUser.Posts.Count();
            var commentCount = _redditUser.Comments.Count();

            if (postCount > 0 | commentCount > 0)
            {
                PopulateQuantityPickers(postCount, commentCount);


                ShredButton.IsEnabled = true;
                OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, $"Found {postCount} post(s) and {commentCount} comment(s) waiting to be shredded.", false);
                OptionsDock.Visibility = Visibility.Visible;
                ShredDock.Visibility = Visibility.Visible;
            }
            else
            {
                OutputTextBlock.Text = _logger.LogEntry(OutputTextBlock.Text, "Couldn't find any posts or comments to shred. If you know you have some, wait a few minutes and try again.", false);
            }
        }

        private void PopulateQuantityPickers(int postCount, int commentCount)
        {
            if (postCount > 50)
                DeletePostsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 50 });
            if (postCount > 100)
                DeletePostsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 100 });
            if (postCount > 500)
                DeletePostsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 500 });
            if (postCount > 1000)
                DeletePostsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 1000 });

            DeletePostsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = postCount });


            if (commentCount > 50)
                DeleteCommentsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 50 });
            if (commentCount > 100)
                DeleteCommentsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 100 });
            if (commentCount > 500)
                DeleteCommentsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 500 });
            if (commentCount > 1000)
                DeleteCommentsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = 1000 });

            DeleteCommentsQuantity.Items.Add(new ComboBoxItem() { IsSelected = false, Content = commentCount });

        }
    }


}
