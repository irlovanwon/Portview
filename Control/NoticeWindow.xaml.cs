///Copyright(c) 2013,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2013-11-05
///Description:
///Modification:2015-02-26
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
using System;
using System.Windows;

namespace Irlovan.Control
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class NoticeWindow : Window
    {


        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="text"></param>
        public NoticeWindow(string text) {
            InitializeComponent();
            Title = Lib.Properties.Resources.NoticeWindow_Title;
            Button_Confirm.Content = Lib.Properties.Resources.NoticeWindow_Confirm;
            Action<string> showMessage = new Action<string>(ShowMessage);
            Dispatcher.BeginInvoke(showMessage, text);
            ResizeMode = ResizeMode.NoResize;
        }

        #endregion Structure

        #region Function

        /// <summary>
        /// OPEN NOTICE WINDOW
        /// </summary>
        public static void Notice(string message) {
            Irlovan.Control.NoticeWindow window = new Irlovan.Control.NoticeWindow(message);
            window.ShowDialog();
        }

        /// <summary>
        /// Button_Confirm_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Confirm_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// ShowMessage
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message) {
            NoticeText.Text = message;
        }

        #endregion Function

    }
}
