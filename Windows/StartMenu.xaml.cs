using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

/************************************************
 * This class is responsible for being the start menu of the GUI mode
 * It contains the world most gorgeous and epoch-marking interface of start button
 * 
 * This start menu is used to initialize the game and also asking for the number of player in the game.
 * It helps to redirect to the main game page for GUI mode. 
 * 
 ***********************************************/

namespace HKMonopoly
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Page
    {
        public StartMenu StartMenuInstance;
        public MainGame mainGame;
        DispatcherTimer timer = new DispatcherTimer();
        bool startGameTextFlag = true;
        bool inStartingMenu = true;

        public StartMenu()
        {
            InitializeComponent();
            StartMenuInstance = this;
            mainGame = new MainGame();
            timer.Interval = TimeSpan.FromMilliseconds(25);
            timer.Tick += new System.EventHandler(Timer_Tick);
            timer.Start();
        }

        // Display the shining 'Tap to start'
        void Timer_Tick(object sender, EventArgs e)
        {
            if (inStartingMenu == true)
            {
                if (startGameTextFlag == true)
                    startGameText.Opacity -= 0.04;
                else
                    startGameText.Opacity += 0.04;

                if (startGameText.Opacity <= 0 || startGameText.Opacity >= 1)
                    startGameTextFlag = !(startGameTextFlag);
            }
        }

        public void StartMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (inStartingMenu == true)
            {
                MainGameLogic.Instnace.NextGameStage(-1);
            }
        }

        public void ToStateNegativeOne()
        {
            startGameText.Opacity = 0;
            NumberOfPlayerGrid.Opacity = 1;
            inStartingMenu = false;
            Choose2.IsEnabled = true;
            Choose3.IsEnabled = true;
            Choose4.IsEnabled = true;
        }

        private void Choose2_Click(object sender, RoutedEventArgs e)
        {
            MainGameLogic.Instnace.NextGameStage(2);
        }

        private void Choose3_Click(object sender, RoutedEventArgs e)
        {
            MainGameLogic.Instnace.NextGameStage(3);
        }

        private void Choose4_Click(object sender, RoutedEventArgs e)
        {
            MainGameLogic.Instnace.NextGameStage(4);
        }

        public void NavigateToMainGame(MainGame mainGame)
        {
            //NavigationService.Navigate(new MainGame());
            NavigationService.Navigate(mainGame);
        }
    }
}
