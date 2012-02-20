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
using MyStik;
using System.Windows.Threading;
using System.Timers;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        MediaPlayer player = new MediaPlayer();
        public Clock()
        {
            InitializeComponent();
            Timer timer= new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed+=new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            
        }
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                seconds.Angle = DateTime.Now.Second * 6;
                minutes.Angle = DateTime.Now.Minute * 6;
                hours.Angle = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5);
                secondBlur.Direction = -((DateTime.Now.Second * 6));
                minuteBlur.Direction = -((DateTime.Now.Minute * 6));
                hourBlur.Direction = -(((DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5)));
            }));
        }

        private void Grid_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            try
            {
                string speechstring = "http://translate.google.com/translate_tts?tl=de&q=" + "Es ist jetzt " + DateTime.Now.ToShortTimeString();
                player = new MediaPlayer();
                player.Open(new Uri(speechstring));
                player.Volume = 1.0f;

                player.Play();
            }
            catch { }
            
        }

        

    }
}
