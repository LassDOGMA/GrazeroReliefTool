using log4net;
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GrazeroReliefTool.Scripts;

namespace GrazeroReliefTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        // log4Net
        //public static readonly ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().FullName);
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static StringBuilder sb = new StringBuilder();
        private static CancellationTokenSource tokenSource;
        private static CancellationToken cancelToken;
        public MainWindow()
        {
            logger.Info("Start GrazeroReliefTool");
            InitializeComponent();

            // get arguments
            string[] args = Environment.GetCommandLineArgs();
            logger.Debug(args);

            // open the chrome
            Program.Program.OpenChrome(args[1]);

            // start the stream of twitter
            Program.Program.tokens = CoreTweet.Tokens.Create(args[2], args[3], args[4], args[5]);
            Task.Run(() => Program.Program.StreamingFindParticipationId(SelectMulti, SelectTime));
        }
        protected virtual void WindowClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // end chromedriver
            Program.Program.driver.Close();
            // close chrome window
            Program.Program.driver.Quit();
            // terminate remaining processes
            ChromeHelper.KillAllProcesses("chromedriver");
        }

        private void SetParticipationIdOnClick(object sender, RoutedEventArgs e)
        {
            // enter text in TextBlock(multibattle)
            Program.Program.MultiBattleText(SelectMulti);
        }
        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            ButtonAction.SetListView((Button)sender, MultiList);
        }

        private void ListViewOnClick(object sender, SelectionChangedEventArgs e)
        {
            // ボタン⇒リスト⇒ボタン　って押すとなんか動くバグ回避
            ListView listView = (ListView)sender;
            if (listView.SelectedValue != null)
            {
                // リスト押下でチェンジ
                ButtonAction.ChangeMulti((ListView)sender, SelectMulti);
            }
        }

        private void RestartOnClick(object sender, System.EventArgs e)
        {
            if(Restart.IsChecked == true)
            {
                // 自動画面遷移モード開始
                tokenSource = new CancellationTokenSource();
                cancelToken = tokenSource.Token;
                Task.Run(() => Program.Program.UrlChecker(cancelToken));
            }
            else
            {
                tokenSource.Cancel();
            }
        }
    }
}