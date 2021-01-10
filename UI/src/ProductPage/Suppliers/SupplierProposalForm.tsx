import * as React from "react";
import { Stack } from "office-ui-fabric-react";
import {
  DefaultButton,
  MessageBar,
  MessageBarType,
  PrimaryButton,
  Separator,
  Spinner,
  SpinnerSize,
  Text,
  TextField,
} from "@fluentui/react";
import axios, { AxiosRequestConfig } from "axios";
import * as FormsStyles from "../../FormStyles/FormsStyles";
import { ISupplierProposalFormProps } from "./SupplierProposalForm.interface";
import { useParams } from "react-router";
import { ISupplierProposalRequest } from "./SupplierSection.interface";
import { useAuth0 } from "@auth0/auth0-react";
import { addSupplierProposal } from "../../Services/BidsControllerService";
import { horizontalGapStackToken } from "../../FormStyles/FormsStyles";

export const SupplierProposalForm: React.FunctionComponent<ISupplierProposalFormProps> = ({
  addPropposalToSupplierList,
  handleClose,
}) => {
  const [isDataLoaded, setIsDataLoaded] = React.useState<boolean>(false);
  const [errorMessage, setErrorMessage] = React.useState<string>();
  const { user, getAccessTokenSilently } = useAuth0();
  const { id } = useParams<{ id: string }>();
  const [formInputs, setFormInputs] = React.useReducer<
    (
      prevState: Partial<ISupplierProposalRequest>,
      state: Partial<ISupplierProposalRequest>
    ) => Partial<ISupplierProposalRequest>
  >(
    (
      prevState: Partial<ISupplierProposalRequest>,
      state: Partial<ISupplierProposalRequest>
    ) => ({
      ...prevState,
      ...state,
    }),
    {
      proposedPrice: undefined,
      minimumUnits: undefined,
      description: undefined,
      paymentLink: undefined,
    }
  );

  const onGetErrorMessage = (value: string) => {
    if (!isNaN(Number(value))) {
      return "";
    }
    return "Numbers only!";
  };

  //TODO: fix when the input isn't correct (wrong value or hasn't set)
  const onClickSend = async () => {
    setIsDataLoaded(true);
    try {
      const url = `/${id}/proposals`;
      const date = new Date();
      if (!(!formInputs.proposedPrice && !formInputs.minimumUnits)) {
        await addSupplierProposal(
          {
            publishedTime: date,
            bidId: id,
            supplierId: user.sub,
            supplierName: user.name,
            ...formInputs,
          },
          url,
          getAccessTokenSilently
        );
        addPropposalToSupplierList({
          publishedTime: String(date),
          bidId: id,
          supplierId: user.sub,
          supplierName: user.name,
          ...formInputs,
        });
      }
    } catch {
      setErrorMessage(
        "An error occurred while trying to add new proposal. Please try again later."
      );
    }
    setIsDataLoaded(false);
  };

  const onTextFieldChange = (
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string
  ): void => {
    if (
      (event.target as HTMLInputElement).id === "description" ||
      (event.target as HTMLInputElement).id === "paymentLink"
    ) {
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
      {errorMessage && (
        <MessageBar
          messageBarType={MessageBarType.error}
          onDismiss={() => setErrorMessage("")}
        >
          {errorMessage}
        </MessageBar>
      )}
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
          id="paymentLink"
          label="Payment link"
          ariaLabel="Required without visible label"
          required
          onChange={onTextFieldChange}
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
          <Stack horizontal tokens={horizontalGapStackToken}>
            {isDataLoaded && <Spinner size={SpinnerSize.small} />}
            <PrimaryButton
              onClick={onClickSend}
              text="Send"
              allowDisabledFocus
            />
          </Stack>
        </Stack>
      </Stack>
    </Stack>
  );
};
