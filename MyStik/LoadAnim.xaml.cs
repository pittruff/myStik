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
using System.Timers;

namespace myUserControls
{
    /// <summary>
    /// Interaction logic for LoadAnim.xaml
    /// </summary>
    public partial class LoadAnim : UserControl
    {
        public LoadAnim()
        {
            InitializeComponent();
            Timer t = new Timer();
            t.Interval = 100;
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate
                {
                    rot.Angle += 45;
                    if (rot.Angle == 360)
                    {
                        rot.Angle = 0;
                    }
                }));
        }
    }
}
