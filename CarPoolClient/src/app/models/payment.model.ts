export interface PaymentDto {
  paymentId: string;
  rideId: string;
  userId: string;
  fare: number;
  status?: string;
  timestamp: string;
}
