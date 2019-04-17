using System;

/************************************************
 * This class is created for storing the station information.
 * It contains Name, number, railline information and some other specific method
 * 
 * As the station information should be fixed through out the whole game,
 * only getting information is allowed but not changing the value in most of the field
 * 
 * A constructor has been created for each time creating new class
 * It requied user to input all required information of station while creating new station object
 * 
 * 
 ***********************************************/

namespace HKMonopoly
{
    class Station
    {
        public Station(int inputStationNo, string inputStationName, int inputRailLine, bool inputNorikae, int inputCoincStation, int x, int y, int sellingPrice, int basicRent)
        {
            stationNo = inputStationNo;
            stationName = inputStationName;
            railLine = inputRailLine;
            norikae = inputNorikae;
            coinciStationNo = inputCoincStation;
            xAxis = x;
            yAxis = y;
            this.stationSellingPrice = sellingPrice;
            this.stationBasicRent = basicRent;
        }

        private int stationNo;
        public int StationNo
        {
            get { return stationNo; }
        }
        private string stationName;
        public string StationName
        {
            get { return stationName; }
        }
        private int railLine;
        public int RailLine
        {
            get { return railLine; }
        }
        private bool norikae = false;                           //change line
        public bool Norikae
        {
            get { return norikae; }
        }

        private int coinciStationNo;
        public int CoinciStationNo
        {
            get { return coinciStationNo; }
        }

        private Station coincidenceStation = null;              //The station that can change line
        public Station CoincidenceStation
        {
            get { return coincidenceStation; }
        }

        //To add Coincidence station while reading the file
        public void AddCoincidenceStation(Station coincidenceStation)
        {
            this.coincidenceStation = coincidenceStation;
        }

        private Player owner;
        public Player Owner
        {
            get { return owner; }
            
        }

        public void SetOwner(Player ownerName)
        {
            this.owner = ownerName;
            if (this.coincidenceStation != null)
            {
                this.coincidenceStation.owner = ownerName;
            }
        }

        private int stationRank = 0;
        public int StationRank
        {
            get;
        }

        private int stationBasicRent;

        private int stationRent;
        public int StationRent
        {
            get
            {
                stationRent = stationBasicRent * stationRank;
                return stationRent;
            }
        }

        private int stationSellingPrice;

        public int StationSellingPrice
        {
            get { return stationSellingPrice; }
        }

        private int xAxis;          //x-axis of station position on the map
        private int yAxis;          //y-axis of station position on the map
        public int XAxis
        {
            get { return xAxis; }
        }
        public int YAxis
        {
            get { return yAxis; }
        }

        public bool StationRankUp()
        {
            if (stationRank < 5)
            {
                if (this.CoincidenceStation != null)
                    this.CoincidenceStation.stationRank++;
                stationRank++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
