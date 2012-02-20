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
using System.Windows.Media.Media3D;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Controls.Primitives;
using System.Windows.Media.Effects;
using System.Threading;
using MyStik;


namespace myUserControls
{
    /// <summary>
    /// Interaction logic for Raumplan.xaml
    /// </summary>
    public partial class Raumplan : UserControl
    {
        
      
        //ManipulationProcessor _processor = new ManipulationProcessor(ProcessorManipulations.ALL);
        
        //private readonly Windows7.Multitouch.GestureHandler _gestureHandler;
        bool Is3DMode = false;
        SolidColorBrush red_brush = new SolidColorBrush();
        SolidColorBrush colorbrush = new SolidColorBrush();
        Color highlighted_green = new Color();
        Color hm_red = new Color();
        
        //double initialPositionX = 0;
        //double initialPositionY = 0;
        Path selectedRoom;
        private SurfaceWindow1 _hauptformular;
        public SurfaceWindow1 Hauptformular
        {
            get { return _hauptformular; }
            set { _hauptformular = value; }
        }
        public Raumplan()
        {
            App.splashWindow.StatusUpdate("Raumplan wird initialisiert.");
            InitializeComponent();

            //Loaded += (s, e) => { Factory.EnableStylusEvents(Hauptformular); };

            //StylusDown += (s, e) => { _processor.ProcessDown((uint)e.StylusDevice.Id, e.GetPosition(ViewGrid).ToDrawingPointF()); };
            //StylusUp += (s, e) => { _processor.ProcessUp((uint)e.StylusDevice.Id, e.GetPosition(ViewGrid).ToDrawingPointF()); };
            //StylusMove += (s, e) => { _processor.ProcessMove((uint)e.StylusDevice.Id, e.GetPosition(ViewGrid).ToDrawingPointF()); };
            //_processor.ManipulationStarted += (s, e) => { MessageBox.Show("test"); };
            //_processor.ManipulationDelta += ProcessManipulationDelta;
            //_processor.PivotRadius = 0;

           
         
                  btn1.IsChecked = true;
            Stockwerk0.Opacity = 0;
            Stockwerk2.Opacity = 0;
            Stockwerk3.Opacity = 0;
            Stockwerk4.Opacity = 0;
            Stockwerk5.Opacity = 0;
            Stockwerk6.Opacity = 0;

            
            hm_red.A = 255;
            hm_red.R = 211;
            hm_red.G = 1;
            hm_red.B = 46;
            
            red_brush.Color = hm_red;
            
            highlighted_green.A = 255;
            highlighted_green.R = 0;
            highlighted_green.G = 255;
            highlighted_green.B = 0;
            
            colorbrush.Color = highlighted_green;
            App.splashWindow.StatusUpdate("Raumplan ist bereit.");
        }

        
        

        private void ViewGrid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            //Legt die Manipulationsmöglichkeiten des Plans durch Touch-Input fest


            //2D Ansicht: Rotations-Geste dreht Raumplan um Y-Achse, Verschiebe-Geste bewegt den Plan auf der Z-X-Ebene
            if (!Is3DMode)
            {
                rotY.Angle = rotY.Angle - (e.DeltaManipulation.Rotation);
                rotY0.Angle = rotY.Angle;
                rotY2.Angle = rotY.Angle;
                rotY3.Angle = rotY.Angle;
                rotY4.Angle = rotY.Angle;
                rotY5.Angle = rotY.Angle;
                rotY6.Angle = rotY.Angle;

                camOffset.OffsetX = camOffset.OffsetX - 0.002 * e.DeltaManipulation.Translation.X;
                camOffset.OffsetY = camOffset.OffsetY + 0.002 * e.DeltaManipulation.Translation.Y;
            }
            else
            {
            //3D Ansicht: Verschieben in Y-Richtung dreht den Plan um die X-Achse, Verschieben in X-Richtung um die Y-Achse
                rotX.Angle = rotX.Angle + 0.15 * e.DeltaManipulation.Translation.Y;
                rotX0.Angle = rotX.Angle;
                rotX2.Angle = rotX.Angle;
                rotX3.Angle = rotX.Angle;
                rotX4.Angle = rotX.Angle;
                rotX5.Angle = rotX.Angle;
                rotX6.Angle = rotX.Angle;

                rotY.Angle = rotY.Angle + 0.15 * e.DeltaManipulation.Translation.X;
                rotY0.Angle = rotY.Angle;
                rotY2.Angle = rotY.Angle;
                rotY3.Angle = rotY.Angle;
                rotY4.Angle = rotY.Angle;
                rotY5.Angle = rotY.Angle;
                rotY6.Angle = rotY.Angle;
            
            }


                
                //2D und 3D Ansicht: Pinch-Geste verkleinert/vergrößert den Plan durch Verschieben des Kamera-Objekts
                camOffset.OffsetZ = camOffset.OffsetZ - (2 * Math.Log(e.DeltaManipulation.Scale.X));
                
            
            //Festlegen der Grenzen für Manipulationen des Plans. Der Plan springt auf Grenzwert zurück, wenn die Grenze für die jeweilige Manipulation überschritten wurde.
            
