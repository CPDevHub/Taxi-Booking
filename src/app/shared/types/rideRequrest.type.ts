export type RideRequestType = {
  id: number;
  pickupLocation: {
    address:string,
    latitude:number,
    longitude:number;
  };
  dropOffLocation: {
    address:string,
    latitude:number,
    longitude:number;
  };
  //   fare: number;
  //   distance: number;
  //   userId: string;
};
