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
	public partial class olu : UserControl
	{
        public Canvas MyParent { get; set; }
        public olu(Canvas Parent, int SkorPoint)
		{
			InitializeComponent();
            MyParent = Parent;
            Skor.Text = "Vurulan Gemi:" + SkorPoint.ToString();
            this.Basla.Click += new RoutedEventHandler(Basla_Click);
		}

        void Basla_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            MyParent.Children.Clear();
        }
	}
}