export type rideDetailsType = {
  rideId: number;
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
  passengerName: string;
  contactNumber: string;
};
