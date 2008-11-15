using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightOyunum
{
    public partial class Page : UserControl
    {
        System.Windows.Threading.DispatcherTimer Sayac;
        System.Windows.Threading.DispatcherTimer DusmanSayac;
        int SkorPoint;
        public Canvas MyParent { get; set; }

        public Page(Canvas Parent)
        {
            InitializeComponent();
            MyParent = Parent;
            this.Loaded += new RoutedEventHandler(Page_Loaded);
            this.MouseMove += new MouseEventHandler(Page_MouseMove);
            this.Roket.MouseLeftButtonDown += new MouseButtonEventHandler(Roket_MouseLeftButtonDown);
        }

        void Roket_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle Kursun = new Rectangle();
            Kursun.Height = 5;
            Kursun.Width = 10;
            Kursun.RadiusX = 5;
            Kursun.RadiusY = 5;
            Kursun.Stroke  = new SolidColorBrush(Colors.Blue);
            Kursun.Fill = new SolidColorBrush(Colors.Cyan);
            Canvas.SetLeft(Kursun, Canvas.GetLeft(Roket) + Roket.ActualWidth);
            Canvas.SetTop(Kursun, Canvas.GetTop(Roket) + (Roket.ActualHeight / 2));
            Kursunlar.Children.Add(Kursun);
        }

        void Page_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetLeft(Roket, e.GetPosition(this).X - (Roket.ActualWidth/2));
            Canvas.SetTop(Roket, e.GetPosition(this).Y - (Roket.ActualHeight/2));
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SkorPoint = 0;
            Skor.Text = "Vurulan Gemi: 0";
            Sayac = new System.Windows.Threading.DispatcherTimer();
            Sayac.Interval = new TimeSpan(0, 0, 0, 0, 50);
            Sayac.Tick += new EventHandler(Sayac_Tick);
            Sayac.Start();
            FonAnim.Begin();

            DusmanSayac = new System.Windows.Threading.DispatcherTimer();
            DusmanSayac.Interval = new TimeSpan(0, 0, 0,8);
            DusmanSayac.Tick += new EventHandler(DusmanSayac_Tick);
            DusmanSayac.Start();
        }

        void DusmanSayac_Tick(object sender, EventArgs e)
        {
            Random RastGele = new Random();
            DusmanControl YeniDusman = new DusmanControl(Dusmanlar, DKursunlar);
            Canvas.SetLeft(YeniDusman, this.ActualWidth);
            Canvas.SetTop(YeniDusman, RastGele.Next(0, (int)(this.ActualHeight - YeniDusman.ActualHeight)));
            Dusmanlar.Children.Add(YeniDusman);

            Storyboard Anim = new Storyboard();
            DoubleAnimationUsingKeyFrames DBL = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(DBL, YeniDusman);
            Storyboard.SetTargetProperty(DBL, new PropertyPath(Canvas.TopProperty));

            SplineDoubleKeyFrame KF = new SplineDoubleKeyFrame();
            KF.KeyTime = new TimeSpan(0, 0, 0);
            KF.Value = Canvas.GetTop(YeniDusman);
            DBL.KeyFrames.Add(KF);
            KF = new SplineDoubleKeyFrame();
            KF.KeyTime = new TimeSpan(0, 0, RastGele.Next(2,4));
            KF.Value = 250;
            DBL.KeyFrames.Add(KF);
            KF = new SplineDoubleKeyFrame();
            KF.KeyTime = new TimeSpan(0, 0, RastGele.Next(4, 8));
            KF.Value = 0;
            DBL.KeyFrames.Add(KF);
            KF = new SplineDoubleKeyFrame();
            KF.KeyTime = new TimeSpan(0, 0, RastGele.Next(8, 14));
            KF.Value = Canvas.GetTop(YeniDusman);
            DBL.KeyFrames.Add(KF);
            DBL.RepeatBehavior = RepeatBehavior.Forever;
            Anim.Children.Add(DBL);
            Anim.Begin();

            //DusmanSayac.Stop();
            DusmanSayac.Interval = DusmanSayac.Interval.Subtract(new TimeSpan(0, 0, 0, 0, 100));
        }

        void Sayac_Tick(object sender, EventArgs e)
        {
            for (int x = 0; x < Kursunlar.Children.Count; x++)
            {
                if (Canvas.GetLeft(Kursunlar.Children[x]) < this.ActualWidth)
                {
                    Rectangle AnkiKursun = (Rectangle)Kursunlar.Children[x];
                    Canvas.SetLeft(AnkiKursun, Canvas.GetLeft(AnkiKursun) + 10);
                }
            }

            for (int x = 0; x < DKursunlar.Children.Count; x++)
            {
                if (Canvas.GetLeft(DKursunlar.Children[x]) > -10)
                {
                    Rectangle AnkiKursun = (Rectangle)DKursunlar.Children[x];
                    Canvas.SetLeft(AnkiKursun, Canvas.GetLeft(AnkiKursun) - 10);
                }
            }

            for (int x = 0; x < Dusmanlar.Children.Count; x++)
            {
                DusmanControl AnlikDusman = (DusmanControl)Dusmanlar.Children[x];
                if (Canvas.GetLeft(Dusmanlar.Children[x]) > 200 )
                {
                    Canvas.SetLeft(AnlikDusman, Canvas.GetLeft(AnlikDusman) - 3);
                }
                if (AnlikDusman.Patladi == false)
                {
                    var Sonuclar = from gelenler in Kursunlar.Children where Canvas.GetLeft(gelenler) < this.ActualWidth && Canvas.GetLeft(gelenler) > Canvas.GetLeft(AnlikDusman) && Canvas.GetLeft(gelenler) < (Canvas.GetLeft(AnlikDusman) + AnlikDusman.ActualWidth) && Canvas.GetTop(gelenler) > Canvas.GetTop(AnlikDusman) && Canvas.GetTop(gelenler) < (Canvas.GetTop(AnlikDusman) + AnlikDusman.ActualHeight) select gelenler;
                    if (Sonuclar.Count() > 0)
                    {
                        AnlikDusman.Patlat();
                        SkorPoint += 1;
                        Skor.Text = "Vurulan Gemi:" + SkorPoint.ToString();
                    }
                }
            }

            var GelenKursunlar = from gelenler in DKursunlar.Children where Canvas.GetLeft(gelenler) > Canvas.GetLeft(Roket) && Canvas.GetLeft(gelenler) < (Canvas.GetLeft(Roket) + Roket.ActualWidth) && Canvas.GetTop(gelenler) > Canvas.GetTop(Roket) && Canvas.GetTop(gelenler) < (Canvas.GetTop(Roket) + Roket.ActualHeight) select gelenler;
            if (GelenKursunlar.Count() > 0)
            {
                this.Visibility = Visibility.Collapsed;
                Sayac.Stop();
                DusmanSayac.Stop();
                MyParent.Children.Clear();
                MyParent.Children.Add(new olu(MyParent, SkorPoint));
            }

        }
    }
}
