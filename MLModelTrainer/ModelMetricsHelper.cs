using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;

namespace HousePriceModelTrainer
{
    class ModelMetricsHelper
    {
        public static void PrintRegressionFoldsAverageMetrics(string algorithmName, IReadOnlyList<TrainCatalogBase.CrossValidationResult<RegressionMetrics>> crossValidationResults)
        {
            Console.WriteLine($"**********************************************");
            Console.WriteLine($"* Metrics for Trainer: {algorithmName}        ");
            Console.WriteLine($"*---------------------------------------------");
            Console.WriteLine();

            int i = 1;
            foreach (var result in crossValidationResults)
            {
                Console.WriteLine($"* Metrics for Model: " + i++);
                Console.WriteLine($"* Loss Function:\t\t{result.Metrics.LossFunction.ToString("N3")}  ");
                Console.WriteLine($"* Mean Absolute Error:\t\t{result.Metrics.MeanAbsoluteError.ToString("N3")}  ");
                Console.WriteLine($"* Mean Squared Error:\t\t{result.Metrics.MeanSquaredError.ToString("N3")}  ");
                Console.WriteLine($"* Root Mean Squared Error:\t{result.Metrics.RootMeanSquaredError.ToString("N3")}  ");
                Console.WriteLine($"* R-squared:\t\t\t{result.Metrics.RSquared.ToString("N3")}  ");
                Console.WriteLine();
            }

            Console.WriteLine($"**********************************************");
        }
    }
}