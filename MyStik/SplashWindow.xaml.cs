using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;
using System.Reflection;


namespace MyStik
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        int progress = 0;
        public interface IApplicationLoading
        {

            //void LoadComplete();
        }
        public SplashWindow()
        {
            //Mouse.OverrideCursor = Cursors.None;
            InitializeComponent();
            versionLabel.Content = "Version: " + Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + " Build: " + Assembly.GetExecutingAssembly().GetName().Version.Build;

        }
        public void LoadComplete()
        {
            //this.Dispatcher.Invoke(new Action(delegate
            //        {
            //            this.Close();
            //        }));
            Dispatcher.InvokeShutdown();
        }  
        public void Progress(int percentage)
        {
            
            
            
            this.Dispatcher.Invoke(new Action(delegate
                    {
                        progressbar.Width = (float)(194.0f/100.0f)*percentage;
                        
                    }));
        }
        public void StatusUpdate(string message)
        { 
        this.Dispatcher.Invoke(new Action(delegate
                    {
                        label.Content=message;
                        progress += 5;
                        progressbar.Width = (float)(194.0f / 100.0f) * progress;
                    }));
       
        
        }
    }
}
