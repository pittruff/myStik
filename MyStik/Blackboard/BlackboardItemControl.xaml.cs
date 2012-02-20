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
using System.Collections;
using System.Windows.Media.Effects;

namespace myUserControls
{




    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BlackboardItemControl : UserControl
    {

        private ArrayList images = new ArrayList();
        private Random random = new Random();


        private static int SCREEN_WIDTH = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
        private static int SCREEN_HEIGHT = (int)System.Windows.SystemParameters.PrimaryScreenHeight;


        public static DependencyProperty CollageVisibilityProperty = DependencyProperty.Register("CollageVisibility", typeof(Visibility), typeof(BlackboardItemControl));
        public Visibility CollageVisibility
        {
            get { return (Visibility)GetValue(CollageVisibilityProperty); }
            set { SetValue(CollageVisibilityProperty, value); }
        }






        public static DependencyProperty DetailsColumnSpanProperty = DependencyProperty.Register("DetailsColumnSpan", typeof(int), typeof(BlackboardItemControl));
        public int DetailsColumnSpan
        {
            get { return (int)GetValue(DetailsColumnSpanProperty); }
            set { SetValue(DetailsColumnSpanProperty, value); }
        }













        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BlackboardItemControl));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(BlackboardItemControl));
        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set {

                if (value.Equals("gebot"))
                {
                    BgColor = Brushes.Blue;
                    SucheBiete = "Biete";
                }
                else if (value.Equals("gesuch"))
                {
                    BgColor = Brushes.Green;
                    SucheBiete = "Suche";
                }
            

                SetValue(TypeProperty, value); 
            
            
            }
        }


        public static DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(string), typeof(BlackboardItemControl));
        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }



        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(BlackboardItemControl));
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }


        public static DependencyProperty EmailProperty = DependencyProperty.Register("Email", typeof(string), typeof(BlackboardItemControl));
        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }




        public static DependencyProperty TelProperty = DependencyProperty.Register("Tel", typeof(string), typeof(BlackboardItemControl));
        public string Tel
        {
            get { return (string)GetValue(TelProperty); }
            set { SetValue(TelProperty, value); }
        }







        public static DependencyProperty ContactProperty = DependencyProperty.Register("Contact", typeof(string), typeof(BlackboardItemControl));
        public string Contact
        {


            get { return (string)GetValue(ContactProperty); }
            set { SetValue(ContactProperty, value); }

            //string returnString = "";

            //if (!Email.Equals(""))
            //    returnString = "E-Mail: " + Email;

            //if (!Tel.Equals(""))
            //{
            //    if (!returnString.Equals(""))
            //        returnString += ", ";


            //    returnString += "Telefon" + Tel;

            //}



            //if (returnString.Equals(""))
            //    returnString = "Keine Kontaktinformationen";



            //return (string)returnString; 


        }




        public static DependencyProperty DetailsProperty = DependencyProperty.Register("Details", typeof(string), typeof(BlackboardItemControl));
        public string Details
        {
            get { return (string)GetValue(DetailsProperty); }
            set { SetValue(DetailsProperty, value); }
        }






        public static DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(BlackboardItemControl));
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }



        public static DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(int), typeof(BlackboardItemControl));
        public int Orientation
        {
            get { return (int)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }





        //public static DependencyProperty BgColorProperty = DependencyProperty.Register("BgColor", typeof(Brush), typeof(BlackboardControl));
        //public Brush BgColor
        //{
        //    get { return (Brush)GetValue(BgColorProperty); }
        //    set { SetValue(BgColorProperty, value); }
        //}





        public static DependencyProperty BgColorProperty = DependencyProperty.Register("BgColor", typeof(Brush), typeof(BlackboardItemControl));
        public Brush BgColor
        {
            get { return (Brush)GetValue(BgColorProperty); }
            set { SetValue(BgColorProperty, value); }

        }


        //        if (Type.Equals("gebot"))
        //    return Brushes.Blue;
        //else if (Type.Equals("gesuch"))
        //    return Brushes.Green;
        //else return Brushes.Red;




        public static DependencyProperty SucheBieteProperty = DependencyProperty.Register("SucheBiete", typeof(string), typeof(BlackboardItemControl));
        public string SucheBiete
        {
            get { return (string)GetValue(SucheBieteProperty); }
            set { SetValue(SucheBieteProperty, value); }

        }


        ////VNameProperty
        //public static DependencyProperty VNameProperty = DependencyProperty.Register("VName", typeof(string), typeof(Appointment));

        //public string VName
        //{
        //    get { return (string)GetValue(VNameProperty); }
        //    set { SetValue(VNameProperty, value); }
        //}

        public BlackboardItemControl()
        {
            InitializeComponent();

            CollageVisibility = Visibility.Hidden;
            DetailsColumnSpan = 2;

            // random x/y position
            int x = random.Next(300, SCREEN_WIDTH - 400);
            int y = random.Next(200, SCREEN_HEIGHT - 500);

            Center = new Point(x, y);


            //BgColor = Brushes.Red;


            // random orientation

            Orientation = random.Next(-20, 20);




            













        }






        //int imageCount = 0;


        public void addImage(string imagePath)
        {
            CollageVisibility = Visibility.Visible;

            DetailsColumnSpan = 1;



            //images.Add(imagePath);


            //ColumnDefinition colDef = new ColumnDefinition();
            ////colDef.Width = AutoResizedEventArgs;
            //imageGrid.ColumnDefinitions.Add(colDef);




            Grid grid = new Grid();

            grid.Margin = new Thickness(4);




            Border border = new Border();
            border.BorderBrush = Brushes.Black;
            border.Opacity = 0.5;
            border.BorderThickness = new Thickness(2);
            //border.Margin = new Thickness(4);

            grid.Children.Add(border);

            Rectangle rect = new Rectangle();
            rect.Fill = Brushes.WhiteSmoke;
            rect.Margin = new Thickness(1);
            grid.Children.Add(rect);


            Image image = new Image();
            BitmapImage bImage = new BitmapImage();
            bImage.BeginInit();
            bImage.UriSource = new Uri(imagePath);
            bImage.EndInit();
            image.Source = bImage;














            image.Margin = new Thickness(8);




            grid.Children.Add(image);











            RotateTransform rotate = new RotateTransform();
            rotate.Angle = random.Next(-4, 4);
            grid.RenderTransform = rotate;






            //DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            //// Set the color of the shadow to Black.
            //Color myShadowColor = new Color();
            //myShadowColor.ScA = 1;
            //myShadowColor.ScB = 0;
            //myShadowColor.ScG = 0;
            //myShadowColor.ScR = 0;
            //myDropShadowEffect.Color = myShadowColor;

            //// Set the direction of where the shadow is cast to 320 degrees.
            //myDropShadowEffect.Direction = 320;

            //// Set the depth of the shadow being cast.
            //myDropShadowEffect.ShadowDepth = 1;



            //// Set the shadow softness to the maximum (range of 0-1).
            //myDropShadowEffect.Softness = 0.1;
            //// Set the shadow opacity to half opaque or in other words - half transparent.
            //// The range is 0-1.
            //myDropShadowEffect.Opacity = 0.2;

            //// Apply the bitmap effect to the Button.
            //grid.BitmapEffect = myDropShadowEffect; 










            imageStackPanel.Children.Add(grid);
            //imageGrid.Children.Add(image);
            //Grid.SetColumn(imageGrid, imageCount);



            //imageCount++;

            //image.sou





        }

        public void addInfo(string info)
        {

        }



    }
}
