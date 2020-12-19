export interface BidDetails {
  id: string;
  product: ProductDetails1;
  ownerId: string;
  category: number;
  aubCategory: Date;
  maxPrice: string;
  creationDate: string;
  expirationDate: string;
  potenialSuplliersCounter: number;
  unitsCounter: number;
}

export interface ProductDetails1 {
  name: string;
  image: string;
  description: string;
}

export interface newProductRequest {
  Name: string;
  Image: string | undefined;
  Description: string;
}
