using System;
using System.Windows.Controls;
using System.Windows.Input;

/************************************************
 * This class is responsible for the view of the game
 * 
 * It acts as the console of app mode which used to hold the main game window
 * 
 * It provide a drag scrolling for scrolling through the whole map
 * map is as same as the Main
 * 
 ***********************************************/

namespace HKMonopoly
{

    public partial class AppModeGameConsole : Page
    {
        double x = 0;
        double y = 0;
        public static StartMenu startMenu;
        public static MainGame mainGame;

        public AppModeGameConsole()
        {
            InitializeComponent();
            startMenu = new StartMenu();
            mainGame = new MainGame();
            AppMode.startMenu = startMenu;
            AppMode.mainGame = mainGame;
            ConsoleFrame.NavigationService.Navigate(startMenu);

            ScrollView.ScrollToHorizontalOffset(343);
            ScrollView.ScrollToVerticalOffset(234);
        }

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
