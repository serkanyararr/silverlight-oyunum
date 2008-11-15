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
	public partial class Welcome : UserControl
	{
		public Welcome()
		{
			InitializeComponent();
            this.Basla.Click+=new RoutedEventHandler(Basla_Click);
		}
        void Basla_Click(object sender, RoutedEventArgs e)
        {
            GelGel.Children.Clear();
            GelGel.Children.Add(new Page(GelGel));
        }
	}
}