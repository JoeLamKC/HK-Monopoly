using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HKMonopoly
{
    /// <summary>
    /// TestPage.xaml 的互動邏輯
    /// </summary>
    public partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
            startMenu = new StartMenu();
            mainGame = new MainGame();
            TestFrame.NavigationService.Navigate(startMenu);
            //ScrollView.ScrollToHorizontalOffset(this.Width / 2);
            //ScrollView.ScrollToVerticalOffset(this.Height / 2);
        }

        double x = 0;
        double y = 0;
        public static StartMenu startMenu;
        public static MainGame mainGame;

        private void ScrollView_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ScrollView.ScrollToHorizontalOffset(ScrollView.HorizontalOffset + (x - Mouse.GetPosition(ScrollView).X));
                ScrollView.ScrollToVerticalOffset(ScrollView.VerticalOffset + (y - Mouse.GetPosition(ScrollView).Y));
                x = Mouse.GetPosition(ScrollView).X;
                y = Mouse.GetPosition(ScrollView).Y;
            }

        }

        private void ScrollView_PreviewMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            x = Mouse.GetPosition(ScrollView).X;
            y = Mouse.GetPosition(ScrollView).Y;
        }
    }
}
