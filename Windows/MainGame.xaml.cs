using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


/************************************************
 * This class is responsible for the view updating in the main game part
 * 
 * 
 * For the both GUI window, (App mode and Main), 
 * they are all linked to this object for the main game part
 * 
 * This class is used to provide common updating view logic for app mode and main window.
 * 
 ***********************************************/

namespace HKMonopoly
{
    /// <summary>
    /// Interaction logic for MainGame.xaml
    /// </summary>
    public partial class MainGame : Page
    {
        public MainGame MainGameInstance;
        Image PlayerOneMarkImage = new Image();
        Image PlayerTwoMarkImage = new Image();
        Image PlayerThreeMarkImage = new Image();
        Image PlayerFourMarkImage = new Image();

        public MainGame()
        {
            InitializeComponent();
            MainGameInstance = this;

            PlayerOneMarkImage.Height = 60;
            PlayerOneMarkImage.Width = 60;
            PlayerOneMarkImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerOneMark.png", UriKind.Relative));

            PlayerTwoMarkImage.Height = 60;
            PlayerTwoMarkImage.Width = 60;
            PlayerTwoMarkImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerTwoMark.png", UriKind.Relative));

            PlayerThreeMarkImage.Height = 60;
            PlayerThreeMarkImage.Width = 60;
            PlayerThreeMarkImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerThreeMark.png", UriKind.Relative));

            PlayerFourMarkImage.Height = 60;
            PlayerFourMarkImage.Width = 60;
            PlayerFourMarkImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerFourMark.png", UriKind.Relative));

        }

        private void DiceButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("CLICKED!");
            MainGameLogic.Instnace.NextGameStage(-1);
        }

        public void MakeAnnoucement(string s)
        {
            GUIAnnouncement.Opacity = 1;
            GUIAnnouncement.Text = s;
        }

        public void ClearAnnoucement()
        {
            GUIAnnouncement.Opacity = 0;
        }

        public void UpdateDiceResult(int result)
        {
            DiceResultBlock.Text = result+"";
        }

        public void UpdateMainGameView(List<Player> playerList)
        {
            //Update the current turn
            TurnNumberBlock.Text = "Turn :" + Game.Instance.CurrentTurn;

            //Stamp a fail if player lose

            
            //Update the information box
            PlayerOneAccount.Text = "Account :" + playerList[0].Account;
            PlayerOneIncome.Text = "Income :" + playerList[0].IncomeEarn;
            if (playerList[0].Account < 0)
            {
                PlayerOneFail.Opacity = 1;
            }

            PlayerTwoAccount.Text = "Account :" + playerList[1].Account;
            PlayerTwoIncome.Text = "Income :" + playerList[1].IncomeEarn;
            if (playerList[1].Account < 0)
            {
                PlayerTwoFail.Opacity = 1;
            }
            


            if (playerList.Count > 2)
            {
                PlayerThreeAccount.Text = "Account :" + playerList[2].Account;
                PlayerThreeIncome.Text = "Income :" + playerList[2].IncomeEarn;
                if (playerList[2].Account < 0)
                {
                    PlayerThreeFail.Opacity = 1;
                }
            }
            else
            {
                PlayerThreeInfoBox.Opacity = 0;
            }
            if (playerList.Count > 3)
            {
                PlayerFourAccount.Text = "Account :" + playerList[3].Account;
                PlayerFourIncome.Text = "Income :" + playerList[3].IncomeEarn;
                if (playerList[4].Account < 0)
                {
                    PlayerFourFail.Opacity = 1;
                }
            }
            else
            {
                PlayerFourInfoBox.Opacity = 0;
            }

            //Show only the current player turn mark
            PlayerOneMark.Opacity = 0;
            PlayerTwoMark.Opacity = 0;
            PlayerThreeMark.Opacity = 0;
            PlayerFourMark.Opacity = 0;

            if (!(Game.Instance.IsBankruptcy()))
            {
                switch (Game.Instance.CurrentPlayerTurn)
                {
                    case 0:
                        PlayerOneMark.Opacity = 1;
                        break;
                    case 1:
                        PlayerTwoMark.Opacity = 1;
                        break;
                    case 2:
                        PlayerThreeMark.Opacity = 1;
                        break;
                    case 3:
                        PlayerFourMark.Opacity = 1;
                        break;
                }
            }


            //Update the position of player's mark
            MarkCanvas.Children.Clear();

            //Relocate Player One Mark
            MarkCanvas.Children.Add(PlayerOneMarkImage);
            Canvas.SetLeft(PlayerOneMarkImage, Game.Instance.GetPlayerXCoordinate(0) - (PlayerOneMarkImage.Width / 2));
            Canvas.SetTop(PlayerOneMarkImage, Game.Instance.GetPlayerYCoordinate(0) - PlayerOneMarkImage.Height);

            //Relocate Player Two Mark
            MarkCanvas.Children.Add(PlayerTwoMarkImage);
            Canvas.SetLeft(PlayerTwoMarkImage, Game.Instance.GetPlayerXCoordinate(1) - (PlayerTwoMarkImage.Width / 2));
            Canvas.SetTop(PlayerTwoMarkImage, Game.Instance.GetPlayerYCoordinate(1) - PlayerTwoMarkImage.Height);
            if (playerList.Count > 2)
            {
                //Relocate Player Three Mark
                MarkCanvas.Children.Add(PlayerThreeMarkImage);
                Canvas.SetLeft(PlayerThreeMarkImage, Game.Instance.GetPlayerXCoordinate(2) - (PlayerThreeMarkImage.Width / 2));
                Canvas.SetTop(PlayerThreeMarkImage, Game.Instance.GetPlayerYCoordinate(2) - PlayerThreeMarkImage.Height);
            }
            if (playerList.Count > 3)
            {
                //Relocate Player Four Mark
                MarkCanvas.Children.Add(PlayerFourMarkImage);
                Canvas.SetLeft(PlayerFourMarkImage, Game.Instance.GetPlayerXCoordinate(3) - (PlayerFourMarkImage.Width / 2));
                Canvas.SetTop(PlayerFourMarkImage, Game.Instance.GetPlayerYCoordinate(3) - PlayerFourMarkImage.Height);
            }
        }
        
