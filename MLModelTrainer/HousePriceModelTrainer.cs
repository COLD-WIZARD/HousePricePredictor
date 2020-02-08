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
                Console.WriteLine("Necesita Generar Minimo 2 Modelos");
                return;
            }

            Console.WriteLine("Preparando Data para Entrenar Modelo Predictivo de Precio de Venta para Casas. " + DateTime.Now);
            Console.WriteLine("Se Generaran " + folds + " Modelos para Evaluar y Seleccionar el de Mayor Exactitud. " + DateTime.Now);

            //Crea un Objeto tipo MLContext para Utilizar el Framework de ML.NET
            MLContext mlContext = new MLContext(seed: 0);

            //Carga los Datos desde el Archivo CSV en un DataView para Utilizarlos en el Entrenamiento
            var trainingDataView = mlContext.Data.LoadFromTextFile<HouseDataModel>(Constants.TrainDataPath, hasHeader: true, separatorChar: ',');

            //Crea el Algoritomo para Utilizar en el Entrenamiento (SDCA = Sthochastic Dual Coordinate Ascent)
            var trainer = mlContext.Regression.Trainers.Sdca();

            //Seleccion y Acondicionamiento (Codificacion) de Features desde DataSet (Variables Independientes)
            var dataTransformPipeline = mlContext.Transforms.Categorical.OneHotEncoding("CatGarageType", inputColumnName: "GarageType")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("CatArea", inputColumnName: "Area"))
                .Append(mlContext.Transforms.Concatenate("Features", "Rooms", "BedRooms", "BedRoomsBsmt", "FullBath", "HalfBath", "ApproxSquFeet", "GarageSpaces", "ParkingSpaces", "CatGarageType", "CatArea"));

            //Utilizamos Validación Cruzada para Estimar la Variacion de la Calidad del Modelo entre cada Ejecución,
            //y Obtenemos las Metricas de Calidad del Modelo
            Console.WriteLine("Entrenando y Obteniendo Metricas de Calidad del Modelo Mediante Cross-Validation. " + DateTime.Now);

            //Prepara y Transforma el DataView para el Entrenamiento
            var transformer = dataTransformPipeline.Fit(trainingDataView);
            var transformedData = transformer.Transform(trainingDataView);

            //Realiza el Entrenamiento de los Datos Transformados utilizando el Algoritmo de Entrenamiento
            //definido Anteriormente un total de "folds" veces (numberOfFolds) y Evalua la Calidad de estos
            var cvResults = mlContext.Regression.CrossValidate(transformedData, trainer, numberOfFolds: folds);

            //Ordena los Modelos Generados en base a su Precision (Mayor a Menor) y Selecciona el Modelo con
            //Mayor Exactitud
            ITransformer topModel = cvResults.OrderByDescending(x => x.Metrics.RSquared).FirstOrDefault().Model;

            //Imprime las Metricas del Modelo Seleccionado
            ModelMetricsHelper.PrintRegressionFoldsAverageMetrics(trainer.ToString(), cvResults);
            
            //Verificamos la Existencia del Directorio para Almacenar el Modelo. Si no Existe, lo Creamos
            if (!Directory.Exists(Path.GetDirectoryName(Constants.ModelPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Constants.ModelPath));
            }

            if (!Directory.Exists(Path.GetDirectoryName(Constants.DataTransformerPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Constants.DataTransformerPath));
            }

            //Verificamos la Existencia de Modelos Generados Anteriormente. Si Existe, lo Eliminamos
            if (File.Exists(Constants.ModelPath))
            {
                File.Delete(Constants.ModelPath);
            }

            if (File.Exists(Constants.DataTransformerPath))
            {
                File.Delete(Constants.DataTransformerPath);
            }

            //Guardamos el Modelo y el Transformador de los Datos Generados
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