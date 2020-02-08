using HousePriceModelTrainer;
using HousePriceWebAPI.Models.REST;
using Microsoft.AspNetCore.Mvc;

namespace HousePriceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousePriceController : ControllerBase
    {
        // POST: api/HousePrice/Predict
        [HttpPost("Predict")]
        public ActionResult Post([FromBody] HousePriceRequest houseData)
        {
            HouseDataModel data = new HouseDataModel()
            {
                ApproxSquFeet = houseData.ApproxSquFeet,
                Area = houseData.Area,
                BedRooms = houseData.BedRooms,
                BedRoomsBsmt = houseData.BedRoomsBsmt,
                FullBath = houseData.FullBath,
                GarageSpaces = houseData.GarageSpaces,
                GarageType = houseData.GarageType,
                HalfBath = houseData.HalfBath,
                HouseType = houseData.HouseType,
                ParkingSpaces = houseData.ParkingSpaces,
                Rooms = houseData.Rooms
            };

            var price = HousePricePredictor.PredictSinglePrice(data);

            HousePriceResponse response = new HousePriceResponse()
            {
                ApproxSquFeet = data.ApproxSquFeet,
                Area = data.Area,
                BedRooms = data.BedRooms,
                BedRoomsBsmt = data.BedRoomsBsmt,
                FullBath = data.FullBath,
                GarageSpaces = data.GarageSpaces,
                GarageType = data.GarageType,
                HalfBath = data.HalfBath,
                ParkingSpaces = data.ParkingSpaces,
                Price = price.ToString("C2"),
                Rooms = data.Rooms
            };

            return Ok(response);
        }
    }
}