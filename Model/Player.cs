using System;
using System.Collections.Generic;

/************************************************
 * This class is created for storing the player information.
 * It contains number, account, income information and some other specific method
 * 
 * As the player information can be change over time, 
 * properties are provide for getting and setting some of the information of player
 * 
 * A constructor has been created for each time creating new class
 * It can be used to initialize the player for the game.
 * 
 * 
 ***********************************************/

namespace HKMonopoly
{
    public class Player
    {
        public Player(int pPlayerNumber)
        {
            playerNo = pPlayerNumber;
            account = 10000;
            incomeEarn = 2000;
            position = 1;
        }

        private int playerNo;
        public int PlayerNo
        {
            get { return playerNo; }
        }

        private int account;
        public int Account
        {
            get { return account; }
            set { account = value; }
        }

        private int incomeEarn;
        public int IncomeEarn
        {
            get { return incomeEarn; }
            set
            {
                incomeEarn = value;
            }
        }

        private List<int> ownPropertyList = new List<int>();

        private int position;
        public int Position
        {
            get { return position; }
            set
            {
                if (Position > 0)
                {
                    position = value;
                }
            }
        }

        private int status = 1;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private bool robMoneyItem = true;
        public bool RobMoneyItem
        {
            get { return robMoneyItem; }
            set { robMoneyItem = value; }
        }

        private bool isFailed = false;
        public bool IsFailed
        {
            get { return isFailed; }
            set { isFailed = value; }
        }

        private Boolean playerDirection = true; // true is +
        public Boolean PlayerDirection
        {
            get { return playerDirection; }
            set { playerDirection = value; }
        }

        // 3 basic property control methods

        public void AddProperty(int propertyPosition, int rentEarn)
        {
            ownPropertyList.Add(propertyPosition);
            this.IncomeEarn += rentEarn;
        }

        public void RemoveProperty(int propertyPosition, int rentEarn)
        {
            ownPropertyList.Remove(propertyPosition);
            this.IncomeEarn -= rentEarn;
        }

        public List<int> CheckPlayerPropery()
        {
            return ownPropertyList;
        }

        public string ReportPlayerStatus()
        {
            string reportString ="";

            reportString += "------------------\n";
            reportString += "Player " + this.PlayerNo + ": \n";
            reportString += "--Account :" + this.Account + "\n";
            reportString += "--Position : " + this.Position + "\n";
            reportString += "--Income : " + this.IncomeEarn + "\n";
            reportString += "Property own : " + this.ownPropertyList.Count + "\n";
            reportString += "------------------\n";

            return reportString;
        }

        public void ReportPlayerStatusInSalaryDay(int salary, int updatedAccount)
        {

        }

        public void SalaryRecieve()
        {
            ReportPlayerStatusInSalaryDay(IncomeEarn, Account + IncomeEarn);
            Account += IncomeEarn;
        }

    }
}
