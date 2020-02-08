using Microsoft.ML;
using System;
using System.IO;
using System.Linq;

namespace HousePriceModelTrainer
{
    class HousePriceModelTrainer
    {
        public static void TrainHousePriceModelUsingCrossValidationPipeline(int folds = 10)
        {
            if (folds < 2)
            {
                Console.WriteLine("You need to generate at least 2 models.");
                return;
            }

            Console.WriteLine("Setting Data to Train a House Price Predictive Model. " + DateTime.Now);
            Console.WriteLine("There will be " + folds + " Models Generated to Test and Select the Most Accurate Model. " + DateTime.Now);

            //Create an MLContext Object Using the Microsoft ML.NET Framework
            MLContext mlContext = new MLContext(seed: 0);

            //Load Training Data From CSV File into an IDataView to Train the Model
            var trainingDataView = mlContext.Data.LoadFromTextFile<HouseDataModel>(Constants.TrainDataPath, hasHeader: true, separatorChar: ',');

            //Define the Training Algorithm (SDCA = Sthochastic Dual Coordinate Ascent)
            var trainer = mlContext.Regression.Trainers.Sdca();

            //Select and Formate (Encode) Data Features from Dataset
            var dataTransformPipeline = mlContext.Transforms.Categorical.OneHotEncoding("CatGarageType", inputColumnName: "GarageType")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("CatArea", inputColumnName: "Area"))
                .Append(mlContext.Transforms.Concatenate("Features", "Rooms", "BedRooms", "BedRoomsBsmt", "FullBath", "HalfBath", "ApproxSquFeet", "GarageSpaces", "ParkingSpaces", "CatGarageType", "CatArea"));

            Console.WriteLine("Starts Training Models and Getting Quality Metrics with Cross-Validation. " + DateTime.Now);

            //Create Model and Data Pipeline from IDataView
            var transformer = dataTransformPipeline.Fit(trainingDataView);
            var transformedData = transformer.Transform(trainingDataView);

            //Generate {folds} Prediction Models using Data Pipeline and Training Model, then Validate them using
            //Cross-Validation to get Models Quality Metrics
            var cvResults = mlContext.Regression.CrossValidate(transformedData, trainer, numberOfFolds: folds);

            //Orders Generated Models by Precission (From Most to Less) and Select the First Model (More Accurate)
            ITransformer topModel = cvResults.OrderByDescending(x => x.Metrics.RSquared).FirstOrDefault().Model;

            //Prints Model Quality Metrics
            ModelMetricsHelper.PrintRegressionFoldsAverageMetrics(trainer.ToString(), cvResults);
            
            //Verify if Output Paths Exists and Creates them if not
            if (!Directory.Exists(Path.GetDirectoryName(Constants.ModelPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Constants.ModelPath));
            }

            if (!Directory.Exists(Path.GetDirectoryName(Constants.DataTransformerPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Constants.DataTransformerPath));
            }

            //Verify if there are old Models with the Output Path and Removes Them
            if (File.Exists(Constants.ModelPath))
            {
                File.Delete(Constants.ModelPath);
            }

            if (File.Exists(Constants.DataTransformerPath))
            {
                File.Delete(Constants.DataTransformerPath);
            }

            //Saves Model and Data Pipeline to Output Paths
            using (var file = File.OpenWrite(Constants.DataTransformerPath))
            {
                mlContext.Model.Save(transformer, trainingDataView.Schema, file);
            }

            using (var file = File.OpenWrite(Constants.ModelPath))
            {
                mlContext.Model.Save(topModel, transformedData.Schema, file);
            }
        }
    }
}