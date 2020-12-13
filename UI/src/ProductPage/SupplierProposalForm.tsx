import * as React from "react";
import { Stack } from "office-ui-fabric-react";
import {
  DefaultButton,
  PrimaryButton,
  Separator,
  Text,
  TextField,
} from "@fluentui/react";
import axios from "axios";
import * as FormsStyles from "../FormStyles/FormsStyles";

export const SupplierProposalForm: React.FunctionComponent<ISupplierProposalFormProps> = ({
  addPropposalToSupplierList,
  handleClose,
}) => {
  const [formInputs, setFormInputs] = React.useReducer<
    (
      prevState: Partial<ISupplierProposalFormDetails>,
      state: Partial<ISupplierProposalFormDetails>
    ) => Partial<ISupplierProposalFormDetails>
  >(
    (
      prevState: Partial<ISupplierProposalFormDetails>,
      state: Partial<ISupplierProposalFormDetails>
    ) => ({
      ...prevState,
      ...state,
    }),
    {
      proposedPrice: undefined,
      minimumUnits: undefined,
      description: undefined,
    }
  );

  const onGetErrorMessage = (value: string) => {
    if (!isNaN(Number(value))) {
      return "";
    }
    return "Numbers only!";
  };

  //TODO: fix when the input isn't correct (wrong value or hasn't set)
  const onClickSend = () => {
    const date = Date();
    if (!(!formInputs.proposedPrice && !formInputs.minimumUnits)) {
      addPropposalToSupplierList({
        date: date,
        bidId: "123456",
        supplierId: "1234",
        supplierName: "Ivory",
        ...formInputs,
      });
      //TODO: needs to consume from the context the bidId and the supplierId
      postSupplierPropposal({
        date: date,
        bidId: "123456",
        supplierId: "1234",
        ...formInputs,
      });
    }
    //TODO: The user can't add the proposal need to return popup or some error message
  };

  const postSupplierPropposal = (
    supplierProposalFormDetails: Partial<ISupplierProposalFormDetails>
  ): void => {
    axios
      .post(
        "https://localhost:5001/api/v1/bids/1234/Proposals",
        supplierProposalFormDetails
      )
      .then((response) => {
        console.log(response);
      });
  };

  const onTextFieldChange = (
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string
  ): void => {
    if ((event.target as HTMLInputElement).id === "description") {
      setFormInputs({
        [(event.target as HTMLInputElement).id]: newValue,
      });
    } else if (!isNaN(Number(newValue))) {
      setFormInputs({
        [(event.target as HTMLInputElement).id]: Number(newValue),
      });
    }
  };

  return (
    <Stack horizontalAlign={"center"}>
      <Text
        block={true}
        className="Bold"
        styles={FormsStyles.headerStyle}
        variant="xLargePlus"
      >
        New Supplier proposal for a product
      </Text>
      <Stack
        styles={{ root: { width: "80%" } }}
        tokens={FormsStyles.verticalGapStackTokens}
      >
        <Separator styles={{ root: { width: "100%" } }} />
        <TextField
          id="proposedPrice"
          label="Price"
          ariaLabel="Required without visible label"
          required
          onChange={onTextFieldChange}
          onGetErrorMessage={onGetErrorMessage}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <TextField
          id="minimumUnits"
          label="Minimum units"
          ariaLabel="Required without visible label"
          required
          onChange={onTextFieldChange}
          onGetErrorMessage={onGetErrorMessage}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <TextField
          id="description"
          label="Description"
          multiline
          rows={3}
          onChange={onTextFieldChange}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <Separator styles={{ root: { width: FormsStyles.inputWidth } }} />
        <Stack
          horizontal
          tokens={FormsStyles.horizontalGapStackTokensForButtons}
          styles={{ root: { margin: "auto" } }}
        >
          <DefaultButton
            onClick={handleClose}
            text="Cancel"
            allowDisabledFocus
          />
          <PrimaryButton onClick={onClickSend} text="Send" allowDisabledFocus />
        </Stack>
      </Stack>
    </Stack>
  );
};

//needs to add the description
export interface ISupplierProposalFormDetails {
  proposedPrice?: number;
  minimumUnits?: number;
  description?: string;
  date: string;
  supplierId: string;
  bidId: string;
  supplierName: string;
}

interface ISupplierProposalFormProps {
  addPropposalToSupplierList: (
    supplierProposalFormDetails: ISupplierProposalFormDetails
  ) => void;
  handleClose: () => void;
}
