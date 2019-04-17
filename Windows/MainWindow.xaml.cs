using System;
using System.Windows;

/************************************************
 * This class is responsible to control the main window and
 * to open the other two views (text mode and app mode)
 * 
 ***********************************************/

namespace HKMonopoly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TextMode textWindow;
        public static StartMenu startMenu;
        public static MainGame mainGame;
        public static MainWindow MainWindowInstance;

        public MainWindow()
        {
            InitializeComponent();
            MainWindowInstance = this;
            startMenu = new StartMenu();
            mainGame = new MainGame();
            MainFrame.NavigationService.Navigate(startMenu);
            textWindow = new TextMode();
            textWindow.Show();

            AppMode appWindow = new AppMode();
            appWindow.Show();

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

    }
}
