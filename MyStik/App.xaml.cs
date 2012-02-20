using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using myUserControls;
using System.Windows.Media;

namespace MyStik
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    
    public partial class App: Application
    {

        public static ManualResetEvent ResetSplashCreated;
        public static Thread SplashThread;
        public static SplashWindow splashWindow;

        [System.STAThreadAttribute()]
       
        public static void Main()
        {
            
            ResetSplashCreated = new ManualResetEvent(false);
            SplashThread = new Thread(ShowSplash);
            SplashThread.SetApartmentState(ApartmentState.STA);
            SplashThread.IsBackground = true;
            SplashThread.Name = "Splash Screen";
            SplashThread.Start();
            
            ResetSplashCreated.WaitOne();

            var app = new App();
            app.InitializeComponent();
            
            app.Run();
        }

        

        private static  void ShowSplash()
        {
            splashWindow = new SplashWindow();
            splashWindow.Show();
            ResetSplashCreated.Set();
            System.Windows.Threading.Dispatcher.Run();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            // define application exception handler
           


            Application.Current.DispatcherUnhandledException += new
               DispatcherUnhandledExceptionEventHandler(
                  AppDispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //// defer other startup processing to base class
            //base.OnStartup(e);
            App.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);
            
        }
        public App()
            : base()
        {


            Application.Current.DispatcherUnhandledException += new
               DispatcherUnhandledExceptionEventHandler(
                  AppDispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            
            Exception ex = e.ExceptionObject as Exception;
            //MessageBox.Show(ex.Message, "Uncaught Thread Exception",
            //                MessageBoxButton.OK, MessageBoxImage.Error);

            //MessageBox.Show(ex.InnerException.ToString());
            //SendMessage("MyStik Crash Report - Unhandled Exception", ex.Message);
            try
            {
                string myInsertQuery = "INSERT INTO Crashlog (date, message, source, targetsite, innerexception, type) Values(@date, @message, @source, @targetsite, @innerexception, @type)";

                
                MySqlConnection myConnection = new MySqlConnection
                                        (
                                            "UID=mystik;" +
                                            "PASSWORD=puchan@2.067;" +
                                            "SERVER=gauss.wi.hm.edu;" +
                                            "PORT=3306;" +
                                            "DATABASE=mystik;"
                                        );

                MySqlCommand myCommand = new MySqlCommand(myInsertQuery);
                myCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString());
                try
                {
                myCommand.Parameters.AddWithValue("@message", ex.Message.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@message", "None");
                }
                
                try
                {
                myCommand.Parameters.AddWithValue("@innerexception", ex.InnerException.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@innerexception", "None");
                }
                try
                {
                    myCommand.Parameters.AddWithValue("@targetsite", ex.TargetSite.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@targetsite", "None");
                }
                try
                {
                    myCommand.Parameters.AddWithValue("@source", ex.Source.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@source", "None");
                }
                myCommand.Parameters.AddWithValue("@type", "Exception");
                myCommand.Connection = myConnection;
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch { }
            Process.GetCurrentProcess().Kill();
        }

        void AppDispatcherUnhandledException(object sender,
           DispatcherUnhandledExceptionEventArgs e)
        {
            //do whatever you need to do with the exception
            //e.Exception
            //SendMessage("MyStik Crash Report - Unhandled Exception", e.Exception.InnerException.ToString());
            e.Handled = true;

            try
            {
                
                string myInsertQuery = "INSERT INTO Crashlog (date, message, source, targetsite innerexception, type) Values(@date, @message, @source, @targetsite, @innerexception, @type)";

                MySqlConnection myConnection = new MySqlConnection
                                        (
                                            "UID=mystik;" +
                                            "PASSWORD=puchan@2.067;" +
                                            "SERVER=gauss.wi.hm.edu;" +
                                            "PORT=3306;" +
                                            "DATABASE=mystik;"
                                        );

                MySqlCommand myCommand = new MySqlCommand(myInsertQuery);
                myCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString());
                try
                {
                    myCommand.Parameters.AddWithValue("@message", e.Exception.Message.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@message", "None");
                }
                try
                {
                    myCommand.Parameters.AddWithValue("@souce", e.Exception.Source.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@source", "None");
                }
                try
                {
                    myCommand.Parameters.AddWithValue("@targetsite", e.Exception.TargetSite.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@targetsite", "None");
                }
                try
                {
                    myCommand.Parameters.AddWithValue("@innerexception", e.Exception.InnerException.ToString());
                }
                catch
                {
                    myCommand.Parameters.AddWithValue("@innerexception", "None");
                }
                myCommand.Parameters.AddWithValue("@type", "Exception");
                myCommand.Connection = myConnection;
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch { }

            Process.GetCurrentProcess().Kill();
            
            
        }
        
        
    }

}