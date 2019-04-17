using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/******************************************
 *  V3.2
 * Most likely ur open end challenge is done, just need your like to fill in the method below
 * Now instead of 2, we have 3 situations when arriving the position
 * so I changed the structure of MainGameLogic a bit...
 * This may lead to a complete collapse of the program (plus my brain is empty now)
 * A men
 * 
 * Your task now...
 * 1. complete the method in Game AND ALSO ***GOBALEVENT***~!!!!!
 * 2. Test this shit to make sure no bug on logic
 * 3  Freeze the AppMode button
 * 4. Maybe try to add a stop between every step move 
 * ( It's not easy, may need to cover the MainGameLogic by a dispatchtimer)
 * 
 * And I think we are done! 
 *  1. Backup one first 
 *  2. Try to make the code clean and del not useful comment
 *  3. Also add some comments for more impression marks
 *  4. Report.........
 * 
 * 
/*#############################################################################
 #  Open End Challenge (Mission B):
 #  There are something need you to do in order to make the game playable for human:
 #  This mission can only be done by you as THESE ARE ALL AT THE MAINGAMELOGIC! GOOD LUCK!
 #
 #  1.  Let the dice button to come back after following situation:
 #      After one player stepped on other player's station and pressed "yes"
 #      After upgrade the station and pressed "yes"
 #
 #  2.  If possible, I think we can add something to indicate the coming of salary day
 #
 #  3.  I've added the selling price and basic rent of station in the csv (just last 2 column, selling price, rent), you can modify it if you think the price is too cheap
 #
 #  4. I've add the button to the App Mode Window, but the show and hiddenis still not yet finished as I dont know which part is doing this.
 #
 #  5. 6 new methods add at the last of the Game Class, they are check bankruptcy, check having item to steal property, perform stealing, check highest money, check having rob money item, perform rob money action respectively.
 #  ***TO DO: the 2 check items methods are not yet implement as the field for holding the items are not yet added. These methods can be used if needed, else, just delete them.
 #
 #  Congratulations! The game can now be running smoothly, except the button really cannot fix on the window by using normal way.
 #
 #############################################################################*/

namespace HKMonopoly
{
    class Game
    {
        List<Player> playerList = new List<Player>();
        int numberOfPlayers = 0;
        Random randomDice = new Random();

        private StreamReader railwayNameFile;
        private List<Station> stationList;
        private string[] stationInfo;
        private string stationTemp;
            

        int currentTurn = 0;
        public int CurrentTurn
        {
            get { return currentTurn; }
            set { currentTurn = value; }
        }

        int currentPlayerTurn = 0;
        public int CurrentPlayerTurn
        {
            get
            {
                currentPlayerTurn = CurrentTurn % numberOfPlayers;
                return currentPlayerTurn;
            }
        }

        //read the infomation of the map (station) into a list
        private Game()
        {
            try
            {
                railwayNameFile = new StreamReader("mtrRailwayStationName.csv");
                stationList = new List<Station>();
                while ((stationTemp = railwayNameFile.ReadLine()) != null)
                {
                    stationInfo = stationTemp.Split(',');
                    stationList.Add(new Station(Int32.Parse(stationInfo[0]), stationInfo[1], Int32.Parse(stationInfo[2]), Boolean.Parse(stationInfo[3]), Int32.Parse(stationInfo[4]), Int32.Parse(stationInfo[5]), Int32.Parse(stationInfo[6]), Int32.Parse(stationInfo[7]), Int32.Parse(stationInfo[8])));
                }
            }
            catch (Exception e)
            {
                throw new FileNotFoundException();
            }
            for (int i = 0; i < stationList.Count; i++)
            {
                if (stationList[i].Norikae)
                {
                    stationList[i].AddCoincidenceStation(stationList[stationList[i].CoinciStationNo - 1]);
                }
            }
        }

