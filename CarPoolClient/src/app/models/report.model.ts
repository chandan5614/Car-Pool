export interface ReportDto {
  reportId: string;
  rideId: string;
  userId: string;
  issue?: string;
  status?: string;
  timestamp: string;
}
