using System;
using System.Collections.Generic;

/************************************************
 * This class is created for storing the Global Event
 * It contains probability and event metho
 * 
 * As there should be only one global event object through out the whole game,
 * Singleton has been used to ensure only one global event object is created
 * 
 * A Instance has been used as the medium to call the method of global event
 * The probabilties of different event will increase when turn increase.
 * 
 ***********************************************/

namespace HKMonopoly
{
    class GlobalEvent
    {
        static double[] eventProbability = new double[4];
        static int eventCoolDown = 0;
        double randomResult;
        Random rnd = new Random();


        private GlobalEvent()
        {
        }

        private static GlobalEvent instance;
        public static GlobalEvent Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalEvent();
                }
                return instance;
            }
        }



        public int TrigglerGobalEvent()
        {
            if (eventCoolDown == 0)
            {
                eventProbability[0] += 0.025;
                eventProbability[1] += 0.038;
                eventProbability[2] += 0.043;
                eventProbability[3] += 0.044;

                randomResult = rnd.NextDouble();
                Console.WriteLine(randomResult);

                // event 1 :MTR
                if (randomResult > 0 && randomResult <= eventProbability[0])
                {
                    for (int i = 0; i < 4; i++)
                    {
                        eventProbability[i] -= eventProbability[0];
                    }
                    eventCoolDown += 4;
                    Console.WriteLine("GobalEvent 1");
                    return 1;
                } // event 2: Typhoon
                else if (randomResult > eventProbability[0] && randomResult <= eventProbability[1])
                {
                    eventProbability[1] = 0;
                    for (int i = 1; i < 4; i++)
                    {
                        eventProbability[i] -= eventProbability[1];
                    }
                    eventCoolDown += 4;
                    Console.WriteLine("GobalEvent 2");
                    return 2;
                }//event 3 Being fired
                else if (randomResult > eventProbability[1] && randomResult <= eventProbability[2])
                {
                    for (int i = 2; i < 4; i++)
                    {
                        eventProbability[i] -= eventProbability[2];
                    }
                    eventCoolDown += 4;
                    Console.WriteLine("GobalEvent 3");
                    return 3;
                }//event 4 KIDNAPPING
                else if (randomResult > eventProbability[2] && randomResult <= eventProbability[3])
                {
                    for (int i = 3; i < 4; i++)
                    {
                        eventProbability[i] -= eventProbability[3];
                    }
                    eventCoolDown += 4;
                    Console.WriteLine("GobalEvent 4");
                    return 4;
                }
            }
            else
            {
                eventCoolDown--;
            }

            return 0;
        }

        public string SelectGobalEvent(List<Player> playerList, int eventNumber)
        {
            string s="";
            switch (eventNumber)
            {
                case 1:
                    // event 1 :MTR
                    // [Joe V2.6] : the current turn player account -$500
                    s = "MTR stopped again, but you still need to work, paid $1000 for taxi.";
                    playerList[Game.Instance.CurrentPlayerTurn].Account -= 100;
                    break;
                case 2:
                    // event 2: Typhoon
                    // [Joe V2.6] : No need to do anything, del this comment
                    s = "Typhoon,stay at home! You turn is passed.";
                    Game.Instance.CurrentTurn++;
                    break;
                case 3:
                    //event 3 Being fired
                    // [Joe V2.6] : the current turn player income -$700
                    s = "You are fired, found another low pay job. Income -$1500";
                    playerList[Game.Instance.CurrentPlayerTurn].IncomeEarn -= 1500;
                    break;
                case 4:
                    //event 4 KIDNAPPING
                    // [Joe V2.6] : All other players EXCEPT the current player account -$2500
                    s = "Being kidnapped, others paid $2500 ransom for you, thinks them.";
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        if (!(i == Game.Instance.CurrentPlayerTurn))
                        {
                            playerList[i].Account -= 2500;
                        }
                    }
                    break;
            }
            return s;
        }

        public bool IsTurnPassed(int eventNumber)
        {
            bool output = false;
            switch (eventNumber)
            {
                case 0:
                    //No event
                    output = false;
                    break;
                case 1:
                    // event 1 :MTR
                    output = false;
                    break;
                case 2:
                    // event 2: Typhoon
                    output = true;
                    break;
                case 3:
                    //event 3 Being fired
                    output = false;
                    break;
                case 4:
                    //event 4 KIDNAPPING
                    output = true;
                    break;
            }
            return output;
        }
    }
}
