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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Microsoft.Surface.Presentation.Controls;
using MyStik;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for Screensaver.xaml
    /// </summary>
    public partial class Screensaver : UserControl
    {
        Dictionary<int, object> d = new Dictionary<int, object>();
        Random rand = new Random();
        private delegate void DummyDelegate();
        private Janitor _parentJanitor;
        public Janitor ParentJanitor
        {
            get { return _parentJanitor; }
            set { _parentJanitor = value; }
        }
        public Screensaver()
        {
            App.splashWindow.StatusUpdate("Screensaver wird initialisiert");
            InitializeComponent();
            
            System.Timers.Timer timer = new System.Timers.Timer();

            timer.Elapsed += timer_Tick;

            timer.Interval = 2000;
            timer.Start();

            d.Add(0, "VVK");
            d.Add(1, "Games");
            d.Add(2, "MVV");
            d.Add(3, "Schwarzes Brett");
            d.Add(4, "News");
            d.Add(5, "Raumplan");


            DoubleAnimation OpacityAnim = new DoubleAnimation(.2, 1, new Duration(TimeSpan.FromSeconds(1)), FillBehavior.Stop);
            OpacityAnim.AutoReverse = true;
            OpacityAnim.RepeatBehavior = RepeatBehavior.Forever;
            startLabel.BeginAnimation(Label.OpacityProperty, OpacityAnim);

            DoubleAnimation OpacityAnim2 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.5)), FillBehavior.HoldEnd);

            OpacityAnim2.Completed+=new EventHandler(OpacityAnim2_Completed2);
            mainWindow.BeginAnimation(Label.OpacityProperty, OpacityAnim2);
            App.splashWindow.StatusUpdate("Screensaver ist bereit");
        }
        void timer_Tick(object sender, EventArgs e)
        {

            this.ParentJanitor.ScreenSaverRuns = true;
            this.Dispatcher.Invoke(
System.Windows.Threading.DispatcherPriority.Normal,
(DummyDelegate)
delegate
{
    ScatterViewItem svi = new ScatterViewItem();
    //Viewbox svi = new Viewbox();
    Viewbox vb = new Viewbox();
    Label label = new Label();

    label.Content = d[rand.Next(0, d.Count)];
    label.Foreground = new SolidColorBrush(Colors.White);
    label.Width = 200;
    label.Height = 50;
    label.FontSize = 20;
    label.FontWeight = FontWeights.ExtraLight;

    label.HorizontalContentAlignment = HorizontalAlignment.Center;
    vb.Child = label;
    svi.Content = vb;
    vb.IsEnabled = false;
    svi.Orientation = 0;
    svi.Opacity = 0;
    svi.Width = 4000;
    svi.Height = 1000;

    BlurEffect blur = new BlurEffect();
    blur.Radius = 50;
    blur.RenderingBias = RenderingBias.Performance;
    blur.KernelType = KernelType.Gaussian;
    svi.Effect = blur;

    svi.ApplyTemplate();
    svi.Background = new SolidColorBrush(Colors.Transparent);
    svi.ShowsActivationEffects = false;
    Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;
    ssc = svi.Template.FindName("shadow", svi) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
    ssc.Visibility = Visibility.Hidden;
    svi.BorderBrush = Brushes.Transparent;
    svi.IsHitTestVisible = false;
    svi.ClipToBounds = false;


    DoubleAnimation blurAnim = new DoubleAnimation(20, 10, new Duration(TimeSpan.FromSeconds(4)), FillBehavior.Stop);
    blurAnim.AutoReverse = true;

    blur.BeginAnimation(BlurEffect.RadiusProperty, blurAnim, HandoffBehavior.Compose);

    DoubleAnimation WidthAnim = new DoubleAnimation(200, 4000, new Duration(TimeSpan.FromSeconds(8)), FillBehavior.Stop);
    WidthAnim.Completed += new EventHandler(WidthAnim_Completed);

    svi.BeginAnimation(ScatterViewItem.WidthProperty, WidthAnim, HandoffBehavior.Compose);

    DoubleAnimation HeightAnim = new DoubleAnimation(50, 1000, new Duration(TimeSpan.FromSeconds(8)), FillBehavior.Stop);

    svi.BeginAnimation(ScatterViewItem.HeightProperty, HeightAnim, HandoffBehavior.Compose);

    DoubleAnimation OpacityAnim = new DoubleAnimation(0, .5, new Duration(TimeSpan.FromSeconds(4)), FillBehavior.Stop);
    OpacityAnim.AutoReverse = true;

    svi.BeginAnimation(ScatterViewItem.OpacityProperty, OpacityAnim, HandoffBehavior.Compose);

    //sb.Children.Add(blurAnim);
    //sb.Children.Add(WidthAnim);
    //sb.Children.Add(HeightAnim);
    //sb.Children.Add(OpacityAnim);
    //Storyboard.SetTarget(blurAnim, blur);
    //Storyboard.SetTarget(WidthAnim, svi);
    //Storyboard.SetTarget(HeightAnim, svi);
    //Storyboard.SetTarget(OpacityAnim, svi);
    //Storyboard.SetTargetProperty(blurAnim, new PropertyPath(BlurEffect.RadiusProperty));
    //Storyboard.SetTargetProperty(WidthAnim, new PropertyPath(ScatterViewItem.WidthProperty));
    //Storyboard.SetTargetProperty(HeightAnim, new PropertyPath(ScatterViewItem.HeightProperty));
    //Storyboard.SetTargetProperty(OpacityAnim, new PropertyPath(ScatterViewItem.OpacityProperty));
    //sb.Begin();
    MainSV.Items.Add(svi);


});




        }
        void WidthAnim_Completed(object sender, EventArgs e)
        {
            MainSV.Items.RemoveAt(0);

        }
        private void SurfaceWindow_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            DoubleAnimation OpacityAnim2 = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(.5)), FillBehavior.HoldEnd);
            OpacityAnim2.Completed += new EventHandler(OpacityAnim2_Completed);
            this.ParentJanitor.Hauptformular.MainGrid.Visibility = Visibility.Visible;
            mainWindow.BeginAnimation(Label.OpacityProperty, OpacityAnim2);

        }

        void OpacityAnim2_Completed2(object sender, EventArgs e)
        {
            this.ParentJanitor.Hauptformular.MainGrid.Visibility = Visibility.Hidden;
            
        }
        void OpacityAnim2_Completed(object sender, EventArgs e)
        {
            
            this.ParentJanitor.Hauptformular.ScreensaverGrid.Children.Clear();
            this.ParentJanitor.ScreenSaverRuns = false;
        }
    }
}
