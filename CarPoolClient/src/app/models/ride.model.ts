export interface RideDto {
  rideId: string;
  driverId: string;
  departureTime: string;
  route: RouteDto;
  availableSeats: number;
  bookedSeats?: BookingDto[];
  fare: number;
  rideCode?: string;
}

export interface RouteDto {
  startLocation?: string;
  endLocation?: string;
  stops?: string[];
}

export interface BookingDto {
  userId: string;
  status?: string;
}
