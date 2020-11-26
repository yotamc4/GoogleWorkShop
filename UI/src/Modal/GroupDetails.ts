import { SupplierSuggestion } from "./SupplierSuggestion";

export interface GroupDetails {
  numberOfParticipants: number;
  groupExpirationDate: Date;
  suppliers: SupplierSuggestion[];
}
