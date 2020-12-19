export interface ISupplierProposalRequest {
  bidId?: string;
  supplierName?: string;
  supplierId?: string;
  publishedTime?: string;
  minimumUnits?: number;
  proposedPrice?: number;
  description?: string;
  progressBar?: JSX.Element;
}

export interface ISuppliersSectionProps {
  supplierProposalRequestList: ISupplierProposalRequest[] | undefined;
  numberOfParticipants: number;
  expirationDate: string;
}
