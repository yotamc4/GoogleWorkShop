export interface ISupplierProposalRequest {
  bidId?: string;
  supplierName?: string;
  supplierId?: string;
  publishedTime?: string | Date;
  minimumUnits?: number;
  proposedPrice?: number;
  description?: string;
  progressBar?: JSX.Element;
}

export interface ISuppliersSectionProps {
  supplierProposalRequestList: ISupplierProposalRequest[] | undefined;
  numberOfParticipants: number;
  bidPhase: number;
  isUserInBid: boolean | undefined;
}
