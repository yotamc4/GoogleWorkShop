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
  phase: number;
  isUserInBid: boolean;
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
