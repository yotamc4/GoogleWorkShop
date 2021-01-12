export interface IParticipancyFullDetails {
  profilePicture:string;
  buyerId: string;
  buyerName: string;
  numOfUnits: number;
  hasPaid: boolean;
  buyerAddress: string;
  buyerPostalCode: string;
  buyerPhoneNumber: string;
}

export interface IMarkPaidRequest {
  buyerId: string;
  hasPaid: boolean;
  bidId: string;
}