            if(Is3DMode)
            {
            if(rotX.Angle >-20)
            {rotX.Angle=-20;}
            if(rotX.Angle <-70)
            { rotX.Angle = -70;}
            }

            if (camOffset.OffsetX > 1.6)
            {camOffset.OffsetX=1.6;}
            if (camOffset.OffsetX < -1.6)
            { camOffset.OffsetX = -1.6; }

            if (camOffset.OffsetY > 1.2)
            { camOffset.OffsetY = 1.2; }
            if (camOffset.OffsetY < -1.2)
            { camOffset.OffsetY = -1.2; }

            if (camOffset.OffsetZ > 1.5)
            { camOffset.OffsetZ = 1.5; }
            if (camOffset.OffsetZ < -2)
            { camOffset.OffsetZ = -2; }
              
            

                
                
            
            
        }

        private void ViewGrid_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            //Trägheitseffekte für die Touch-Manipulation des Raumplans
            //siehe auch http://msdn.microsoft.com/de-de/library/ee649090.aspx#Y1710

            
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;

        }

        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            btn0.Foreground = Brushes.Black;
            btn1.Foreground = Brushes.Black;
            btn2.Foreground = Brushes.Black;
            btn3.Foreground = Brushes.Black;
            btn4.Foreground = Brushes.Black;
            btn5.Foreground = Brushes.Black;
            btn6.Foreground = Brushes.Black;
            Is3DMode = true;
            btn0.IsChecked = true;
            btn1.IsChecked = true;
            btn2.IsChecked = true;
            btn3.IsChecked = true;
            btn4.IsChecked = true;
            btn5.IsChecked = true;
            btn6.IsChecked = true;
            camOffset.OffsetX = 0;
            camOffset.OffsetY = 0;
            DoubleAnimation doubleAnimationX = new DoubleAnimation(.1, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationZ = new DoubleAnimation(.4, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationY = new DoubleAnimation(.3, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimation3 = new DoubleAnimation(.2, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationCam = new DoubleAnimation(-.5, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimation0 = new DoubleAnimation(-.1, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimation6 = new DoubleAnimation(.5, new Duration(TimeSpan.FromSeconds(.6)));
            doubleAnimationX.From = 0;
            doubleAnimationY.From = 0;
            doubleAnimationZ.From = 0;
            doubleAnimation3.From = 0;
            doubleAnimation0.From = 0;
            doubleAnimation6.From = 0;
            doubleAnimationCam.From = camOffset.OffsetZ;
            offset0.OffsetZ = -.1;
            offset1.OffsetZ = .1;
            offset2.OffsetZ = .3;
            offset3.OffsetZ = .4;
            offset4.OffsetZ = .2;
            offset5.OffsetZ = .5;
            camOffset.OffsetZ = -.5;
            doubleAnimationX.FillBehavior = FillBehavior.Stop;
            doubleAnimationY.FillBehavior = FillBehavior.Stop;
            doubleAnimationZ.FillBehavior = FillBehavior.Stop;
            doubleAnimation3.FillBehavior = FillBehavior.Stop;
            doubleAnimationCam.FillBehavior = FillBehavior.Stop;
            doubleAnimation0.FillBehavior = FillBehavior.Stop;
            doubleAnimation6.FillBehavior = FillBehavior.Stop;

            offset1.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationX);
            offset2.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationY);
            offset3.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationZ);
            offset4.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimation3);
            camOffset.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationCam);
            offset0.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimation0);
            offset5.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimation6);


            DoubleAnimation doubleAnimationrotX = new DoubleAnimation(-60, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationrotX2 = new DoubleAnimation(-60, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationrotX3 = new DoubleAnimation(-60, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationrotX4 = new DoubleAnimation(-60, new Duration(TimeSpan.FromSeconds(.6)));

            doubleAnimationrotX.From = rotX.Angle;
            doubleAnimationrotX2.From = rotX2.Angle;
            doubleAnimationrotX3.From = rotX3.Angle;
            doubleAnimationrotX4.From = rotX4.Angle;
            doubleAnimationrotX4.From = rotX5.Angle;
            rotX0.Angle = -60;
            rotX.Angle = -60;
            rotX2.Angle = -60;
            rotX3.Angle = -60;
            rotX4.Angle = -60;
            rotX5.Angle = -60;
            rotX6.Angle = -60;
            doubleAnimationrotX.FillBehavior = FillBehavior.Stop;
            doubleAnimationrotX2.FillBehavior = FillBehavior.Stop;
            doubleAnimationrotX3.FillBehavior = FillBehavior.Stop;
            doubleAnimationrotX4.FillBehavior = FillBehavior.Stop;

            
            rotX.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);
            rotX2.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX2);
            rotX3.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX3);
            rotX4.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX4);
            rotX5.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX4);
            rotX0.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX4);
            rotX6.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX4);


            DoubleAnimation doubleAnimationrotY = new DoubleAnimation(-90, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationrotY2 = new DoubleAnimation(-90, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationrotY3 = new DoubleAnimation(-90, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation doubleAnimationrotY4 = new DoubleAnimation(-90, new Duration(TimeSpan.FromSeconds(.6)));
            doubleAnimationrotY.From = rotY.Angle;
            doubleAnimationrotY2.From = rotY2.Angle;
            doubleAnimationrotY3.From = rotY3.Angle;
            doubleAnimationrotY4.From = rotY4.Angle;
            rotY0.Angle = -90;
            rotY.Angle = -90;
            rotY2.Angle = -90;
            rotY3.Angle = -90;
            rotY4.Angle = -90;
            rotY5.Angle = -90;
            rotY6.Angle = -90;
            doubleAnimationrotY.FillBehavior = FillBehavior.Stop;
            doubleAnimationrotY2.FillBehavior = FillBehavior.Stop;
            doubleAnimationrotY3.FillBehavior = FillBehavior.Stop;
            doubleAnimationrotY4.FillBehavior = FillBehavior.Stop;

            rotY.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);
            rotY2.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY2);
            rotY3.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY3);
            rotY4.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY4);
            rotY5.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY4);
            rotY0.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY4);
            rotY6.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY4);

            DoubleAnimation opacityanim1 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation opacityanim2 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation opacityanim3 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation opacityanim4 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(.6)));
            DoubleAnimation opacityanim5 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(.6)));

            opacityanim1.From = Stockwerk1.Opacity;
            opacityanim2.From = Stockwerk2.Opacity;
            opacityanim2.From = Stockwerk3.Opacity;
            opacityanim3.From = Stockwerk4.Opacity;
            opacityanim4.From = Stockwerk5.Opacity;

            Stockwerk0.Opacity = 1;
            Stockwerk1.Opacity = 1;
            Stockwerk2.Opacity = 1;
            Stockwerk3.Opacity = 1;
            Stockwerk4.Opacity = 1;
            Stockwerk5.Opacity = 1;
            Stockwerk6.Opacity = 1;

            opacityanim1.FillBehavior = FillBehavior.Stop;
            opacityanim2.FillBehavior = FillBehavior.Stop;
            opacityanim3.FillBehavior = FillBehavior.Stop;
            opacityanim4.FillBehavior = FillBehavior.Stop;
            opacityanim5.FillBehavior = FillBehavior.Stop;

            Stockwerk1.BeginAnimation(Canvas.OpacityProperty, opacityanim1);
            Stockwerk2.BeginAnimation(Canvas.OpacityProperty, opacityanim2);
            Stockwerk3.BeginAnimation(Canvas.OpacityProperty, opacityanim3);
            Stockwerk4.BeginAnimation(Canvas.OpacityProperty, opacityanim4);
            Stockwerk5.BeginAnimation(Canvas.OpacityProperty, opacityanim5);
            Stockwerk6.BeginAnimation(Canvas.OpacityProperty, opacityanim5);
            Stockwerk0.BeginAnimation(Canvas.OpacityProperty, opacityanim5);

        }

        public void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            Is3DMode = false;
            camOffset.OffsetX = 0;
            camOffset.OffsetY = 0;
            btn0.IsChecked = false;
            btn1.IsChecked = true;
            btn2.IsChecked = false;
            btn3.IsChecked = false;
            btn4.IsChecked = false;
            btn5.IsChecked = false;
            btn6.IsChecked = false;

            btn0.Foreground = Brushes.WhiteSmoke;
            btn1.Foreground = Brushes.Black;
            btn2.Foreground = Brushes.WhiteSmoke;
            btn3.Foreground = Brushes.WhiteSmoke;
            btn4.Foreground = Brushes.WhiteSmoke;
            btn5.Foreground = Brushes.WhiteSmoke;
            btn6.Foreground = Brushes.WhiteSmoke;
            
            offset0.OffsetZ = 0;
            offset1.OffsetZ = 0;
            offset2.OffsetZ = 0;
            offset3.OffsetZ = 0;
            offset4.OffsetZ = 0;
            offset5.OffsetZ = 0;
            
            DoubleAnimation doubleAnimationCam = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.6)));
            
            doubleAnimationCam.From = camOffset.OffsetZ;
           
            
            camOffset.OffsetZ = 0;
            
            doubleAnimationCam.FillBehavior = FillBehavior.Stop;
            

            
            camOffset.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationCam);


            DoubleAnimation doubleAnimationrotX = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.6)));
            
            doubleAnimationrotX.From = rotX.Angle;
            
            rotX0.Angle = 0;
            rotX.Angle = 0;
            rotX2.Angle = 0;
            rotX3.Angle = 0;
            rotX4.Angle = 0;
            rotX5.Angle = 0;
            rotX6.Angle = 0;
            doubleAnimationrotX.FillBehavior = FillBehavior.Stop;

            rotX0.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);
            rotX.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);
            rotX2.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);
            rotX3.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);
            rotX4.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);
            rotX5.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);
            rotX6.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotX);

            DoubleAnimation doubleAnimationrotY = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.6)));
            
            doubleAnimationrotY.From = rotY.Angle;
            
            rotY0.Angle = 0;
            rotY.Angle = 0;
            rotY2.Angle = 0;
            rotY3.Angle = 0;
            rotY4.Angle = 0;
            rotY5.Angle = 0;
            rotY6.Angle = 0;
            doubleAnimationrotY.FillBehavior = FillBehavior.Stop;
            

            rotY0.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);
            rotY.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);
            rotY2.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);
            rotY3.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);
            rotY4.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);
            rotY5.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);
            rotY6.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimationrotY);



        }


        private void SurfaceToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            //Ein-/Ausblenden der einzelnen Stockwerke per SurfaceToggleButton
            (sender as SurfaceToggleButton).Foreground = Brushes.Black;
            Canvas animatedObject = new Canvas();
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk0"))
            {
                Stockwerk0.Opacity = 1; animatedObject = Stockwerk0; if (!Is3DMode)
                { 
                    
                    btn1.IsChecked = false;
                    btn2.IsChecked = false;
                    btn3.IsChecked = false;
                    btn4.IsChecked = false;
                    btn5.IsChecked = false;
                    btn6.IsChecked = false;
                }
            }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk1"))
            {
                Stockwerk1.Opacity = 1; animatedObject = Stockwerk1; if (!Is3DMode)
                {
                    btn0.IsChecked = false;
                    
                    btn2.IsChecked = false;
                    btn3.IsChecked = false;
                    btn4.IsChecked = false;
                    btn5.IsChecked = false;
                    btn6.IsChecked = false;
                }
            }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk2"))
            {
                Stockwerk2.Opacity = 1; animatedObject = Stockwerk2; if (!Is3DMode)
                {
                    btn0.IsChecked = false;
                    btn1.IsChecked = false;
                    
                    btn3.IsChecked = false;
                    btn4.IsChecked = false;
                    btn5.IsChecked = false;
                    btn6.IsChecked = false;
                }
            }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk3"))
            {
                Stockwerk3.Opacity = 1; animatedObject = Stockwerk3; if (!Is3DMode)
                {
                    btn0.IsChecked = false;
                    btn1.IsChecked = false;
                    btn2.IsChecked = false;
                    
                    btn4.IsChecked = false;
                    btn5.IsChecked = false;
                    btn6.IsChecked = false;
                }
            }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk4"))
            {
                Stockwerk4.Opacity = 1; animatedObject = Stockwerk4; if (!Is3DMode)
                {
                    btn0.IsChecked = false;
                    btn1.IsChecked = false;
                    btn2.IsChecked = false;
                    btn3.IsChecked = false;
                    
                    btn5.IsChecked = false;
                    btn6.IsChecked = false;
                }
            }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk5"))
            {
                Stockwerk5.Opacity = 1; animatedObject = Stockwerk5; if (!Is3DMode)
                {
                    btn0.IsChecked = false;
                    btn1.IsChecked = false;
                    btn2.IsChecked = false;
                    btn3.IsChecked = false;
                    btn4.IsChecked = false;
                    
                    btn6.IsChecked = false;
                }
            }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk6"))
            {
                Stockwerk6.Opacity = 1; animatedObject = Stockwerk6; if (!Is3DMode)
                {
                    btn0.IsChecked = false;
                    btn1.IsChecked = false;
                    btn2.IsChecked = false;
                    btn3.IsChecked = false;
                    btn4.IsChecked = false;
                    btn5.IsChecked = false;
                    
                }
            }
            DoubleAnimation opacityAnim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.6)), FillBehavior.Stop);
            animatedObject.BeginAnimation(Canvas.OpacityProperty, opacityAnim);
            
        }
        
        private void SurfaceToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            (sender as SurfaceToggleButton).Foreground = Brushes.WhiteSmoke;
            Canvas animatedObject = new Canvas();
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk0")) { Stockwerk0.Opacity = 0; animatedObject = Stockwerk0; }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk1")) { Stockwerk1.Opacity = 0; animatedObject = Stockwerk1; }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk2")) { Stockwerk2.Opacity = 0; animatedObject = Stockwerk2; }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk3")) { Stockwerk3.Opacity = 0; animatedObject = Stockwerk3; }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk4")) { Stockwerk4.Opacity = 0; animatedObject = Stockwerk4; }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk5")) { Stockwerk5.Opacity = 0; animatedObject = Stockwerk5; }
            if ((sender as SurfaceToggleButton).Tag.Equals("Stockwerk6")) { Stockwerk6.Opacity = 0; animatedObject = Stockwerk6; }
            DoubleAnimation opacityAnim = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(.6)), FillBehavior.Stop);
            animatedObject.BeginAnimation(Canvas.OpacityProperty, opacityAnim);
            
        }

        private void SurfaceButton_Click_2(object sender, RoutedEventArgs e)
        {
            resetRooms();
            SurfaceButton_Click_1(sender, e);
            selectedRoom = null;
            btn0.IsChecked = false;
            btn1.IsChecked = false;
            btn2.IsChecked = false;
            btn3.IsChecked = false;
            btn4.IsChecked = false;
            btn5.IsChecked = false;
            btn6.IsChecked = false;
            tbtn0.IsChecked = false;
            tbtn1.IsChecked = false;
            tbtn2.IsChecked = false;
            tbtn3.IsChecked = false;
            tbtn4.IsChecked = false;
            tbtn5.IsChecked = false;
            tbtn6.IsChecked = false;

            Thread t = new Thread(new System.Threading.ThreadStart(delegate()
            {
                
                this.Dispatcher.Invoke(new Action(delegate {
                    string searchString = "R "+roomBox.Text;
                    searchString = searchString.ToUpper();
                    foreach (var currentroom in raumliste0.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(searchString))
                                    {

                                        btn0.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste1.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(searchString))
                                    {

                                        btn1.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste2.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(searchString))
                                    {

                                        btn2.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }


                    }
                    foreach (var currentroom in raumliste3.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(searchString))
                                    {

                                        btn3.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste4.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(searchString))
                                    {

                                        btn4.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste5.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(searchString))
                                    {

                                        btn5.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste6.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(searchString))
                                    {

                                        btn6.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    if (selectedRoom == null)
                    { 
                        roomLabel.Content = "Raum nicht gefunden";
                        roomBox.Text = "";
                    }
                    else
                    {
                        ColorAnimation coloranim = new ColorAnimation();
                        coloranim.From = highlighted_green;
                        coloranim.To = hm_red;
                        coloranim.Duration = new Duration(TimeSpan.FromSeconds(.5));

                        coloranim.AutoReverse = true;
                        coloranim.RepeatBehavior = RepeatBehavior.Forever;
                        colorbrush.BeginAnimation(SolidColorBrush.ColorProperty, coloranim);
                        selectedRoom.Fill = colorbrush;
                        
                        roomLabel.Content = selectedRoom.Tag.ToString();
                        roomBox.Text="";
                    }
                }));
            }));
            t.Start();
        }
        public void findRoom(string roomNumber)
        {
            SurfaceButton_Click_1(null, null);
            resetRooms();
            selectedRoom = null;
            btn0.IsChecked = false;
            btn1.IsChecked = false;
            btn2.IsChecked = false;
            btn3.IsChecked = false;
            btn4.IsChecked = false;
            btn5.IsChecked = false;
            btn6.IsChecked = false;
            Thread t = new Thread(new System.Threading.ThreadStart(delegate()
            {
                
                this.Dispatcher.Invoke(new Action(delegate
                {
                    foreach (var currentroom in raumliste0.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn0.IsChecked = true;
                                        selectedRoom = (currentroom as Path);
                                        
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste1.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn1.IsChecked = true;
                                        selectedRoom = (currentroom as Path);
                                        
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste2.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn2.IsChecked = true;
                                        selectedRoom = (currentroom as Path);
                                        
                                    }
                                }
                            }
                            catch { }
                        }


                    }
                    foreach (var currentroom in raumliste3.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn3.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste4.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn4.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste5.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn5.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste6.Children)
                    {
                        
                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn6.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    if (selectedRoom == null)
                    { roomLabel.Content = "Raum nicht gefunden";
                    roomBox.Text="";
                    }
                    else
                    {
                        ColorAnimation coloranim = new ColorAnimation();
                        coloranim.From = highlighted_green;
                        coloranim.To = hm_red;
                        coloranim.Duration = new Duration(TimeSpan.FromSeconds(.5));

                        coloranim.AutoReverse = true;
                        coloranim.RepeatBehavior = RepeatBehavior.Forever;
                        colorbrush.BeginAnimation(SolidColorBrush.ColorProperty, coloranim);
                        selectedRoom.Fill = colorbrush;
                        
                        roomLabel.Content = selectedRoom.Tag.ToString();
                        roomBox.Text = "";
                    }
                }));
            }));
            t.Start();
            

        }
        public void resetRooms()
        {
            
            if (selectedRoom != null)
            {
                selectedRoom.Fill = red_brush;
            }

        }

        private void roomBox_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            SurfaceButton_Click_1(sender, e);
            
            this.Hauptformular.Call_Keyboard(sender, e);
            
        }

        private void roomBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SurfaceButton_Click_1(sender, e);
            
            this.Hauptformular.Call_Keyboard(sender, e);
            
        }

        private void SurfaceToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {
            resetRooms();
            (sender as SurfaceToggleButton).Foreground = Brushes.Black;
            //SurfaceButton_Click_1(sender, e);
            String roomNumber = (sender as SurfaceToggleButton).Tag.ToString();
            if ((sender as SurfaceToggleButton) == tbtn0)
            {
                //tbtn0.IsChecked = false;
                tbtn1.IsChecked = false;
                tbtn2.IsChecked = false;
                tbtn3.IsChecked = false;
                tbtn4.IsChecked = false;
                tbtn5.IsChecked = false;
                tbtn6.IsChecked = false;

            }
            if ((sender as SurfaceToggleButton) == tbtn1)
            {
                tbtn0.IsChecked = false;
                //tbtn1.IsChecked = false;
                tbtn2.IsChecked = false;
                tbtn3.IsChecked = false;
                tbtn4.IsChecked = false;
                tbtn5.IsChecked = false;
                tbtn6.IsChecked = false;

            }
            if ((sender as SurfaceToggleButton) == tbtn2)
            {
                tbtn0.IsChecked = false;
                tbtn1.IsChecked = false;
                //tbtn2.IsChecked = false;
                tbtn3.IsChecked = false;
                tbtn4.IsChecked = false;
                tbtn5.IsChecked = false;
                tbtn6.IsChecked = false;

            }
            if ((sender as SurfaceToggleButton) == tbtn3)
            {
                tbtn0.IsChecked = false;
                tbtn1.IsChecked = false;
                tbtn2.IsChecked = false;
                //tbtn3.IsChecked = false;
                tbtn4.IsChecked = false;
                tbtn5.IsChecked = false;
                tbtn6.IsChecked = false;

            }
            if ((sender as SurfaceToggleButton) == tbtn4)
            {
                tbtn0.IsChecked = false;
                tbtn1.IsChecked = false;
                tbtn2.IsChecked = false;
                tbtn3.IsChecked = false;
                //tbtn4.IsChecked = false;
                tbtn5.IsChecked = false;
                tbtn6.IsChecked = false;

            }
            if ((sender as SurfaceToggleButton) == tbtn5)
            {
                tbtn0.IsChecked = false;
                tbtn1.IsChecked = false;
                tbtn2.IsChecked = false;
                tbtn3.IsChecked = false;
                tbtn4.IsChecked = false;
                //tbtn5.IsChecked = false;
                tbtn6.IsChecked = false;

            }
            if ((sender as SurfaceToggleButton) == tbtn6)
            {
                tbtn0.IsChecked = false;
                tbtn1.IsChecked = false;
                tbtn2.IsChecked = false;
                tbtn3.IsChecked = false;
                tbtn4.IsChecked = false;
                tbtn5.IsChecked = false;
                //tbtn6.IsChecked = false;

            }
            Thread t = new Thread(new System.Threading.ThreadStart(delegate()
            {

                this.Dispatcher.Invoke(new Action(delegate
                {
                    foreach (var currentroom in raumliste0.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn0.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste1.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn1.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste2.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn2.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }


                    }
                    foreach (var currentroom in raumliste3.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn3.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste4.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn4.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste5.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn5.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste6.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn6.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    if (selectedRoom == null)
                    {
                        roomLabel.Content = "Raum nicht gefunden";
                        roomBox.Text = "";
                    }
                    else
                    {
                        ColorAnimation coloranim = new ColorAnimation();
                        coloranim.From = Colors.Blue;
                        coloranim.To = hm_red;
                        coloranim.Duration = new Duration(TimeSpan.FromSeconds(.5));

                        coloranim.AutoReverse = true;
                        coloranim.RepeatBehavior = RepeatBehavior.Forever;
                        colorbrush.BeginAnimation(SolidColorBrush.ColorProperty, coloranim);
                        selectedRoom.Fill = colorbrush;

                        roomLabel.Content = selectedRoom.Tag.ToString();
                        roomBox.Text = "";
                    }
                }));
            }));
            t.Start();
        }

        private void SurfaceToggleButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            String roomNumber = (sender as SurfaceToggleButton).Tag.ToString();

            (sender as SurfaceToggleButton).Foreground = Brushes.WhiteSmoke;
            Thread t = new Thread(new System.Threading.ThreadStart(delegate()
            {

                this.Dispatcher.Invoke(new Action(delegate
                {
                    foreach (var currentroom in raumliste0.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn0.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste1.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn1.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste2.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn2.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }


                    }
                    foreach (var currentroom in raumliste3.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn3.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste4.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn4.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste5.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn5.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    foreach (var currentroom in raumliste6.Children)
                    {

                        if (currentroom.GetType().ToString().Equals("System.Windows.Shapes.Path"))
                        {
                            try
                            {
                                if (!((currentroom as Path).Tag == null))
                                {
                                    if ((currentroom as Path).Tag.ToString().Equals(roomNumber))
                                    {

                                        btn6.IsChecked = true;
                                        selectedRoom = (currentroom as Path);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    if (selectedRoom == null)
                    {
                        roomLabel.Content = "Raum nicht gefunden";
                        roomBox.Text = "";
                    }
                    else
                    {
                        
                        selectedRoom.Fill = red_brush;

                        roomLabel.Content = selectedRoom.Tag.ToString();
                        roomBox.Text = "";
                    }
                }));
            }));
            t.Start();

        }

        private void roomBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SurfaceButton_Click_2(null, null);
            }
        }

        

        
    
    }
}
