import * as React from "react";
import { Stack } from "office-ui-fabric-react";
import {
  DefaultButton,
  PrimaryButton,
  Separator,
  Text,
  TextField,
} from "@fluentui/react";
import * as FormsStyles from "../FormStyles/FormsStyles";
import { ISupplierProposalFormProps } from "./Suppliers/SupplierProposalForm.interface";
import { useParams } from "react-router";
import { useAuth0 } from "@auth0/auth0-react";
import {
  addBuyer,
  addSupplierProposal,
  deleteBuyer,
} from "../Services/BidsControllerService";
import {
  BidBuyerJoinRequest,
  BidBuyerJoinRequest2,
} from "../Modal/ProductDetails";

export interface IJoinTheGroupFormProps {
  handleClose: () => void;
  changeNumberOfParticipants: (addedNumber: number) => void;
  setIsJoinTheGroupButtonClicked: React.Dispatch<React.SetStateAction<boolean>>;
}

export const JoinTheGroupForm: React.FunctionComponent<IJoinTheGroupFormProps> = ({
  handleClose,
  changeNumberOfParticipants,
  setIsJoinTheGroupButtonClicked: setIsJoinTheGroupButtonClicked,
}) => {
  const { user, getAccessTokenSilently } = useAuth0();
  const { id } = useParams<{ id: string }>();
  const [inputsAreValid, setInputsAreValid] = React.useState<boolean>(true);
  const [formInputs, setFormInputs] = React.useReducer<
    (
      prevState: Partial<BidBuyerJoinRequest2>,
      state: Partial<BidBuyerJoinRequest2>
    ) => Partial<BidBuyerJoinRequest2>
  >(
    (
      prevState: Partial<BidBuyerJoinRequest2>,
      state: Partial<BidBuyerJoinRequest2>
    ) => ({
      ...prevState,
      ...state,
    }),
    {
      numOfUnits: 1,
      buyerAddress: undefined,
      buyerPostalCode: undefined,
      buyerPhoneNumber: undefined,
    }
  );

  const onClickSend = () => {
    const bidBuyerJoinRequest: BidBuyerJoinRequest2 = {
      buyerId: user.sub,
      bidId: id,
      buyerName: user.name,
      ...formInputs,
    };
    const url = `/${id}/buyers`;
    addBuyer(bidBuyerJoinRequest, url, getAccessTokenSilently);
    setIsJoinTheGroupButtonClicked(true);
    handleClose();
    changeNumberOfParticipants(1);
  };

  const onTextFieldChange = (
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string
  ): void => {
    if ((event.target as HTMLInputElement).id === "buyerAddress") {
      setFormInputs({
        [(event.target as HTMLInputElement).id]: newValue,
      });
    } else {
      setFormInputs({
        [(event.target as HTMLInputElement).id]: Number(newValue),
      });
    }
  };

  const validateInputIsNumber = (value: string): string => {
    if (value && value.match(/^[0-9]+$/) === null) {
      setInputsAreValid(false);
      return "Only numbers allowed";
    } else {
      setInputsAreValid(true);
      return "";
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
        Join the group
      </Text>
      <Stack
        styles={{ root: { width: "80%" } }}
        tokens={FormsStyles.verticalGapStackTokens}
      >
        <Separator styles={{ root: { width: "100%" } }} />
        <TextField
          id="numOfUnits"
          label="Units"
          ariaLabel="Required without visible label"
          required
          onChange={onTextFieldChange}
          onGetErrorMessage={validateInputIsNumber}
          styles={{ root: { width: FormsStyles.inputWidth } }}
          defaultValue={"1"}
        />
        <TextField
          id="buyerAddress"
          label="Buyer address"
          ariaLabel="Required without visible label"
          required
          onGetErrorMessage={(value) => {
            if (value && value.match(/^[0-9]+$/) != null) {
              setInputsAreValid(false);
              return "Only numbers are not allowed";
            } else {
              setInputsAreValid(true);
              return "";
            }
          }}
          onChange={onTextFieldChange}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <TextField
          id="buyerPostalCode"
          label="Postal code"
          ariaLabel="Required without visible label"
          required
          onGetErrorMessage={validateInputIsNumber}
          onChange={onTextFieldChange}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <TextField
          id="buyerPhoneNumber"
          label="Phone number"
          ariaLabel="Required without visible label"
          required
          onGetErrorMessage={validateInputIsNumber}
          onChange={onTextFieldChange}
          styles={{ root: { width: FormsStyles.inputWidth } }}
        />
        <Separator styles={{ root: { width: FormsStyles.inputWidth } }} />
        <Stack horizontal horizontalAlign={"space-between"}>
          <DefaultButton
            onClick={handleClose}
            text="Cancel"
            allowDisabledFocus
          />
          <PrimaryButton
            onClick={onClickSend}
            text="Send"
            disabled={
              !(
                formInputs.numOfUnits &&
                formInputs.buyerAddress &&
                formInputs.buyerPhoneNumber &&
                formInputs.buyerPostalCode
              ) || !inputsAreValid
            }
          />
        </Stack>
      </Stack>
    </Stack>
  );
};
