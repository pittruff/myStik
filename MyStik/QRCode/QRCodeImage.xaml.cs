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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for QRCodeImage.xaml
    /// </summary>
    public partial class QRCodeImage : UserControl
    {
        private string message;
        
        public string Message
        {
            set { this.message = value; }
        }
        public QRCodeImage(string message)
        {
            try
            {
                InitializeComponent();
                try
                {
                    qrImage.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("http://chart.apis.google.com/chart?chs=300x300&choe=ISO-8859-1&cht=qr&chl=" + message + "&choe=UTF-8&chld=L|1");
                    messageLabel.Content = message;
                }
                catch
                {
                }
            }
            catch
            { 
            
            }
        }
    }
}
