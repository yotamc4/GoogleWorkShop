export interface BidDetails {
  id: string;
  product: newProductRequest;
  ownerId: string;
  category: number;
  subCategory: string;
  maxPrice: string;
  creationDate: string;
  expirationDate: string;
  potenialSuplliersCounter: number;
  unitsCounter: number;
  phase: Phase;
  isUserInBid: boolean;
  hasVoted: boolean;
}

export interface ProductDetails {
  name: string;
  category: string;
  subCategory: string;
  maximumAcceptablePrice: number;
  groupExpirationDate: Date;
  imageUrl: string;
  description: string;
  mockId: number;
  supplierHasChosen?: boolean;
}

export interface newProductRequest {
  name: string;
  image: string | undefined;
  description: string;
}

export interface BidBuyerJoinRequest {
  buyerId: string;
  bidId: string;
  items: number;
}

export enum Phase {
  Join,
  Vote,
  Payment,
  CancelledSupplierNotFound,
  CancelledNotEnoughBuyersPayed,
  Completed,
}

export const PhasesName: Map<Phase, string> = new Map([
  [Phase.Join, "Open to join"],
  [Phase.Vote, "Voting"],
  [Phase.Payment, "Payment"],
  [Phase.CancelledSupplierNotFound, "Cancelled- supplier has not found"],
  [Phase.CancelledNotEnoughBuyersPayed, "Cancelled- not enough buyers payed"],
  [Phase.Completed, "Completed"],
]);
