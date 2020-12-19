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
  Name: string;
  Image: string | undefined;
  Description: string;
}
