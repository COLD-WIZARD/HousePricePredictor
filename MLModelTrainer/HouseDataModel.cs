using Microsoft.ML.Data;

namespace HousePriceModelTrainer
{
    public class HouseDataModel
    {
        [LoadColumn(10)]
        public float ApproxSquFeet;

        [LoadColumn(8)]
        public string Area;

        [LoadColumn(15)]
        public float BedRooms;

        [LoadColumn(16)]
        public float BedRoomsBsmt;

        [LoadColumn(12)]
        public float FullBath;

        [LoadColumn(18)]
        public float GarageSpaces;

        [LoadColumn(17)]
        public string GarageType;

        [LoadColumn(13)]
        public float HalfBath;

        [LoadColumn(4)]
        public string HouseType;

        [LoadColumn(3)]
        public float Label;

        [LoadColumn(19)]
        public float ParkingSpaces;

        [LoadColumn(11)]
        public float Rooms;
    }

    //Valores a Predecir
    public class HousePricePrediction
    {
        [ColumnName("Score")]
        public float SoldPrice;
    }
}