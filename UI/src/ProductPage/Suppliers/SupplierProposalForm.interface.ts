import { ISupplierProposalRequest } from "./SupplierSection.interface";


export interface ISupplierProposalFormProps {
  addPropposalToSupplierList: (
    supplierProposalFormDetails: ISupplierProposalRequest
  ) => void;
  handleClose: () => void;
}
