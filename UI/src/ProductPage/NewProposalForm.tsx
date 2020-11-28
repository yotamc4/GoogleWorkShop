import * as React from "react";
import { Stack, Label } from "office-ui-fabric-react";
import {
  DefaultButton,
  PrimaryButton,
  Separator,
  Text,
  TextField,
} from "@fluentui/react";
import * as FormsStyles from "../FormStyles/FormsStyles";

//needs to add the description
interface INewProposalDetails {
  price?: number;
  minimumUnits?: number;
}

interface INewProposalFormProps {
  addPropposalToSupplierList: (price: number, minimumUnits: number) => void;
  handleClose: () => void;
}

export const NewProposalForm: React.FunctionComponent<INewProposalFormProps> = ({
  addPropposalToSupplierList,
  handleClose,
}) => {
  const [formInputs, setFormInputs] = React.useReducer<
    (
      prevState: INewProposalDetails,
      state: INewProposalDetails
    ) => INewProposalDetails
  >(
    (prevState: INewProposalDetails, state: INewProposalDetails) => ({
      ...prevState,
      ...state,
    }),
    {
      price: undefined,
      minimumUnits: undefined,
    }
  );
  //fix when the input isn't correct (wrong value or hasn't set)
  const onSave = () => {
    if (formInputs.price != undefined && formInputs.minimumUnits != undefined) {
      return addPropposalToSupplierList(
        formInputs.price,
        formInputs.minimumUnits
      );
    }
    return addPropposalToSupplierList(
      formInputs.price as number,
      formInputs.minimumUnits as number
    );
  };

  const onTextFieldChange = (
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string
  ): void => {
    setFormInputs({
      [(event.target as HTMLInputElement).id]: Number(newValue),
    });
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
          id="price"
          label="Price"
          ariaLabel="Required without visible label"
          required
          onChange={onTextFieldChange}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <TextField
          id="minimumUnits"
          label="Minimum units"
          ariaLabel="Required without visible label"
          required
          onChange={onTextFieldChange}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <TextField
          label="Description"
          multiline
          rows={3}
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
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            // checked={checked}
          />
          <PrimaryButton
            onClick={onSave}
            text="Send"
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            //checked={checked}
          />
        </Stack>
      </Stack>
    </Stack>
  );
};
