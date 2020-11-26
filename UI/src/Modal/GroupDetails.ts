import { SupplierSuggestion } from "./SupplierSuggestion";

export interface GroupDetails {
  numberOfParticipants: number;
  groupExpirationDate: string;
  suppliers?: SupplierSuggestion[];
}
