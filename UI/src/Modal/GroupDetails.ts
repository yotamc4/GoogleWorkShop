import { newProductRequest } from "./ProductDetails";
import { SupplierSuggestion } from "./SupplierSuggestion";

export interface GroupDetails {
  numberOfParticipants: number;
  groupExpirationDate: string;
  suppliers?: SupplierSuggestion[];
}

export interface NewBidRequest {
  ownerId: string;
  category: string;
  subCategory: string;
  expirationDate: Date;
  maxPrice: number;
  product: newProductRequest | undefined;
}

export interface Bid extends NewBidRequest {
  id: string;
  potenialSuplliersCounter: number;
  unitsCounter: number;
  creationDate: Date;
  phase: number;
}
