using System;
using System.Collections.Generic;
using System.Windows;

/************************************************
 * This class is responsible for the view updating in the App mode
 * 
 * 
 * It has the source, which the the initial front page as the start menu at the left hand corner
 * And different button laying at the right hand side and also
 * player information at the buttom
 * 
 * This class is used to get input from the player and
 * also, responsible for showing the buttons and other views.
 * 
 ***********************************************/

namespace HKMonopoly
{
    /// <summary>
    /// Interaction logic for AppMode.xaml
    /// </summary>
    public partial class AppMode : Window
    {
        double x = 0;
        double y = 0;
        public static StartMenu startMenu;
        public static MainGame mainGame;
        public static AppMode AppModeInstance;

        public AppMode()
        {

            InitializeComponent();
            AppModeInstance = this;
        }

        private void DiceButtonAPP_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("CLICKED!");
            MainGameLogic.Instnace.NextGameStage(-1);
        }

        private void YesButtonAPP_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("YES CLICKED!");
            MainGameLogic.Instnace.NextGameStage(1);
        }

        private void NoButtonAPP_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("NO CLICKED!");
            MainGameLogic.Instnace.NextGameStage(2);
        }

        public void MakeAnnoucement(string s)
        {
            GUIAnnouncementAPP.Opacity = 1;
            GUIAnnouncementAPP.Text = s;
        }

        public void ClearAnnoucement()
        {
            GUIAnnouncementAPP.Opacity = 0;
        }

        public void UpdateDiceResult(int result)
        {
            DiceResultBlockAPP.Text = result + "";
        }


        public void ShowYesNoButtonPair()
        {
            DiceButtonAPP.IsEnabled = false;
            DiceButtonAPP.Opacity = 0;

            YesButtonAPP.Opacity = 1;
            YesButtonAPP.IsEnabled = true;
            NoButtonAPP.Opacity = 1;
            NoButtonAPP.IsEnabled = true;

        }

        public void ShowYesButton()
        {
            DiceButtonAPP.IsEnabled = false;
            DiceButtonAPP.Opacity = 0;

            YesButtonAPP.Opacity = 1;
            YesButtonAPP.IsEnabled = true;
            NoButtonAPP.Opacity = 0;
            NoButtonAPP.IsEnabled = false;

        }

        public void HideYesNoButtonPair()
        {
            YesButtonAPP.Opacity = 0;
            YesButtonAPP.IsEnabled = false;
            NoButtonAPP.Opacity = 0;
            NoButtonAPP.IsEnabled = false;

            DiceButtonAPP.Opacity = 1;
            DiceButtonAPP.IsEnabled = true;
        }

        public void UpdateInfoTextAPP(List<Player> playerList)
        {
            //Update the current turn
            TurnNumberBlockAPP.Text = "Turn :" + Game.Instance.CurrentTurn;


            //Update the information box
            PlayerOneAccount.Text = "Account :" + playerList[0].Account;
            PlayerOneIncome.Text = "Income :" + playerList[0].IncomeEarn;

            PlayerTwoAccount.Text = "Account :" + playerList[1].Account;
            PlayerTwoIncome.Text = "Income :" + playerList[1].IncomeEarn;

            if (playerList.Count > 2)
            {
                PlayerThreeAccount.Text = "Account :" + playerList[2].Account;
                PlayerThreeIncome.Text = "Income :" + playerList[2].IncomeEarn;
            }
            else
            {
                PlayerThreeInfoBox.Opacity = 0;
            }
            if (playerList.Count > 3)
            {
                PlayerFourAccount.Text = "Account :" + playerList[3].Account;
                PlayerFourIncome.Text = "Income :" + playerList[3].IncomeEarn;
            }
            else
            {
                PlayerFourInfoBox.Opacity = 0;
            }
        }

        public void SwitchPlayerInfoBox()
        {
            //Show only the current player turn mark
            PlayerOneInfoBox.Opacity = 0;
            PlayerTwoInfoBox.Opacity = 0;
            PlayerThreeInfoBox.Opacity = 0;
            PlayerFourInfoBox.Opacity = 0;

            switch (Game.Instance.CurrentPlayerTurn)
            {
                case 0:
                    PlayerOneInfoBox.Opacity = 1;
                    break;
                case 1:
                    PlayerTwoInfoBox.Opacity = 1;
                    break;
                case 2:
                    PlayerThreeInfoBox.Opacity = 1;
                    break;
                case 3:
                    PlayerFourInfoBox.Opacity = 1;
                    break;
            }
        }

        public void EnableAPPUIAPP()
        {
            PlayerInfo.Opacity = 1;
            TurnNumberBlockAPP.Opacity = 1;
            DiceButtonAPP.Opacity = 1;
        }

        public void RobTextShow()
        {
            RobText.Opacity = 1;
        }

        public void RobTextHide()
        {
            RobText.Opacity = 0;
        }
    }
}
