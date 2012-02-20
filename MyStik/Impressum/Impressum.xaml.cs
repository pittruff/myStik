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

using Microsoft.Surface.Presentation.Controls;
using MyStik;
using System.IO;
using System.Windows.Media.Animation;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for Impressum.xaml
    /// </summary>
    public partial class Impressum2 : UserControl
    {
        Canvas creditCanvas = new Canvas();
        public Impressum2()
        {
            InitializeComponent();
            
         
        }
        private SurfaceWindow1 mainwindow;
        public SurfaceWindow1 Mainwindow
        {
          set{mainwindow=value;}
            get { return mainwindow; }
        }

        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            this.Mainwindow.BackGrid.Children.Clear();
            
            this.Mainwindow.MainGrid.Effect = null;
            this.Mainwindow.MainGrid.IsEnabled = true;
            
        }

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            this.Mainwindow.BackGrid.Children.Clear();
            this.Mainwindow.MainGrid.Visibility = Visibility.Hidden;
            creditCanvas.Background = Brushes.White;
            creditCanvas.IsHitTestVisible = true;
            RichTextBox richTextBox1 = new RichTextBox();
            creditCanvas.Children.Add(richTextBox1);
            FileStream fs = new FileStream("Documents\\Credits\\credits.rtf", FileMode.Open, FileAccess.Read);

            TextRange RTBText = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            RTBText.Load(fs, DataFormats.Rtf);
            richTextBox1.Width = 1920;
            richTextBox1.Height = 10000;
            richTextBox1.HorizontalAlignment = HorizontalAlignment.Center;
            richTextBox1.Background = Brushes.Black;
            richTextBox1.Foreground = Brushes.WhiteSmoke;
            richTextBox1.FontFamily = new FontFamily("Segoe UI");


            richTextBox1.BorderThickness = new Thickness(0);
            TranslateTransform ttransform = new TranslateTransform();
            richTextBox1.RenderTransform= ttransform;
            DoubleAnimation scrollAnim = new DoubleAnimation(0,-10000,new Duration(TimeSpan.FromSeconds(60)),FillBehavior.Stop);
            scrollAnim.Completed += new EventHandler(scrollAnim_Completed);
            richTextBox1.ClipToBounds = true;
            //scrollAnim.RepeatBehavior = RepeatBehavior.Forever;
            ttransform.BeginAnimation(TranslateTransform.YProperty, scrollAnim);
            creditCanvas.PreviewMouseDown += new MouseButtonEventHandler(creditCanvas_PreviewMouseDown);
            this.Mainwindow.BackGrid.Children.Add(creditCanvas);


        }

        void scrollAnim_Completed(object sender, EventArgs e)
        {
            creditCanvas.Visibility = Visibility.Hidden;
            this.Mainwindow.MainGrid.Effect = null;
            this.Mainwindow.MainGrid.IsEnabled = true;
            this.Mainwindow.MainGrid.Visibility = Visibility.Visible;
        }

        void creditCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Canvas).Visibility = Visibility.Hidden;
            this.Mainwindow.MainGrid.Effect = null;
            this.Mainwindow.MainGrid.IsEnabled = true;
            this.Mainwindow.MainGrid.Visibility = Visibility.Visible;
            
        }
    }
}
