using System;

namespace HousePriceModelTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creates and Stores Predictive Model
            HousePriceModelTrainer.TrainHousePriceModelUsingCrossValidationPipeline(3);

            //Creates Some Samples to Test Model
            var housePriceSample1 = new HouseDataModel() { ApproxSquFeet = 1300, Area = "76", BedRooms = 3, BedRoomsBsmt = 0, FullBath = 2, GarageSpaces = 2, GarageType = "Attached", HalfBath = 0, HouseType = "", ParkingSpaces = 1, Rooms = 7 };
            var housePriceSample2 = new HouseDataModel() { ApproxSquFeet = 8410, Area = "62", BedRooms = 5, BedRoomsBsmt = 1, FullBath = 6, GarageSpaces = 4, GarageType = "Attached", HalfBath = 1, HouseType = "", ParkingSpaces = 1, Rooms = 16 };

            //Creates a Multi-Sample Array to Test Multiple Prediction
            HouseDataModel[] hd = new HouseDataModel[] { housePriceSample1, housePriceSample2 };

            //Makes Predictions Using Multiple Samples at Once and then Individually
            var results = HousePricePredictor.PredictSinglePriceSet(hd);
            var result = HousePricePredictor.PredictSinglePrice(housePriceSample1);

            //Prints Predicted Prices
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine("Precio Estimado Para Casa " + (i + 1) + ": " + results[i].ToString("C2"));
            }

            Console.WriteLine("Precio Estimado Para Casa 1: " + result.ToString("C2"));
        }
    }
}