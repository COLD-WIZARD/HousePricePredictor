using Microsoft.ML;
using Microsoft.ML.Data;

namespace HousePriceModelTrainer
{
    public static class HousePricePredictor
    {
        //Makes Predictions From a Multiple Samples at Once
        public static float[] PredictSinglePriceSet(HouseDataModel[] houseData)
        {
            //Create an MLContext Object Using the Microsoft ML.NET Framework
            MLContext mlContext = new MLContext();

            //Creates an IDataView using Supplied HouseData Array (Input Data for Predictions)
            IDataView data = mlContext.Data.LoadFromEnumerable(houseData);

            //Load Data Pipeline and Model to Predict
            ITransformer dataPrepPipeline = mlContext.Model.Load(Constants.DataTransformerPath, out _);
            ITransformer trainedModel = mlContext.Model.Load(Constants.ModelPath, out _);

            //Transforms Input Data Using Data Pipeline
            var transformedData = dataPrepPipeline.Transform(data);

            //Uses Transformed Data to Predict Prices
            var predictedPrices = trainedModel.Transform(transformedData);

            //Get Predicted Results and Generates an Output Prices Array
            float[] results = new float[houseData.Length];
            var scoreColumn = predictedPrices.GetColumn<float>("Score");

            int i = 0;
            foreach (var r in scoreColumn)
            {
                results[i++] = r;
            }

            return results;
        }

        //Makes Predictions From a Single Sample
        public static float PredictSinglePrice(HouseDataModel houseData)
        {
            return PredictSinglePriceSet(new HouseDataModel[] { houseData })[0];
        }
    }
}