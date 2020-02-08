using Microsoft.ML;
using Microsoft.ML.Data;

namespace HousePriceModelTrainer
{
    public static class HousePricePredictor
    {
        public static float[] PredictSinglePriceSet(HouseDataModel[] houseData)
        {
            //Crea un Objeto tipo MLContext para Utilizar el Framework de ML.NET
            MLContext mlContext = new MLContext();

            //Crea un DataView a partir del Arreglo de Objetos de tipo HouseData
            IDataView data = mlContext.Data.LoadFromEnumerable(houseData);

            //Carga los Datos y Prepara los Objetos para el Pipeline y el Modelo Entrenado
            ITransformer dataPrepPipeline = mlContext.Model.Load(Constants.DataTransformerPath, out _);
            ITransformer trainedModel = mlContext.Model.Load(Constants.ModelPath, out _);

            //Transforma la Data para Utilizar en las Predicciones
            var transformedData = dataPrepPipeline.Transform(data);

            //Usa los Datos Transformados para Producir un Set de Predicciones
            var predictedPrices = trainedModel.Transform(transformedData);

            //Obtiene los Resultados y los Almacena en un Arreglo
            float[] results = new float[houseData.Length];
            var scoreColumn = predictedPrices.GetColumn<float>("Score");

            int i = 0;
            foreach (var r in scoreColumn)
            {
                results[i++] = r;
            }

            return results;
        }

        public static float PredictSinglePrice(HouseDataModel houseData)
        {
            return PredictSinglePriceSet(new HouseDataModel[] { houseData })[0];
        }
    }
}