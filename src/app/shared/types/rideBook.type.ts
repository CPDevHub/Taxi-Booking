import { VehicleType } from "../enums/vehicleType.enums";

export type rideBookType = {
  pickupLocation: {
    address: string;
    latitude: number;
    longitude: number;
  };
  dropOffLocation: {
    address: string;
    latitude: number;
    longitude: number;
  };
  rideVehicle:VehicleType
};
