using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.ComponentModel;
using System.Xml;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Threading;
using HtmlAgilityPack;
using myUserControls;
using Microsoft.Expression.Interactivity.Core;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Effects;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net;
using System.Reflection;





namespace MyStik
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        Grid currentGrid;
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool LockWorkStation();
       
        
        Point touchPos;
        
        public Janitor janitor = new Janitor();
        public StatusReport statusReport = new StatusReport();
        bool firstRender = true;
        bool newUser = false;
        
        
        public int BlurRadius;
        private String Text = "";
        TextBox activeTextbox = null;
        PasswordBox activePasswordbox = null;
        ComboBox activeCombobox = null;
        bool loggedIn;
        bool userExists;
        public CardReader cardReader = new CardReader();
        public MySqlConnection myConnection = new MySqlConnection
                                        (
                                            "UID=mystik;" +
                                            "PASSWORD=puchan@2.067;" +
                                            "SERVER=gauss.wi.hm.edu;" +
                                            "PORT=3306;" +
                                            "DATABASE=mystik;"
                                        );
        public string username = "";
        public string password = "";
        
        
        public SurfaceWindow1()
        {
            App.splashWindow.Progress(25);
            //parseBlackboardXml("bbitems.xml");
        //    AppDomain.CurrentDomain.UnhandledException +=
        //new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            
            App.splashWindow.StatusUpdate("Starte MyStik");
            InitializeComponent();
            //Mouse.OverrideCursor = Cursors.None;
            App.splashWindow.StatusUpdate("Hauptfenster ist initialisiert");
            
            App.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            (this.Resources["KeyboardBrush"] as SolidColorBrush).Freeze();
            App.splashWindow.Progress(90);
            
            currentGrid = HomeGrid;
            statusReport.Hauptformular = this;
            cardReader.Hauptformular = this;
            
            calendarControl1.Hauptformular = this;
            blackboardControl.Hauptformular = this;
            
            raumplan.Hauptformular = this;
            fahrplan.Mainwindow = this;
            newsReader.Hauptformular = this;
            janitor.Hauptformular = this;
            infos.Hauptformular = this;
            janitor.CleanUp();
            
            try
            {
                Thread t = new Thread(cardReader.checkCard);
                
                t.Start();
            }
            catch { }
            blackboardControl.Populate();
            statusReport.doReport();
            
            
          
            
            


            
            
            loggedIn = false;
            App.splashWindow.Progress(95);
            App.splashWindow.StatusUpdate("Hauptfenster ist bereit.");
            
            
        }
        
        private void CreateScreenShot(UIElement visual, string file)
        {

            double width = Convert.ToDouble(visual.GetValue(FrameworkElement.WidthProperty));

            double height = Convert.ToDouble(visual.GetValue(FrameworkElement.HeightProperty));



            if (double.IsNaN(width) || double.IsNaN(height))
            {

                throw new FormatException("You need to indicate the Width and Height values of the UIElement.");

            }



            RenderTargetBitmap render = new RenderTargetBitmap(

               Convert.ToInt32(width),

               Convert.ToInt32(visual.GetValue(FrameworkElement.HeightProperty)),

               96,

               96,

               PixelFormats.Pbgra32);



            // Indicate which control to render in the image

            render.Render(visual);



            using (FileStream stream = new FileStream(file, FileMode.Create))
            {

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();


                encoder.QualityLevel = 70;
                
                encoder.Frames.Add(BitmapFrame.Create(render));



                encoder.Save(stream);


            }

        }

     
        public void RollIn(Grid grid)
        {

            //Console.WriteLine("rolled in: " + grid.Name);
            currentGrid = grid;
            grid.Visibility = Visibility.Visible;

            TranslateTransform offsetTransform = new TranslateTransform();

            DoubleAnimation offsetXAnimation = new DoubleAnimation(1920, 0, new Duration(TimeSpan.FromSeconds(.5)));

            offsetXAnimation.AccelerationRatio = 0.5;
            offsetXAnimation.DecelerationRatio = 0.5;

            offsetXAnimation.BeginTime = TimeSpan.FromSeconds(0);
            offsetTransform.BeginAnimation(TranslateTransform.XProperty, offsetXAnimation);
            //offsetTransform.BeginAnimation(TranslateTransform.YProperty, offsetXAnimation);

            grid.RenderTransform = offsetTransform;
            grid.IsHitTestVisible = true;
            
        }
        public void RollOut(Grid grid)
        {
            //Console.WriteLine("rolled out: " + grid.Name);
            KeyboardSVI.Visibility = Visibility.Hidden;
            if (grid.Visibility == Visibility.Visible)
            {

                grid.Visibility = Visibility.Visible;

                TranslateTransform offsetTransform = new TranslateTransform();

                DoubleAnimation offsetXAnimation = new DoubleAnimation(0, -1920, new Duration(TimeSpan.FromSeconds(.5)));
                offsetXAnimation.AccelerationRatio = 0.5;
                offsetXAnimation.DecelerationRatio = 0.5;
                offsetXAnimation.BeginTime = TimeSpan.FromSeconds(0);
                offsetTransform.BeginAnimation(TranslateTransform.XProperty, offsetXAnimation);
                //offsetTransform.BeginAnimation(TranslateTransform.YProperty, offsetXAnimation);

                grid.RenderTransform = offsetTransform;

                grid.IsHitTestVisible = false;
                
            }




        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //VVK
            
            NavLabel.Content = (sender as SurfaceButton).Tag;
            if (button1.IsEnabled == false)
            {
                RollOut(VVKGrid);
            }
            if (button2.IsEnabled == false)
            {
                RollOut(NewsGrid);
            }
            if (button3.IsEnabled == false)
            {
                RollOut(BlackboardGrid);
            }
            if (button4.IsEnabled == false)
            {
                RollOut(MVVGrid);
            }
            if (homeButton.IsEnabled == false)
            {
                RollOut(HomeGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button7.IsEnabled == false)
            {
                RollOut(GamesGrid);
            }

            tb1.Foreground = Brushes.Black;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.WhiteSmoke;
            button1.IsEnabled = false;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            homeButton.IsEnabled = true;






            RollIn(VVKGrid);

            
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //NEWS
            NavLabel.Content = (sender as SurfaceButton).Tag;
            if (button1.IsEnabled == false)
            {
                RollOut(VVKGrid);
            }
            if (button2.IsEnabled == false)
            {
                RollOut(NewsGrid);
            }
            if (button3.IsEnabled == false)
            {
                RollOut(BlackboardGrid);
            }
            if (button4.IsEnabled == false)
            {
                RollOut(MVVGrid);
            }
            if (homeButton.IsEnabled == false)
            {
                RollOut(HomeGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button7.IsEnabled == false)
            {
                RollOut(GamesGrid);
            }
            tb1.Foreground = Brushes.WhiteSmoke;
            tb2.Foreground = Brushes.Black;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.WhiteSmoke;
            button1.IsEnabled = true;
            
            button2.IsEnabled = false;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            homeButton.IsEnabled = true;


            
            RollIn(NewsGrid);
        }
   

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            NavLabel.Content = (sender as SurfaceButton).Tag;
            if (button1.IsEnabled == false)
            {
                RollOut(VVKGrid);
            }
            if (button2.IsEnabled == false)
            {
                RollOut(NewsGrid);
            }
            if (button3.IsEnabled == false)
            {
                RollOut(BlackboardGrid);
            }
            if (button4.IsEnabled == false)
            {
                RollOut(MVVGrid);
            }
            if (homeButton.IsEnabled == false)
            {
                RollOut(HomeGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button7.IsEnabled == false)
            {
                RollOut(GamesGrid);
            }
            tb1.Foreground = Brushes.WhiteSmoke;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.Black;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.WhiteSmoke;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            
            button3.IsEnabled = false;
            button4.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            homeButton.IsEnabled = true;

          

            RollIn(BlackboardGrid);


            


        }


        private void button4_Click(object sender, RoutedEventArgs e)
        {
            NavLabel.Content = (sender as SurfaceButton).Tag;
            if (button1.IsEnabled == false)
            {
                RollOut(VVKGrid);
            }
            if (button2.IsEnabled == false)
            {
                RollOut(NewsGrid);
            }
            if (button3.IsEnabled == false)
            {
                RollOut(BlackboardGrid);
            }
            if (button4.IsEnabled == false)
            {
                RollOut(MVVGrid);
            }
            if (homeButton.IsEnabled == false)
            {
                RollOut(HomeGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button7.IsEnabled == false)
            {
                RollOut(GamesGrid);
            }

            tb1.Foreground = Brushes.WhiteSmoke;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.Black;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.WhiteSmoke;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button6.IsEnabled = true;
            button4.IsEnabled = false;
            button7.IsEnabled = true;
            homeButton.IsEnabled = true;
            fahrplan.resetPlans();



            
            RollIn(MVVGrid);





        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            NavLabel.Content = (sender as SurfaceButton).Tag;
            if (button1.IsEnabled == false)
            {
                RollOut(VVKGrid);
            }
            if (button2.IsEnabled == false)
            {
                RollOut(NewsGrid);
            }
            if (button3.IsEnabled == false)
            {
                RollOut(BlackboardGrid);
            }
            if (button4.IsEnabled == false)
            {
                RollOut(MVVGrid);
            }
            if (homeButton.IsEnabled == false)
            {
                RollOut(HomeGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button7.IsEnabled == false)
            {
                RollOut(GamesGrid);
            }

            tb1.Foreground = Brushes.WhiteSmoke;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.Black;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            homeButton.IsEnabled = false;




            RollIn(HomeGrid);





        }

        //public void SVI_Init(object sender, RoutedEventArgs e)
        //{

        //    Random random = new Random();
        //    int xShift = random.Next(-20, 20);
        //    int yShift = random.Next(-20, 20);
        //    int rotShift = random.Next(-20, 20);
        //    (sender as ScatterViewItem).Center = new Point(1200 + xShift, 300 + yShift);
        //    (sender as ScatterViewItem).Orientation = rotShift;
        //    Duration duration = new Duration(TimeSpan.FromMilliseconds(500));

        //    Storyboard myStoryboard = new Storyboard();


        //    DoubleAnimation opacityAnimation = new DoubleAnimation();
        //    opacityAnimation.From = 0.0;
        //    opacityAnimation.To = 1.0;
        //    opacityAnimation.AccelerationRatio = 0.5;
        //    opacityAnimation.DecelerationRatio = 0.5;
        //    opacityAnimation.Duration = duration;

        //    myStoryboard.Children.Add(opacityAnimation);
        //    Storyboard.SetTarget(opacityAnimation, sender as ScatterViewItem);
        //    Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(ScatterViewItem.OpacityProperty));


        //    DoubleAnimation heigthAnimation = new DoubleAnimation();
        //    heigthAnimation.From = 60.0;
        //    heigthAnimation.To = 250.0;

        //    heigthAnimation.Duration = duration;
        //    heigthAnimation.AccelerationRatio = 0.5;
        //    heigthAnimation.DecelerationRatio = 0.5;
        //    myStoryboard.Children.Add(heigthAnimation);
        //    Storyboard.SetTarget(heigthAnimation, sender as ScatterViewItem);
        //    Storyboard.SetTargetProperty(heigthAnimation, new PropertyPath(ScatterViewItem.HeightProperty));


        //    DoubleAnimation widthAnimation = new DoubleAnimation();
        //    widthAnimation.From = 60.0;
        //    widthAnimation.To = 600.0;
        //    widthAnimation.AccelerationRatio = 0.5;
        //    widthAnimation.DecelerationRatio = 0.01;
        //    widthAnimation.Duration = duration;

        //    myStoryboard.Children.Add(widthAnimation);
        //    Storyboard.SetTarget(widthAnimation, sender as ScatterViewItem);
        //    Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));



        //    PointAnimation moveCenter = new PointAnimation();
        //    Point startPoint = new Point(300, 300);
        //    Point endPoint = new Point(1200 + xShift, 300 + yShift);

        //    moveCenter.AccelerationRatio = 0.5;
        //    moveCenter.DecelerationRatio = 0.5;
        //    moveCenter.From = startPoint;
        //    moveCenter.To = endPoint;
        //    moveCenter.Duration = new Duration(TimeSpan.FromSeconds(.5));
        //    moveCenter.FillBehavior = FillBehavior.Stop;

        //    myStoryboard.Children.Add(moveCenter);

        //    Storyboard.SetTarget(moveCenter, (sender as ScatterViewItem));
        //    Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));

        //    myStoryboard.Begin(this);
        //}

        //public void bbItem_Anim(object sender)
        //{
        //    if ((sender as ScatterViewItem).Visibility == Visibility.Hidden)
        //    {

        //    }
        //    else
        //    {


        //    }
        //}
        //public void LBI_Init(object sender, RoutedEventArgs e)
        //{

        //    Storyboard myStoryboard = new Storyboard();

        //    DoubleAnimation opacityAnimation = new DoubleAnimation();
        //    opacityAnimation.From = 0.0;
        //    opacityAnimation.To = 1.0;

        //    opacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(150));

        //    myStoryboard.Children.Add(opacityAnimation);
        //    Storyboard.SetTarget(opacityAnimation, sender as ListBoxItem);
        //    Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(ListBoxItem.OpacityProperty));



        //    myStoryboard.Begin(this);

        //}
       
        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            //if (Shift == true)
            //{
            //    //Text += ((SurfaceButton)sender).Content.ToString().ToUpper();

            //    shiftUpVisibility = Visibility.Visible;
            //    shiftDownVisibility = Visibility.Hidden;
            //}
            //else
            //{
            //    //Text += ((SurfaceButton)sender).Content.ToString().ToLower();
            //    shiftUpVisibility = Visibility.Hidden;
            //    shiftDownVisibility = Visibility.Visible;
            //}


            Text += ((SurfaceButton)sender).Content;
            textLabel.Content = Text;
            if (activeTextbox != null)
            {
                activeTextbox.Text = Text;
                activeTextbox.SelectionStart = Text.Length;
            }
            if (activePasswordbox != null)
            {
                activePasswordbox.Password = Text;
                
            }
            if (activeCombobox != null)
            {
                activeCombobox.Text = Text;
            }


        }
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            textLabel.Content = Text;
            if (Text.Equals("") || Text ==null)
            {
            }
            else
            {
                Text = Text.Remove(Text.Length - 1, 1);
                textLabel.Content = Text;
                if (activeTextbox != null)
                {
                    activeTextbox.Text = Text;
                    activeTextbox.SelectionStart = Text.Length;
                }
                if (activePasswordbox != null)
                {
                    activePasswordbox.Password = Text;
                }
                if (activeCombobox != null)
                {
                    activeCombobox.Text = Text;
                }
            }

        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            moveToggle.IsChecked = false;
            if (activeTextbox != null)
                {
                    
                    KeyEventArgs keyEventArgs = new KeyEventArgs(InputManager.Current.PrimaryKeyboardDevice, InputManager.Current.PrimaryKeyboardDevice.ActiveSource, System.Environment.ProcessorCount, Key.Enter);    
                    activeTextbox.Text = Text;
                    keyEventArgs.RoutedEvent = UIElement.KeyDownEvent;
                    activeTextbox.RaiseEvent(keyEventArgs);
                    keyEventArgs.RoutedEvent = UIElement.KeyUpEvent;
                    activeTextbox.RaiseEvent(keyEventArgs);

                }
                if (activePasswordbox != null)
                {
                    KeyEventArgs keyEventArgs = new KeyEventArgs(InputManager.Current.PrimaryKeyboardDevice, InputManager.Current.PrimaryKeyboardDevice.ActiveSource, System.Environment.ProcessorCount, Key.Enter);
                    activePasswordbox.Password = Text;
                    keyEventArgs.RoutedEvent = UIElement.KeyDownEvent;
                    activePasswordbox.RaiseEvent(keyEventArgs);
                    keyEventArgs.RoutedEvent = UIElement.KeyUpEvent;
                    activePasswordbox.RaiseEvent(keyEventArgs);
                }
                if (activeCombobox != null)
                {
                    activeCombobox.Text = Text;
                }

            Point endPoint = new Point(960, 1500);
            Point startPoint = KeyboardSVI.Center;
            KeyboardSVI.Center = startPoint;
            Storyboard storyboard = new Storyboard();
            Storyboard storyboard_move = new Storyboard();
            //storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Duration = TimeSpan.FromMilliseconds(500);
            storyboard.Completed += new EventHandler(sb_Out_Completed);

            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.Duration = TimeSpan.FromMilliseconds(500);
            opacityAnimation.From = 1;
            opacityAnimation.To = 0;
            opacityAnimation.AccelerationRatio = 0.5;
            opacityAnimation.DecelerationRatio = 0.5;





            PointAnimation moveCenter = new PointAnimation();
            moveCenter.AccelerationRatio = 0.5;
            moveCenter.DecelerationRatio = 0.5;
            moveCenter.From = startPoint;
            moveCenter.To = endPoint;
            moveCenter.Duration = new Duration(TimeSpan.FromSeconds(.5));
            moveCenter.FillBehavior = FillBehavior.Stop;

            storyboard.Children.Add(opacityAnimation);
            storyboard_move.Children.Add(moveCenter);




            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(ScatterViewItem.OpacityProperty));
            Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));


            Storyboard.SetTarget(opacityAnimation, KeyboardSVI);
            Storyboard.SetTarget(moveCenter, KeyboardSVI);

            KeyboardSVI.Center = endPoint;

            storyboard.Begin();
            storyboard_move.Begin();



        }

        private void SurfaceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void ShiftButton_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //textLabel.Content = "Touchdown";
            //Shift = true;

            KeyboardUpper.Visibility = Visibility.Visible;
            KeyboardLower.Visibility = Visibility.Hidden;
            KeyboardAlt.Visibility = Visibility.Hidden;

        }
        private void ShiftButton_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            //textLabel.Content = "Touchup";
            //MessageBox.Show("enter");
            //Shift = false;

            KeyboardUpper.Visibility = Visibility.Hidden;
            KeyboardLower.Visibility = Visibility.Visible;
            KeyboardAlt.Visibility = Visibility.Hidden;
        }

        private void AltButton_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //textLabel.Content = "Touchdown";
            //Shift = true;

            KeyboardUpper.Visibility = Visibility.Hidden;
            KeyboardLower.Visibility = Visibility.Hidden;
            KeyboardAlt.Visibility = Visibility.Visible;


        }
        private void AltButton_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            //textLabel.Content = "Touchup";
            //MessageBox.Show("enter");
            //Shift = false;

            KeyboardUpper.Visibility = Visibility.Hidden;
            KeyboardLower.Visibility = Visibility.Visible;
            KeyboardAlt.Visibility = Visibility.Hidden;
        }

        private void KeyboardSVI_Initialized(object sender, EventArgs e)
        {
            App.splashWindow.StatusUpdate("Keyboard wird initialisiert");
            KeyboardSVI.ApplyTemplate();
            KeyboardSVI.Background = new SolidColorBrush(Colors.Transparent);
            KeyboardSVI.ShowsActivationEffects = false;
            Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;
            ssc = KeyboardSVI.Template.FindName("shadow", KeyboardSVI) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
            KeyboardSVI.Visibility = Visibility.Hidden;
            App.splashWindow.StatusUpdate("Keyboard ist bereit");


        }

        public void Call_Keyboard(object sender, RoutedEventArgs e)
        {
            moveToggle.IsChecked = true;
            KeyboardSVI.Visibility = Visibility.Visible;
            Text = "";
            textLabel.Content = "";

            Point startPoint = new Point(960, 1500);
            Point endPoint = new Point(960, 800);
            KeyboardSVI.Center = endPoint;
            Storyboard storyboard = new Storyboard();
            Storyboard storyboard_move = new Storyboard();
            storyboard.Duration = TimeSpan.FromMilliseconds(400);
            storyboard.Completed += new EventHandler(sb_In_Completed);


            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.Duration = TimeSpan.FromMilliseconds(500);
            opacityAnimation.From = 0;
            opacityAnimation.To = 1;
            opacityAnimation.AccelerationRatio = 0.5;
            opacityAnimation.DecelerationRatio = 0.5;

            PointAnimation moveCenter = new PointAnimation();
            moveCenter.AccelerationRatio = 0.5;
            moveCenter.DecelerationRatio = 0.5;
            moveCenter.From = startPoint;
            moveCenter.To = endPoint;
            moveCenter.Duration = new Duration(TimeSpan.FromSeconds(.5));
            moveCenter.FillBehavior = FillBehavior.Stop;

            storyboard.Children.Add(opacityAnimation);
            storyboard_move.Children.Add(moveCenter);


            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(ScatterViewItem.OpacityProperty));
            Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));


            Storyboard.SetTarget(opacityAnimation, KeyboardSVI);
            Storyboard.SetTarget(moveCenter, KeyboardSVI);

            if (sender.GetType() == typeof(TextBox))
            {
                activeTextbox = ((TextBox)sender);
                activePasswordbox = null;
                activeCombobox = null;
                activeTextbox.SelectionStart = activeTextbox.Text.Length;
                
                Text = activeTextbox.Text;
                textLabel.Content = Text;
            }
            if (sender.GetType() == typeof(PasswordBox))
            {
                activePasswordbox = ((PasswordBox)sender);
                activeTextbox = null;
                activeCombobox = null;
            }
            if (sender.GetType() == typeof(ComboBox))
            {
                activeCombobox = ((ComboBox)sender);
                activeTextbox = null;
                activePasswordbox = null;
            }

            storyboard.Begin();
            storyboard_move.Begin();
            KeyboardSVI.Center = endPoint;

        }
        private void sb_Out_Completed(object sender, EventArgs e)
        {
            //MessageBox.Show("The Storyboard has fired the Completed Event (Out)");
            KeyboardSVI.Visibility = Visibility.Hidden;
            //KeyboardSVI.Center = KeyboardSVI.ActualCenter;
        }

        private void sb_In_Completed(object sender, EventArgs e)
        {
            //MessageBox.Show("The Storyboard has fired the Completed Event (In)");
            KeyboardSVI.Visibility = Visibility.Visible;
            //KeyboardSVI.Center = KeyboardSVI.ActualCenter;
        }

        //private void loginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    button1_Click(sender,e);
        //    VVKReader loginReader = new VVKReader();
        //    loginReader.sendLoginData(usernameTextBox.Text, passwordPasswordBox.Password);
        //    usernameTextBox.Text = null;
        //    passwordPasswordBox.Password = null;

        //}

        public void userLogin()
        {
            if (!loggedIn||newUser)
            {
                newUser = false;
                loggedIn = true;
                try { myConnection.Close(); }
                catch { }
                //string existingKey = "";
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT * FROM `VVKUserDatabase` WHERE cardkey=@cardkey;";
                command.Parameters.AddWithValue("@cardkey", cardReader.CardKey);
                Cryptor cryptor= new Cryptor();
                MySqlDataReader Reader;
                myConnection.Open();
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    try
                    {
                        
                        //MessageBox.Show(cardReader.CardKey + ", " + Reader.GetValue(1).ToString() + ", "+ cryptor.Decrypt(Reader.GetValue(2).ToString()));
                        username = Reader.GetValue(1).ToString();
                        password = Reader.GetValue(2).ToString();
                        
                        
                        
                    }
                    catch {}

                }
                myConnection.Close();
                if (username.Equals(""))
                {
                    userExists = false;
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        //MessageBox.Show("'" + username + "'");
                        NavLabel.Content = "Registrierung";
                        if (button1.IsEnabled == false)
                        {
                            RollOut(VVKGrid);
                        }
                        if (button2.IsEnabled == false)
                        {
                            RollOut(NewsGrid);
                        }
                        if (button3.IsEnabled == false)
                        {
                            RollOut(BlackboardGrid);
                        }
                        if (button4.IsEnabled == false)
                        {
                            RollOut(MVVGrid);
                        }
                        if (homeButton.IsEnabled == false)
                        {
                            RollOut(HomeGrid);
                        }
                        if (button6.IsEnabled == false)
                        {
                            RollOut(RaumplanGrid);
                        }
                        if (button7.IsEnabled == false)
                        {
                            RollOut(GamesGrid);
                        }

                        button1.IsEnabled = false;
                        button2.IsEnabled = false;
                        button3.IsEnabled = false;
                        button4.IsEnabled = false;
                        button6.IsEnabled = false;
                        button7.IsEnabled = false;
                        homeButton.IsEnabled = false;
                        tb1.Foreground = Brushes.Black;
                        tb2.Foreground = Brushes.Black;
                        tb3.Foreground = Brushes.Black;
                        tb4.Foreground = Brushes.Black;
                        tb7.Foreground = Brushes.Black;
                        tb6.Foreground = Brushes.Black;
                        homeTB.Foreground = Brushes.Black;

                        RollIn(RegistrationGrid);
                        //nameBox.Text = "";
                        //lastnameBox.Text = "";
                        usernameBox.Text = "";
                        passwordBox.Password = "";
                        calendarControl1.myLectures.IsEnabled = false;
                        calendarControl1.myAccount.IsEnabled = true;
                        blackboardControl.likeButton.IsEnabled = false;
                    }));
                }
                else
                {
                    //MessageBox.Show("'" + username + "'");
                    
                    userExists = true;
                    this.Dispatcher.Invoke(new Action(delegate
                    { usernameLabel.Content = ("Eingeloggt");
                    try
                    {
                        calendarControl1.nextWeek.IsEnabled = true;
                        calendarControl1.getUserPlan(username, password);
                        calendarControl1.myLectures.IsEnabled = true;
                        calendarControl1.myAccount.IsEnabled = true;
                        blackboardControl.likeButton.IsEnabled = true;
                        RollOut(currentGrid);
                        RollIn(VVKGrid);
                        button1.IsEnabled = false;
                        button2.IsEnabled = true;
                        button3.IsEnabled = true;
                        button4.IsEnabled = true;
                        button6.IsEnabled = true;
                        button7.IsEnabled = true;
                        homeButton.IsEnabled = true;
                        tb1.Foreground = Brushes.Black;
                        tb2.Foreground = Brushes.WhiteSmoke;
                        tb3.Foreground = Brushes.WhiteSmoke;
                        tb4.Foreground = Brushes.WhiteSmoke;
                        tb7.Foreground = Brushes.WhiteSmoke;
                        tb6.Foreground = Brushes.WhiteSmoke;
                        homeTB.Foreground = Brushes.WhiteSmoke;

                        
                    }
                    catch { }
                    }));
                    



                }

                //XMLUserDBManipulator reader = new XMLUserDBManipulator();
                //XmlNode currentUser = reader.ReadItem(cardReader.CardKey, "userdatabase.xml");
                //if (loggedIn)
                //{
                    
                //}
                //else
                //{
                    

                //}
            }
        }

        public void userLogout()
        {
            if (userExists)
            {
                this.Dispatcher.Invoke(new Action(delegate
                    { usernameLabel.Content = "Nicht Eingeloggt"; }));
                loggedIn = false;
            }
            else
            {
                NavLabel.Dispatcher.Invoke(new Action(delegate { NavLabel.Content = "Home"; }));
                loggedIn = false;
                this.Dispatcher.Invoke(new Action(delegate
                {

                    RollOut(RegistrationGrid);
                    if (currentGrid != null)
                    {
                        RollOut(currentGrid);
                    }
                    
                    RollIn(HomeGrid);
                    button1.IsEnabled = true;
                    button2.IsEnabled = true;
                    button3.IsEnabled = true;
                    button4.IsEnabled = true;
                    button6.IsEnabled = true;
                    button7.IsEnabled = true;
                    homeButton.IsEnabled = false;
                    tb1.Foreground = Brushes.WhiteSmoke;
                    tb2.Foreground = Brushes.WhiteSmoke;
                    tb3.Foreground = Brushes.WhiteSmoke;
                    tb4.Foreground = Brushes.WhiteSmoke;
                    tb7.Foreground = Brushes.WhiteSmoke;
                    tb6.Foreground = Brushes.WhiteSmoke;
                    homeTB.Foreground = Brushes.Black;
                    

                }));

            }
            userExists = false;
            loggedIn = false;
            this.Dispatcher.Invoke(new Action(delegate
            {
                //nameBox.Text = "";
                //lastnameBox.Text = "";
                usernameBox.Text = "";
                passwordBox.Password = "";
                username = "";
                password = "";
                calendarControl1.myLectures.IsEnabled = false;
                calendarControl1.myAccount.IsEnabled = true;
                blackboardControl.likeButton.IsEnabled = false;
                //janitor.ResetEverything();
                foreach (var oldlecture in calendarControl1.cGrid.Children)
                {
                    if (oldlecture.GetType().ToString().Equals("myUserControls.Appointment"))
                    {
                        (oldlecture as Appointment).Visibility = Visibility.Hidden;

                    }
                }
                calendarControl1.groupNameLabel.Content = "Keine Gruppe gewählt";
                calendarControl1.nextWeek.IsEnabled = false;
            }));
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            Cryptor cryptor = new Cryptor();
            //XMLUserDBManipulator writer = new XMLUserDBManipulator();
            //writer.AddItem(cardReader.CardKey, "", "", usernameBox.Text, cryptor.Encrypt(passwordBox.Password), "userdatabase.xml");
            Regex theRegex = new Regex(@"/(\%27)|(\')|(\-\-)|(\%23)|(#)/ix",
                           RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (theRegex.IsMatch(usernameBox.Text) || theRegex.IsMatch(passwordBox.Password))
            {
                MessageBox.Show("Fehlerhafte Eingabe.",
                                "MySQL Syntax Error");

                // Clear/Reset textboxes
                usernameBox.Clear();
                passwordBox.Clear();
                return;
            }
            try
            {
                bool wrongAcc = false;
                try{
                XmlDocument doc = new XmlDocument();
                    doc.Load("http://interface.vorlesungsplanung.de/interface.php?format=xml&aid=1&shared_secret=sadvSe21_9&function=getuserplan&user=" + usernameBox.Text.Trim() + "&pwd=" + passwordBox.Password.Trim());
                
                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                    wrongAcc=true;
                }
                if (passwordBox.Password.Trim().Equals("") || usernameBox.Text.Trim().Equals(""))
                { MessageBox.Show("Bitte beide Felder ausfüllen"); }
                if(wrongAcc)
                {
                    MessageBox.Show("Passwort oder Username falsch");
                    usernameBox.Clear();
                    passwordBox.Clear();
                    wrongAcc = false;
                }
                else
                {
                    try { myConnection.Close(); }
                    catch { }
                    string myInsertQuery = "INSERT INTO VVKUserDatabase (cardkey, username, userpwd) Values(@cardkey, @username, @password)";
                    MySqlCommand myCommand = new MySqlCommand(myInsertQuery);
                    myCommand.Parameters.AddWithValue("@cardkey", cardReader.CardKey);
                    myCommand.Parameters.AddWithValue("@username", usernameBox.Text);
                    myCommand.Parameters.AddWithValue("@password", cryptor.Encrypt(passwordBox.Password));
                    myCommand.Connection = myConnection;
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                    MessageBox.Show("Karte registriert");
                    newUser = true;
                    userLogin();
                }
            }
            catch
            { MessageBox.Show("Karte ist bereits registriert"); }
            button1.IsEnabled = false;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            homeButton.IsEnabled = true;
            tb1.Foreground = Brushes.Black;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.WhiteSmoke;
            RollOut(RegistrationGrid);
            if (currentGrid != null)
            {
                RollOut(currentGrid);
            }
            RollOut(VVKGrid);
            RollIn(VVKGrid);
            calendarControl1.SurfaceButton_Click(sender, e);

        }

        private void SurfaceWindow_Closing(object sender, CancelEventArgs e)
        {

            
                Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send);
                Process.GetCurrentProcess().Kill();
         

            
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            NavLabel.Content = (sender as SurfaceButton).Tag;
            if (button1.IsEnabled == false)
            {
                RollOut(VVKGrid);
            }
            if (button2.IsEnabled == false)
            {
                RollOut(NewsGrid);
            }
            if (button3.IsEnabled == false)
            {
                RollOut(BlackboardGrid);
            }
            if (button4.IsEnabled == false)
            {
                RollOut(MVVGrid);
            }
            if (homeButton.IsEnabled == false)
            {
                RollOut(HomeGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button7.IsEnabled == false)
            {
                RollOut(GamesGrid);
            }

            button1.IsEnabled = true;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button6.IsEnabled = false;
            button7.IsEnabled = true;
            homeButton.IsEnabled = true;

            tb1.Foreground = Brushes.WhiteSmoke;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.Black;
            homeTB.Foreground = Brushes.WhiteSmoke;


            RollIn(RaumplanGrid);
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            NavLabel.Content = (sender as SurfaceButton).Tag;
            if (button1.IsEnabled == false)
            {
                RollOut(VVKGrid);
            }
            if (button2.IsEnabled == false)
            {
                RollOut(NewsGrid);
            }
            if (button3.IsEnabled == false)
            {
                RollOut(BlackboardGrid);
            }
            if (button4.IsEnabled == false)
            {
                RollOut(MVVGrid);
            }
            if (homeButton.IsEnabled == false)
            {
                RollOut(HomeGrid);
            }
            if (button6.IsEnabled == false)
            {
                RollOut(RaumplanGrid);
            }
            if (button7.IsEnabled == false)
            {
                RollOut(GamesGrid);
            }

            button1.IsEnabled = true;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = false;
            homeButton.IsEnabled = true;

            tb1.Foreground = Brushes.WhiteSmoke;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.Black;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.WhiteSmoke;



            RollIn(GamesGrid);
        }

        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            
        foreach (Process clsProcess in Process.GetProcesses()) {

                

                if (clsProcess.ProcessName.StartsWith("WindowsGame2"))

                {
                    clsProcess.Kill();
                       

                }
                if (clsProcess.ProcessName.StartsWith("Airhockey"))
                {
                    clsProcess.Kill();


                }
                if (clsProcess.ProcessName.StartsWith("AnimatedSpriteWindows"))
                {
                    clsProcess.Kill();


                }
               

        }




        string executablePath = (sender as SurfaceButton).Tag.ToString();
            try
            {
                if (executablePath.Equals("Airhockey\\Airhockey.exe"))
                {
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(executablePath);
                    System.Diagnostics.Process.Start(info);
                }
                else
                {
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(executablePath);
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    info.CreateNoWindow = false;

                    info.UseShellExecute = true;
                    info.Verb = "open";


                    System.Diagnostics.Process.Start(info);
                }
            }
            catch {}
            
        }

        
        private void impressum_click(object sender, RoutedEventArgs e)
        {
            Canvas impressum=new Canvas();
            impressum.HorizontalAlignment=HorizontalAlignment.Center;
            impressum.VerticalAlignment=VerticalAlignment.Center;
            
            impressum.Width=300;
            impressum.Height=380;
            //FileStream fs = new FileStream("C:\\Users\\FHMuc\\Desktop\\credits\\credits.rtf", FileMode.Open, FileAccess.Read);
            impressum.HorizontalAlignment = HorizontalAlignment.Center;
            impressum.VerticalAlignment = VerticalAlignment.Center;
            //TextRange RTBText = new TextRange(impressum.Document.ContentStart, impressum.Document.ContentEnd);
            //RTBText.Load(fs, DataFormats.Rtf);
            Impressum2 imp2 = new Impressum2();
            imp2.Mainwindow = this;
            impressum.Children.Add(imp2);
            //ScatterViewItem impressum = new ScatterViewItem();
            impressum.Width = 300;
            impressum.Height = 380;
            //impressum.Orientation = 0;
            //impressum.Center = new Point(960, 540);
           
            //bg.GradientOrigin = new Point(0.5, 0.5);
            //bg.Center = new Point(0.5, 0.5);



            impressum.Background = Brushes.DarkGray;
            
            DropShadowEffect dse = new DropShadowEffect();
            dse.BlurRadius = 15;
            impressum.Effect = dse;
            //Impressum2 imp2 = new Impressum2();
            //imp2.Mainwindow=this;
            //impressum.Content = imp2;
            BlurEffect MainBlur= new BlurEffect();
            this.MainGrid.Effect = MainBlur;
            
            MainBlur.Radius = 10;
            DoubleAnimation blurAnim = new DoubleAnimation(0, 10, new Duration(TimeSpan.FromSeconds(2)),FillBehavior.Stop);
            MainBlur.BeginAnimation(BlurEffect.RadiusProperty, blurAnim);
            
            BackGrid.Children.Add(impressum);
            //impressum.Center = new Point(960, 540);
            //impressum.CanMove = false;
            //impressum.CanScale = false;
            //impressum.CanRotate = false;
            //impressum.UpdateLayout();
            impressum.Opacity = 1;
            DoubleAnimation opacAnim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(2)),FillBehavior.Stop);
            impressum.BeginAnimation(Canvas.OpacityProperty, opacAnim);
            this.MainGrid.IsEnabled = false;
        }

        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            
            Canvas helpCanvas= new Canvas();
            RadialGradientBrush bg = new RadialGradientBrush();
            bg.GradientOrigin = new Point(0.5, 0.5);
            bg.Center = new Point(0.5, 0.5);
            bg.GradientStops.Add(new GradientStop(Colors.DarkGray,1));
            bg.GradientStops.Add(new GradientStop(Colors.LightGray,.5));
            bg.GradientStops.Add(new GradientStop(Colors.WhiteSmoke,0));
            DropShadowEffect dse = new DropShadowEffect();
            dse.BlurRadius = 15;
            helpCanvas.Effect = dse;
            bg.Freeze();
            helpCanvas.Background = Brushes.Transparent;
            MainGrid.IsEnabled = false;
            BackGrid.Children.Add(helpCanvas);
            helpCanvas.Width = 1280;
            helpCanvas.Height = 720;
            helpCanvas.PreviewTouchDown += new EventHandler<TouchEventArgs>(helpCanvas_PreviewTouchDown);
            helpCanvas.PreviewMouseDown +=new MouseButtonEventHandler(helpCanvas_PreviewMouseDown);
            Rectangle rtb1 = new Rectangle();
            rtb1.HorizontalAlignment =HorizontalAlignment.Center;
            rtb1.VerticalAlignment = VerticalAlignment.Center;
            rtb1.Width=1280;
            rtb1.Height=720;

            
                //FileStream fs = new FileStream("Documents\\Info\\Anleitung\\" + NavLabel.Content.ToString() + ".rtf", FileMode.Open, FileAccess.Read);
                //TextRange RTBText = new TextRange(rtb1.Document.ContentStart, rtb1.Document.ContentEnd);
                //RTBText.Load(fs, DataFormats.Rtf);
                
                MediaPlayer mp = new MediaPlayer();
                mp.Open(new Uri("Videos\\"+ NavLabel.Content.ToString() + "Tutorial.mp4", UriKind.Relative));
            
                DrawingBrush MyForeground = new DrawingBrush(new VideoDrawing { Player = mp, Rect = new Rect(0, 0, 640, 480) });
                MyForeground.Stretch = Stretch.UniformToFill;
                
                mp.Play();
                rtb1.Fill = MyForeground;
                helpCanvas.Children.Add(rtb1);
                mp.MediaEnded += new EventHandler(mp_MediaEnded);
                rtb1.RadiusX = 20;
                rtb1.RadiusY = 20;
            }

        void mp_MediaEnded(object sender, EventArgs e)
        {
            BackGrid.Children.Clear();
            MainGrid.IsEnabled = true;
        }
            

        void helpCanvas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            BackGrid.Children.Clear();
            MainGrid.IsEnabled = true;
            
        }
        void helpCanvas_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            BackGrid.Children.Clear();
            MainGrid.IsEnabled = true;
            

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try { myConnection.Close(); }
                catch { }
                string existingKey = "";
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT * FROM `VVKUserDatabase` WHERE cardkey=@cardkey";
                command.Parameters.AddWithValue("@cardkey", cardReader.CardKey);
                MySqlDataReader Reader;
                myConnection.Open();
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    try
                    {
                        existingKey = Reader.GetValue(0).ToString();
                    }
                    catch { existingKey = ""; }

                }
                myConnection.Close();

                if (existingKey.Equals(""))
                {
                    MessageBox.Show("Karte ist nicht bekannt");
                }
                else
                {
                    try { myConnection.Close(); }
                    catch { }
                    string myDeleteQuery = "DELETE FROM VVKUserDatabase WHERE cardkey=@cardkey";
                    
                    MySqlCommand myCommand2 = new MySqlCommand(myDeleteQuery);
                    myCommand2.Parameters.AddWithValue("@cardkey", cardReader.CardKey);
                    myCommand2.Connection = myConnection;
                    myConnection.Open();
                    myCommand2.ExecuteNonQuery();
                    myCommand2.Connection.Close();
                    MessageBox.Show("Gelöscht");
                }
            }
            catch { MessageBox.Show("Datenbank-Fehler"); }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            RollOut(RegistrationGrid);
            RollOut(VVKGrid);


            button1.IsEnabled = true;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            homeButton.IsEnabled = false;

            tb1.Foreground = Brushes.WhiteSmoke;
            tb2.Foreground = Brushes.WhiteSmoke;
            tb3.Foreground = Brushes.WhiteSmoke;
            tb4.Foreground = Brushes.WhiteSmoke;
            tb7.Foreground = Brushes.WhiteSmoke;
            tb6.Foreground = Brushes.WhiteSmoke;
            homeTB.Foreground = Brushes.Black;

            blackboardControl.likeButton.IsEnabled = true;
            RollIn(HomeGrid);
        }

        private void moveToggle_Checked(object sender, RoutedEventArgs e)
        {
            KeyboardSVI.CanMove = false;
            KeyboardSVI.CanScale = false;
            moveToggle.Foreground = Brushes.Black;
        }

        private void moveToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            KeyboardSVI.CanScale = true;
            KeyboardSVI.CanMove = true;
            moveToggle.Foreground = Brushes.WhiteSmoke;
        }
        public void ShowQR(string message)
        {
            
            MainGrid.IsEnabled = false;
            QRCodeImage qrImg = new QRCodeImage(message);
            qrImg.Message = message;
            qrImg.Width = 600;
            qrImg.Height = 600;
            qrImg.HorizontalAlignment = HorizontalAlignment.Center;
            qrImg.VerticalAlignment = VerticalAlignment.Center;
            Canvas helpCanvas = new Canvas();
            BackGrid.Children.Add(helpCanvas);
            helpCanvas.Width = 600;
            helpCanvas.Height = 600;
            helpCanvas.Children.Add(qrImg);
            helpCanvas.HorizontalAlignment = HorizontalAlignment.Center;
            helpCanvas.VerticalAlignment = VerticalAlignment.Center;
            helpCanvas.PreviewTouchDown += new EventHandler<TouchEventArgs>(helpCanvas_PreviewTouchDown);
            helpCanvas.PreviewMouseDown += new MouseButtonEventHandler(helpCanvas_PreviewMouseDown);
        }

        private void usernameBox_KeyDown(object sender, KeyEventArgs e)
        {
    
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                submitButton_Click(null, null);
            }
        }

        private void SurfaceWindow_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            janitor.timer.Stop();
            janitor.timer.Start();
            //aborted = false;
            //if (freezeTimer == null)
            //{
            //    freezeTimer = new DispatcherTimer();
            //    freezeTimer.Interval = TimeSpan.FromSeconds(10);
            //    freezeTimer.Tick += new EventHandler(freezeTimer_Elapsed);
            //    freezeTimer.Start();
            //    //statusLabel.Content="timer started";
                
            //}

        }

     
       

        private void SurfaceWindow_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            //aborted = false;
            if (Math.Abs(touchPos.X - e.GetTouchPoint(this).Position.X) > 2 || Math.Abs(touchPos.Y - e.GetTouchPoint(this).Position.Y) > 2)
            {
                ////MessageBox.Show("moved");
                //freezeTimer.Stop();
                //freezeTimer.Interval = TimeSpan.FromSeconds(10);
                //freezeTimer.Start();
                //statusLabel.Content = (touchPos.ToString() + " != " + e.GetTouchPoint(this).Position.ToString());
                janitor.timer.Stop();
                janitor.timer.Interval = TimeSpan.FromMinutes(5);
                janitor.timer.Start();
            }
            //else {

            //    if (freezeTimer != null)
            //    {
            //        freezeTimer.Start();
            //    }
            //    }
                
            

            touchPos = e.GetTouchPoint(this).Position;

        }


 

       

        private void SurfaceWindow_ContentRendered(object sender, EventArgs e)
        {
            CloseSplashScreen();
        }

        private void SurfaceWindow_Activated(object sender, EventArgs e)
        {
            
            if (!firstRender)
            {
                App.splashWindow.Progress(100);
                App.splashWindow.StatusUpdate("Hauptfenster wird gezeichnet");
            }
            
        }


        public void CloseSplashScreen()
        {
            if (firstRender)
            {
                
                //this.Hide();
                this.Show();
                this.Activate();
                this.UpdateLayout();
                App.splashWindow.LoadComplete();


                firstRender = false;
            }
        }

        
        

    }



}
