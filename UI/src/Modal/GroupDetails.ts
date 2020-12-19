import { newProductRequest } from "./ProductDetails";
import { SupplierSuggestion } from "./SupplierSuggestion";

export interface GroupDetails {
  numberOfParticipants: number;
  groupExpirationDate: string;
  suppliers?: SupplierSuggestion[];
}

export interface NewBidRequest {
  OwnerId: string;
  Category: string;
  SubCategory: string;
  ExpirationDate: Date | null | undefined;
  MaxPrice: number;
  Product: newProductRequest | undefined;
}
