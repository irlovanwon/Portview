///Copyright(c) 2014,HIT All rights reserved.
///Summary：
///Author：Irlovan
///Date：2014-09-28
///Description：
///Modification：


using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.Windows;
using System.Windows.Input;

namespace Irlovan.Server
{
    internal class NotifyIcon
    {

        /// <summary>
        /// OpenLogFile
        /// </summary>
        public ICommand OpenLogFileCommand {
            get {
                return new DelegateCommand {
                    CanExecuteFunc = () => true,
                    CommandAction = () => {
                        //Application.Current.MainWindow = new MainWindow();
                        //Application.Current.MainWindow.Show();
                        try {
                            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + Logger.LogPath);
                        }
                        catch (Exception exception) {
                            Global.Info.LogRecorder.Log(LogLevelEnum.Error, exception.ToString());
                        }
                    }
                };
            }
        }

        /// <summary>
        ///    OpenProjectCommand
        /// </summary>
        public ICommand OpenProjectCommand {
            get {
                return new DelegateCommand {
                    CanExecuteFunc = () => true,
                    CommandAction = () => {
                        try {
                            System.Diagnostics.Process.Start(Global.Info.ProjectPath);
                        }
                        catch (Exception exception) {
                            Global.Info.LogRecorder.Log(LogLevelEnum.Error, exception.ToString());
                        }
                    }
                };
            }
        }

        /// <summary>
        /// About
        /// </summary>
        public ICommand AboutCommand {
            get {
                return new DelegateCommand {
                    CommandAction = () => {
                        Irlovan.Control.NoticeWindow window = new Irlovan.Control.NoticeWindow(
                         "Company:CZGT" +
                         Symbol.NewLine_Symbol +
                         "Email:irlovan.cncgt@gmail.com" +
                         Symbol.NewLine_Symbol +
                         "Phone:＋8613382163271");
                        window.ShowDialog();
                    },
                    CanExecuteFunc = () => true
                };
            }
        }



        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand {
            get {
                return new DelegateCommand {
                    CanExecuteFunc = () => App.InitReady,
                    //CanExecuteFunc = () => true,
                    CommandAction = () => Application.Current.Shutdown()
                    //CommandAction = () => Irlovan.Helper.Helper.Close()
                };
            }
        }
    }


    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter) {
            CommandAction();
        }

        public bool CanExecute(object parameter) {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