        private static Game instance;
        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    try
                    {
                        instance = new Game();
                    }
                    catch (FileNotFoundException e)
                    {
                        throw new FileNotFoundException();
                    }
                }
                return instance;
            }
        }



        public void UpdateNumberOfPlayers(int numberOfPlayerFromInput)
        {
            numberOfPlayers = numberOfPlayerFromInput;
            CreatePlayerData(numberOfPlayers);
        }

        private void CreatePlayerData(int numberOfPlayers)
        {

            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerList.Add(new Player(i + 1));
                Console.WriteLine("Player " + i +" Data Created");
            }
        }

        public List<Player> GetPlayersList()
        {
            return playerList;
        }

        public string reportPlayerInformation(int playerNo)
        {
            string s = playerList[playerNo].ReportPlayerStatus();
            return s;
        }

        public int GameRollDice()
        {
            int DiceResult = 0;
            DiceResult = randomDice.Next(1, 7);
            return DiceResult;

        }

        public void ResumeStageOfMainGmaeLogic()
        {
            MainGameLogic.Instnace.NextGameStage(-1);
        }

        //Perform moving forward action
        public int PlayerMoveForward()
        {
            if (playerList[CurrentPlayerTurn].Position == stationList.Count)
            {
                playerList[CurrentPlayerTurn].PlayerDirection = false;
            }
            else if (playerList[CurrentPlayerTurn].Position == 1)
            {
                playerList[CurrentPlayerTurn].PlayerDirection = true;
            }
            else if (stationList[playerList[CurrentPlayerTurn].Position - 1].RailLine != stationList[playerList[CurrentPlayerTurn].Position].RailLine)
            {
                playerList[CurrentPlayerTurn].PlayerDirection = false;
            }
            else if (stationList[playerList[CurrentPlayerTurn].Position - 2].RailLine != stationList[playerList[CurrentPlayerTurn].Position - 1].RailLine)
            {
                playerList[CurrentPlayerTurn].PlayerDirection = true;
            }
            if (playerList[CurrentPlayerTurn].PlayerDirection)
            {
                playerList[CurrentPlayerTurn].Position++;
            }
            else
            {
                playerList[CurrentPlayerTurn].Position--;
            }

            if (stationList[playerList[CurrentPlayerTurn].Position - 1].Norikae)
            {
                return 1;
            }

            return 0;
        }

        //Increase all player's account money
        public void SalaryDay()
        {
            // give salary to everybody~ beware of the number of players(numberOfPlayers)
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].Account += playerList[i].IncomeEarn;
            }
        }

        // return the amount that is going to be charged
        public int PreviewPlayerBeingCharged()
        {
            return stationList[playerList[CurrentPlayerTurn].Position - 1].StationRent;
        }

        //To perform charging money action
        public int PlayerBeingCharged()
        {
            if (stationList[playerList[CurrentPlayerTurn].Position - 1].Owner != null && stationList[playerList[CurrentPlayerTurn].Position - 1].Owner != playerList[CurrentPlayerTurn])
            {
                int rent = stationList[playerList[CurrentPlayerTurn].Position - 1].StationRent;
                playerList[CurrentPlayerTurn].Account -= rent;
                stationList[playerList[CurrentPlayerTurn].Position - 1].Owner.Account += rent;
                return rent;
            }
            return 0;
        }

        //To retrieve the property's selling price
        public int GetPropertyPrice()
        {
            return stationList[playerList[CurrentPlayerTurn].Position - 1].StationSellingPrice;
        }

        //Return the current station name, used for printing to player
        public string GetCurrentStation()
        {
            return stationList[playerList[CurrentPlayerTurn].Position - 1].StationName;
        }

        public int GetSteppedOnPropertyOnwerNo()
        {
            //           return the propertyOwnerNo + 1 (i.e. player 1 = [0]) that i should pay to
            //           for example: "You stepped on player 3's House! Please pay"  
            //           then, i need return 3, which is the Number value + 1
            
            return stationList[playerList[CurrentPlayerTurn].Position - 1].Owner.PlayerNo;
        }

        public int ArrivalSituation()
        {
            //           When player arrive, there are 3 situations
            //           return 1 : Player can buy a house
            //           return 2 : Player need to pay because he stepped on other player land
            //           return 3 : Player already own the land, upgrade
            if (stationList[playerList[CurrentPlayerTurn].Position - 1].Owner == null)
            {
                return 1;
            }
            else if (stationList[playerList[CurrentPlayerTurn].Position - 1].Owner.Equals(playerList[CurrentPlayerTurn]))
            {
                return 3;
            }
            else
            {
                return 2;
            }
            
        }

        //To perform purchasing property action
        public void PlayerBuyProperty(bool playerBuyProerty)
        {
          
            if (playerBuyProerty)
            {
                stationList[playerList[CurrentPlayerTurn].Position - 1].SetOwner(playerList[CurrentPlayerTurn]);
                stationList[playerList[CurrentPlayerTurn].Position - 1].StationRankUp();
                playerList[CurrentPlayerTurn].Account -= stationList[playerList[CurrentPlayerTurn].Position - 1].StationSellingPrice;
                playerList[CurrentPlayerTurn].IncomeEarn += stationList[playerList[CurrentPlayerTurn].Position - 1].StationRent / 4;
            }
        }

        //To perform change line action
        public void PlayerChangeLine(bool playerChangeLine)
        {
            if (playerChangeLine)
            {
                playerList[CurrentPlayerTurn].Position = stationList[playerList[CurrentPlayerTurn].Position - 1].CoinciStationNo;
            }
        }
        //To retrieve the x coordinate of current position
        public int GetPlayerXCoordinate(int playerNO)
        {
            return stationList[playerList[playerNO].Position - 1].XAxis;
        }

        //To retrieve the y coordinate of current position
        public int GetPlayerYCoordinate(int playerNO)
        {
            return stationList[playerList[playerNO].Position - 1].YAxis;
        }

        //To upgrade player's property
        public void PlayerUpgradeProperty(bool playerUpgradeProerty)
        {
            if (playerUpgradeProerty)
            {
                playerList[CurrentPlayerTurn].IncomeEarn -= stationList[playerList[CurrentPlayerTurn].Position - 1].StationRent;
                stationList[playerList[CurrentPlayerTurn].Position - 1].StationRankUp();
                playerList[CurrentPlayerTurn].Account -= stationList[playerList[CurrentPlayerTurn].Position - 1].StationSellingPrice;
                playerList[CurrentPlayerTurn].IncomeEarn += stationList[playerList[CurrentPlayerTurn].Position - 1].StationRent / 4;
            }
        }

        //check whether player is empty in account
        public bool IsBankruptcy()
        {
            if (playerList[CurrentPlayerTurn].Account < 0)
            {
                playerList[CurrentPlayerTurn].IsFailed = true;
            }
            return playerList[CurrentPlayerTurn].IsFailed;
        }

        //check whether player own the item
        public bool HasStealPropertyItem()
        {
            return false;
        }

        //steal property action
        public void StealProperty(bool playerStealProperty)
        {
            if (playerStealProperty)
            {
                stationList[playerList[CurrentPlayerTurn].Position - 1].Owner.IncomeEarn -= stationList[playerList[CurrentPlayerTurn].Position - 1].StationRent;
                stationList[playerList[CurrentPlayerTurn].Position - 1].SetOwner(playerList[CurrentPlayerTurn]);
            }
        }


        //To Check which player have the most money
        public int HaveMostMoney()
        {
            int mostMoney = 0;
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].Account > mostMoney)
                {
                    mostMoney = playerList[i].Account;
                }
            }
            return playerList.FindIndex(p => p.Account == mostMoney);
        }


        public bool CurrentPlayerHasRobMoneyItem()
        {
            // CHECK CURRENT TURN PLAYER! return true if have
            if (playerList[CurrentPlayerTurn].RobMoneyItem)
                return true;
            return false;
        }

        //To rob all player's money
        public int RobMoney()
        {
            //                  Rob the current position building owner $5000, give it to the current player
            //                  just a reverse of payment to owner
            //                  The return type is int, give me the playerNO being robbed
            //                  player 1 is 1, dont give me 0  :)

            stationList[playerList[CurrentPlayerTurn].Position - 1].Owner.Account -= 5000;
            playerList[CurrentPlayerTurn].Account += 5000;
            playerList[CurrentPlayerTurn].RobMoneyItem = false;
            return CurrentPlayerTurn + 1;
        }

        public int IsOnlyOnePlayerLeft()
        {
            //           loop and see if there is only 1 player who have positive account.
            //           If no, still have other players, return -1;
            //           If yes, return the playerNo, I mean player 1 = 1, player 2 = 2...
            int count = 0;
            int onlyPlayerNo = -1;
            for (int i = 0; i < playerList.Count; i++)
            {
                if (!playerList[i].IsFailed)
                {
                    count++;
                    onlyPlayerNo = i + 1;
                }
            }
            if (count > 1)
            {
                return -1;
            }
            return onlyPlayerNo;
        }

    }
}
