using System;
using System.Collections.Generic;
using System.Windows;

/************************************************
 * This class is the main logic of the whoel game.
 * It acted as the presentor between model and view and response for all logic in-/out-put
 * 
 * It this class, we implement the consept of finite state machine which used to switch through different state of the game
 * It will response to either model or view accordingly depended on the state of present can contral all views' update
 * 
 * The method "NextGameState()" is used to control the state of the game and to des=cide the action to have through out the whole game
 * It also responsible for the syncronization of all the windows in this game (App mode, Text mode, Main Window)
 * 
 * It prevented View to update the model directly and also vice versa.
 * 
 * Singleton has been used to prevent more than one Logic class has been created
 * As only one logic is needed and allowed to have for one game.
 * 
 ***********************************************/

namespace HKMonopoly
{
    class MainGameLogic
    {
        List<Player> playerList;
        int numberOfPlayers = 0;
        int playerStepsRemaining = 0;



        private int currentState = -2;
        public int CurrentState
        {
            get { return currentState; }
        }

        private MainGameLogic()
        {

        }

        private static MainGameLogic _instance = null;
        public static MainGameLogic Instnace
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainGameLogic();
                }
                return _instance;
            }
        }

        public void NextGameStage(int input)
        {
            //End Game Condition
            try
            {
                int gameWinner = Game.Instance.IsOnlyOnePlayerLeft();                        
                if (gameWinner != -1)
                {
                MessageBox.Show("Game Over! Player " + gameWinner + " has won!");
                Application.Current.Shutdown();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("File \"mtrRailwayStationName.csv\" Not Found. The game will be terminated. " + e);
                Application.Current.Shutdown();
            }
            //Turn flow
            switch (currentState)
            {
                case -2:
                    Console.WriteLine("Stage : " + currentState);
                    //"Press any button to start" "Prss Enter to start (TEXT)" Stage
                    MainWindow.startMenu.StartMenuInstance.ToStateNegativeOne();
                    AppMode.startMenu.StartMenuInstance.ToStateNegativeOne();
                    TextMode.TextModeInstance.UpdateText("Number of players : ");
                    currentState++;
                    break;
                case -1:

                    //Choose Number of player Stage
                    Console.WriteLine("Stage : " + currentState);
                    if (input > 1 && input < 5)
                    {
                        
                            Game.Instance.UpdateNumberOfPlayers(input);
                        

                        numberOfPlayers = input;
                        Console.WriteLine("[Input] Number of Player is :" + input);
                        TextMode.TextModeInstance.UpdateText(input + " Players");
                        MainWindow.startMenu.StartMenuInstance.NavigateToMainGame(MainWindow.mainGame);
                        AppMode.startMenu.StartMenuInstance.NavigateToMainGame(AppMode.mainGame);

                        //[GUI] Set up the GUI screen / [TEXT] Print player status
                        playerList = Game.Instance.GetPlayersList();
                        AppMode.mainGame.MainGameInstance.UpdateMainGameView(playerList);
                        AppMode.mainGame.MainGameInstance.HideAllUIMainGameAPP();
                        AppMode.AppModeInstance.EnableAPPUIAPP();
                        AppMode.AppModeInstance.UpdateInfoTextAPP(playerList);
                        MainWindow.mainGame.MainGameInstance.UpdateMainGameView(playerList);
                        
                        for (int i = 0; i < numberOfPlayers; i++)
                        {
                            string s = Game.Instance.reportPlayerInformation(i);
                            
                            TextMode.TextModeInstance.UpdateText(s);
                            TextMode.TextModeInstance.ClearText();
                        }

                        currentState++;
                        NextGameStage(-1);
                    } else
                    {
                        TextMode.TextModeInstance.UpdateText("[Wrong Input] Please enter 2, 3, or 4 players");
                    }
                    break;

                case 0:
                    //Print player turn and check Global Event
                    int currentPlayerNo = Game.Instance.CurrentPlayerTurn + 1;
                    MainWindow.mainGame.MainGameInstance.HideUnderstandButton();
                    AppMode.AppModeInstance.HideYesNoButtonPair();

                    if (Game.Instance.IsBankruptcy())
                    {
                        Game.Instance.CurrentTurn++;
                        currentState = 0;
                        NextGameStage(-1);
                        break;
                    }
                    
                    TextMode.TextModeInstance.UpdateText("-------Player " + currentPlayerNo + " Turn--------");
                    MainWindow.mainGame.MainGameInstance.MakeAnnoucement("-------Player " + currentPlayerNo + " Turn--------");
                    AppMode.AppModeInstance.MakeAnnoucement("-------Player " + currentPlayerNo + " Turn--------");
                    AppMode.AppModeInstance.SwitchPlayerInfoBox();

                    int GobalEventNumber = GlobalEvent.Instance.TrigglerGobalEvent();

                    if(GobalEventNumber != 0)
                    {
                        playerList = Game.Instance.GetPlayersList();
                        string s = GlobalEvent.Instance.SelectGobalEvent(playerList, GobalEventNumber);

                        TextMode.TextModeInstance.UpdateText(s);
                        MainWindow.mainGame.MainGameInstance.MakeAnnoucement(s);
                        AppMode.AppModeInstance.MakeAnnoucement(s);
                    }

                    if (GlobalEvent.Instance.IsTurnPassed(GobalEventNumber))
                    {
                        MainWindow.mainGame.MainGameInstance.HideDice();
                        MainWindow.mainGame.MainGameInstance.ShowUnderstandButton();
                        AppMode.AppModeInstance.ShowYesButton();
                        Game.Instance.CurrentTurn++;
                        currentState = 0;
                        break;
                    }
                    else
                    {
                        TextMode.TextModeInstance.UpdateText("[Input] Type 1 to roll a dice");
                        currentState++;
                    }
                    MainWindow.mainGame.MainGameInstance.ShowDice();
                    break;
                case 1:
                    //Player Input a dice
                    MainWindow.mainGame.MainGameInstance.HideDice();
                    int diceResult = Game.Instance.GameRollDice();
                    playerStepsRemaining = diceResult;          
                    TextMode.TextModeInstance.UpdateText("Roll Dice : " + diceResult);
                    MainWindow.mainGame.MainGameInstance.UpdateDiceResult(diceResult);
                    AppMode.AppModeInstance.UpdateDiceResult(diceResult);

                    currentState++; 
                    NextGameStage(-1);

                    break;
                case 2:
                    //Player moving
                    //deal with move with a while loop

                    while (playerStepsRemaining > 0)
                    {
                        int playerMoveResult = Game.Instance.PlayerMoveForward();
                        playerStepsRemaining--;
                        playerList = Game.Instance.GetPlayersList();
                        //Update the view
                        MainWindow.mainGame.MainGameInstance.UpdateMainGameView(playerList);
                        AppMode.mainGame.MainGameInstance.UpdateMainGameView(playerList);
                        AppMode.AppModeInstance.UpdateInfoTextAPP(playerList);

                        if (playerMoveResult == 1) // Ask for change station
                        {
                            TextMode.TextModeInstance.UpdateText("You can change the railway line at " + Game.Instance.GetCurrentStation() + "Station");
                            TextMode.TextModeInstance.UpdateText("[Input] [1] change");
                            TextMode.TextModeInstance.UpdateText("[Input] [2] no change");
         
                            MainWindow.mainGame.MainGameInstance.MakeAnnoucement("You can change at " +  Game.Instance.GetCurrentStation());
                            AppMode.AppModeInstance.MakeAnnoucement("You can change at " + Game.Instance.GetCurrentStation());

                            MainWindow.mainGame.MainGameInstance.ShowYesNoButtonPair();
                            AppMode.AppModeInstance.ShowYesNoButtonPair();
                            
                            currentState++;
                            break;
                        }

                    //Last step of player ends, go to next stage
                    }
                    if (playerStepsRemaining <= 0)
                    {
                        currentState = 4;
                        NextGameStage(-1);
                    }


                    break;
                case 3:
                    //Change Line
                    MainWindow.mainGame.MainGameInstance.HideYesNoButtonPair();
                    AppMode.AppModeInstance.HideYesNoButtonPair();

                    if (input == 1) // change line
                    {
                        Game.Instance.PlayerChangeLine(true);
                    }
                    else if(input == 2) // change line
                    {
                        Game.Instance.PlayerChangeLine(false);
                    }
                    else
                    {
                       TextMode.TextModeInstance.UpdateText("[Wrong Input] Please input 1 or 2");
                    }
                    currentState = 2;
                    NextGameStage(-1);

                    break;
                case 4:
                    //arrive
                    // 1. Other people land : -$
                    // 2. Buy Building
                    // 3. Upgrade Building
                    int arrivalSituation = Game.Instance.ArrivalSituation();
                    int playerBeingCharged = Game.Instance.PreviewPlayerBeingCharged();
                    if (arrivalSituation == 1)
                    {
                        // buy house
                        TextMode.TextModeInstance.UpdateText("You can buy the house here, price : " + Game.Instance.GetPropertyPrice());
                        TextMode.TextModeInstance.UpdateText("[Input] [1] buy");
                        TextMode.TextModeInstance.UpdateText("[Input] [2] no buy");
                        
                        MainWindow.mainGame.MainGameInstance.MakeAnnoucement("Buy House? Price: $" + Game.Instance.GetPropertyPrice());
                        AppMode.AppModeInstance.MakeAnnoucement("Buy House? Price: $" + Game.Instance.GetPropertyPrice());

                        MainWindow.mainGame.MainGameInstance.ShowYesNoButtonPair();
                        AppMode.AppModeInstance.ShowYesNoButtonPair();
                        currentState = 5;
                        break;
                    }
                    else if (arrivalSituation == 2)
                    {
                        // Comfirm to pay other player's rent
                        TextMode.TextModeInstance.UpdateText("You stepped on player " + Game.Instance.GetSteppedOnPropertyOnwerNo() + "'s House! Pay $" + playerBeingCharged);
                        TextMode.TextModeInstance.UpdateText("[Input] [1] Pay");

                        MainWindow.mainGame.MainGameInstance.MakeAnnoucement("You stepped on player " + Game.Instance.GetSteppedOnPropertyOnwerNo() + "'s House! Pay $" + playerBeingCharged);
                        AppMode.AppModeInstance.MakeAnnoucement("You stepped on player " + Game.Instance.GetSteppedOnPropertyOnwerNo() + "'s House! Pay $" + playerBeingCharged);

                        if (Game.Instance.CurrentPlayerHasRobMoneyItem())
                        {
                            //Have Item, one more choice
                            TextMode.TextModeInstance.UpdateText("[Input] [2] Rob!");
                            MainWindow.mainGame.MainGameInstance.ShowYesNoButtonPair();
                            AppMode.AppModeInstance.ShowYesNoButtonPair();

                            MainWindow.mainGame.MainGameInstance.RobTextShow();
                            AppMode.AppModeInstance.RobTextShow();
                        }
                        else
                        {
                            //No extra button, can only pay to owner
                            MainWindow.mainGame.MainGameInstance.ShowYesButton();
                            AppMode.AppModeInstance.ShowYesButton();
                        }
                        currentState = 7;
                        break;
                    }else if (arrivalSituation == 3)
                    {
                        // Upgrade
                        TextMode.TextModeInstance.UpdateText("You can upgrade the house here, income increase : " + Game.Instance.GetPropertyPrice());
                        TextMode.TextModeInstance.UpdateText("[Input] [1] Upgrade");
                        TextMode.TextModeInstance.UpdateText("[Input] [2] Not Upgrade");

                        MainWindow.mainGame.MainGameInstance.MakeAnnoucement("Upgrade your house? Double income! Cost: $" + Game.Instance.GetPropertyPrice());
                        AppMode.AppModeInstance.MakeAnnoucement("Upgrade your house? Double income! Cost: $" + Game.Instance.GetPropertyPrice());


                        MainWindow.mainGame.MainGameInstance.ShowYesNoButtonPair();
                        AppMode.AppModeInstance.ShowYesNoButtonPair();

                        currentState = 5;
                        break;
                    }
                    currentState = 8;
                    break;

                case 5:
                    //Player input : 1 buy, 2 not buy

                    MainWindow.mainGame.MainGameInstance.HideYesNoButtonPair();
                    AppMode.AppModeInstance.HideYesNoButtonPair();
                    if (input == 1) // buy
                    {
                        Game.Instance.PlayerBuyProperty(true);
                        TextMode.TextModeInstance.UpdateText("Property purchased. Income +" + Game.Instance.PreviewPlayerBeingCharged());
         
                        MainWindow.mainGame.MainGameInstance.AddPropertyIconIntoBuildingCanvas(Game.Instance.CurrentPlayerTurn);
                        AppMode.mainGame.MainGameInstance.AddPropertyIconIntoBuildingCanvas(Game.Instance.CurrentPlayerTurn);
                    }
                    else if (input == 2) // Not buy
                    {
                        Game.Instance.PlayerBuyProperty(false);
                    }
                    else
                    {
                        TextMode.TextModeInstance.UpdateText("[Wrong Input] Please input 1 or 2");
                    }

                    currentState = 8;
                    NextGameStage(-1);
                    break;
                case 6:
                    //Player input : 1 Upgrade, 2 not upgrade

                    MainWindow.mainGame.MainGameInstance.HideYesNoButtonPair();
                    AppMode.AppModeInstance.HideYesNoButtonPair();

                    if (input == 1) // buy
                    {
                        Game.Instance.PlayerUpgradeProperty(true);
                        TextMode.TextModeInstance.UpdateText("Property Upgraded. Income +" + Game.Instance.PreviewPlayerBeingCharged());
                        
                    }
                    else if (input == 2) // Not buy
                    {
                        Game.Instance.PlayerUpgradeProperty(false);
                    }
                    else
                    {
                        TextMode.TextModeInstance.UpdateText("[Wrong Input] Please input 1 or 2");
                    }

                    currentState = 8;
                    NextGameStage(-1);
                    break;
                case 7:
                    // Comfirm the payment
                    if (input == 1)
                    {
                        Game.Instance.PlayerBeingCharged();
                        TextMode.TextModeInstance.UpdateText("Payment completed. You have paid $" + Game.Instance.PreviewPlayerBeingCharged() + "to player " + Game.Instance.GetSteppedOnPropertyOnwerNo());
                    }else if(input ==2 && Game.Instance.CurrentPlayerHasRobMoneyItem())
                    {
                        int robbedPlayerNO = Game.Instance.RobMoney();
                        TextMode.TextModeInstance.UpdateText("You robbed player" + robbedPlayerNO + "$5000!");
                    }
                    else
                    {
                        TextMode.TextModeInstance.UpdateText("[Wrong Input] Please input 1");
                    }
                    MainWindow.mainGame.MainGameInstance.RobTextHide();
                    AppMode.AppModeInstance.RobTextHide();
                    currentState = 8;
                    NextGameStage(-1);
                    break;
                case 8:
                    //End player turn and new turn start
                    
                    currentState = 0;
                    Game.Instance.CurrentTurn++;

                    playerList = Game.Instance.GetPlayersList();
                    Console.WriteLine("Turn " + Game.Instance.CurrentTurn + " End");

                    MainWindow.mainGame.MainGameInstance.UpdateMainGameView(playerList);
                    AppMode.mainGame.MainGameInstance.UpdateMainGameView(playerList);
                    AppMode.AppModeInstance.UpdateInfoTextAPP(playerList);

                    if (Game.Instance.CurrentTurn % (numberOfPlayers * 5) == 0)
                    {
                        TextMode.TextModeInstance.UpdateText("Salary Day! Everyone's account has added their income amount of money!");
                        MainWindow.mainGame.MainGameInstance.MakeAnnoucement("Salary Day!");
                        AppMode.AppModeInstance.MakeAnnoucement("Salary Day!");

                        MainWindow.mainGame.MainGameInstance.HideDice();
                        MainWindow.mainGame.MainGameInstance.ShowUnderstandButton();
                        AppMode.AppModeInstance.ShowYesButton();

                        currentState = 0;
                        Game.Instance.SalaryDay();
                        break;
                    }

                    NextGameStage(-1);

                    break;
            }


            

        }
    }
}
