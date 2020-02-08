namespace HousePriceWebAPI.Models.REST
{
    public class HousePriceRequest
    {
        public float ApproxSquFeet { get; set; }
        public string Area { get; set; }
        public float BedRooms { get; set; }
        public float BedRoomsBsmt { get; set; }
        public float FullBath { get; set; }
        public float GarageSpaces { get; set; }
        public string GarageType { get; set; }
        public float HalfBath { get; set; }
        public string HouseType { get; set; }
        public float ParkingSpaces { get; set; }
        public float Rooms { get; set; }
    }
}