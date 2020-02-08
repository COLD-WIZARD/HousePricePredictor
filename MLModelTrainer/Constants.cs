using System.IO;

namespace HousePriceModelTrainer
{
    public static class Constants
    {
        private static readonly string BasePath = @"C:\Users\IV317131\Desktop\Projects\VisualStudio\BHDLearning\MLModelTrainer";
        public static readonly string TrainDataPath = Path.Combine(BasePath, "Data", "HouseData.csv");
        public static readonly string ModelPath = Path.Combine(BasePath, "Model", "HousePriceModel.zip");
        public static readonly string DataTransformerPath = Path.Combine(BasePath, "Model", "HousePriceDataTransformer.zip");
    }
}