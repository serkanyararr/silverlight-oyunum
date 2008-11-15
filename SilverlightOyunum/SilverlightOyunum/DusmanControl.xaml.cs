using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightOyunum
{
	public partial class DusmanControl : UserControl
	{
        System.Windows.Threading.DispatcherTimer Sayac;

        public Canvas CanvasParent { get; set; }
        public Canvas Kursunluk { get; set; }

        public bool Patladi { get; set; }

		public DusmanControl(Canvas MyParent, Canvas kursunluk)
		{
			InitializeComponent();
            Kursunluk = kursunluk;
            CanvasParent = MyParent;
            Patladi = false;
            Boom.Completed += new EventHandler(Boom_Completed);
            Sayac = new System.Windows.Threading.DispatcherTimer();
            Sayac.Interval = new TimeSpan(0, 0, 0, 1);
            Sayac.Tick += new EventHandler(Sayac_Tick);
            Sayac.Start();
		}

        void Sayac_Tick(object sender, EventArgs e)
        {
            AtesEt();
        }

        void Boom_Completed(object sender, EventArgs e)
        {
            CanvasParent.Children.Remove(this);
        }

        public void Patlat()
        {
            Sayac.Stop();
            Patladi = true;
            Boom.Begin();
        }

        void AtesEt()
        {
            Rectangle Kursun = new Rectangle();
            Kursun.Height = 5;
            Kursun.Width = 10;
            Kursun.RadiusX = 5;
            Kursun.RadiusY = 5;
            Kursun.Stroke = new SolidColorBrush(Colors.Yellow);
            Kursun.Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(Kursun, Canvas.GetLeft(this)-10);
            Canvas.SetTop(Kursun, Canvas.GetTop(this) + (this.ActualHeight / 2));
            Kursunluk.Children.Add(Kursun);
        }
	}
}