        //Add a building icon when player buy the building
        public void AddPropertyIconIntoBuildingCanvas(int boughtPlayerNo)
        {
            Image buildingImage = new Image();
            buildingImage.Height = 30;
            buildingImage.Width = 30;
            switch (boughtPlayerNo)
            {
                case 0:
                    buildingImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerOneBuilding.png", UriKind.Relative));
                    break;
                case 1:
                    buildingImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerTwoBuilding.png", UriKind.Relative));
                    break;
                case 2:
                    buildingImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerThreeBuilding.png", UriKind.Relative));
                    break;
                case 3:
                    buildingImage.Source = new BitmapImage(new Uri("/HKMonopoly;component/Assets/PlayerFourBuilding.png", UriKind.Relative));
                    break;
            }

            BuildingCanvas.Children.Add(buildingImage);
            Canvas.SetLeft(buildingImage, Game.Instance.GetPlayerXCoordinate(boughtPlayerNo) - (buildingImage.Width / 2));
            Canvas.SetTop(buildingImage, Game.Instance.GetPlayerYCoordinate(boughtPlayerNo) - buildingImage.Height);

        }

        public void ShowYesNoButtonPair()
        {
            DiceButton.IsEnabled = false;

            YesButton.Opacity = 1;
            YesButton.IsEnabled = true;
            NoButton.Opacity = 1;
            NoButton.IsEnabled = true;
            
        }

        public void ShowYesButton()
        {
            DiceButton.IsEnabled = false;

            YesButton.Opacity = 1;
            YesButton.IsEnabled = true;
            NoButton.Opacity = 0;
            NoButton.IsEnabled = false;

        }

        public void HideYesNoButtonPair()
        {
            YesButton.Opacity = 0;
            YesButton.IsEnabled = false;
            NoButton.Opacity = 0;
            NoButton.IsEnabled = false;

            DiceButton.IsEnabled = true;
        }


        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("YES CLICKED!");
            MainGameLogic.Instnace.NextGameStage(1);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("NO CLICKED!");
            MainGameLogic.Instnace.NextGameStage(2);
        }

        public void HideAllUIMainGameAPP()
        {
            PlayerInfo.Opacity = 0;
            PlayerInfo.IsEnabled = false;

            DiceButton.Opacity = 0;
            DiceButton.IsEnabled = false;
            DiceButton.IsHitTestVisible = true;
            UnderstandButton.Opacity = 0;
            UnderstandButton.IsEnabled = false;
            UnderstandButton.IsHitTestVisible = true;

            DiceResultBlock.Opacity = 0;
            DiceResultBlock.IsEnabled = false;

            TurnNumberBlock.Opacity = 0;
            TurnNumberBlock.IsEnabled = false;

            PlayerOneMark.Opacity = 0;
            PlayerOneMark.IsEnabled = false;
            PlayerTwoMark.Opacity = 0;
            PlayerTwoMark.IsEnabled = false;
            PlayerThreeMark.Opacity = 0;
            PlayerThreeMark.IsEnabled = false;
            PlayerFourMark.Opacity = 0;
            PlayerFourMark.IsEnabled = false;
        }

        public void ShowDice()
        {
            DiceButton.Opacity = 1;
            DiceButton.IsEnabled = true;
            DiceButton.IsHitTestVisible = true;
            BlurBackground.Opacity = 1;
            DiceResult.Opacity = 0;
            TurnNumberBlock.Opacity = 0;
            DiceResultBlock.Opacity = 0;
            BuildingCanvas.Opacity = 0;
            MarkCanvas.Opacity = 0;

            YesButton.Opacity = 0;
            YesButton.IsEnabled = false;
            NoButton.Opacity = 0;
            NoButton.IsEnabled = false;

        }

        public void HideDice()
        {
            DiceButton.Opacity = 0;
            DiceButton.IsEnabled = false;
            DiceButton.IsHitTestVisible = false;
            BlurBackground.Opacity = 0;
            DiceResult.Opacity = 1;
            TurnNumberBlock.Opacity = 1;
            DiceResultBlock.Opacity = 1;
            BuildingCanvas.Opacity = 1;
            MarkCanvas.Opacity = 1;
        }


        public void ShowUnderstandButton()
        {
            
            UnderstandButton.Opacity = 1;
            UnderstandButton.IsEnabled = true;
            UnderstandButton.IsHitTestVisible = true;
            BlurBackground.Opacity = 1;
            DiceResult.Opacity = 0;
            TurnNumberBlock.Opacity = 0;
            DiceResultBlock.Opacity = 0;
            BuildingCanvas.Opacity = 0;
            MarkCanvas.Opacity = 0;
        }
        public void HideUnderstandButton()
        {
            UnderstandButton.Opacity = 0;
            UnderstandButton.IsEnabled = false;
            UnderstandButton.IsHitTestVisible = false;
            BlurBackground.Opacity = 0;
            DiceResult.Opacity = 1;
            TurnNumberBlock.Opacity = 1;
            DiceResultBlock.Opacity = 1;
            BuildingCanvas.Opacity = 1;
            MarkCanvas.Opacity = 1;
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
