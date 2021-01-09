export interface IParticipancyFullDetails {
  profilePicture:string;
  buyerId: string;
  buyerName: string;
  numOfUnits: number;
  hasPaid: boolean;
  BuyerAddress: string;
  BuyerPostalCode: string;
  BuyerPhoneNumber: string;
}

export interface IMarkPaidRequest {
  buyerId: string;
  hasPaid: boolean;
  bidId: string;
}
