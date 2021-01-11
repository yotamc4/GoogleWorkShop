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
import * as FormsStyles from "../FormStyles/FormsStyles";
import { useParams } from "react-router";
import { useAuth0 } from "@auth0/auth0-react";
import { addBuyer } from "../Services/BidsControllerService";
import { BidBuyerJoinRequest2 } from "../Modal/ProductDetails";
import { horizontalGapStackToken } from "../FormStyles/FormsStyles";
import { stringNotContainsOnlyNumbers } from "../Utils/FormUtils";

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
  const [isDataLoaded, setIsDataLoaded] = React.useState<boolean>(false);
  const { user, getAccessTokenSilently } = useAuth0();
  const { id } = useParams<{ id: string }>();
  const [errorMessage, setErrorMessage] = React.useState<string>();
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

  const onClickSend = async () => {
    setIsDataLoaded(true);
    try {
      const bidBuyerJoinRequest: BidBuyerJoinRequest2 = {
        buyerId: user.sub,
        bidId: id,
        buyerName: user.name,
        ...formInputs,
      };

      const url = `/${id}/buyers`;
      await addBuyer(bidBuyerJoinRequest, url, getAccessTokenSilently);
      setIsJoinTheGroupButtonClicked(true);
      handleClose();
      changeNumberOfParticipants(1);
    } catch {
      setIsJoinTheGroupButtonClicked(false);
      setErrorMessage(
        "An error occurred while trying to join the group. Please try again later."
      );
    }

    setIsDataLoaded(false);
  };

  const onTextFieldChange = (
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string
  ): void => {
    if ((event.target as HTMLInputElement).id === "buyerAddress") {
      if (!stringNotContainsOnlyNumbers(newValue)) {
        newValue = "";
      }

      setFormInputs({
        [(event.target as HTMLInputElement).id]: newValue,
      });
    } else {
      if (stringNotContainsOnlyNumbers(newValue)) {
        newValue = "";
      }

      setFormInputs({
        [(event.target as HTMLInputElement).id]: Number(newValue),
      });
    }
  };

  const validateInputIsNumber = (value: string): string => {
    if (stringNotContainsOnlyNumbers(value)) {
      return "Only numbers allowed";
    } else {
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
      <Separator styles={{ root: { width: "100%" } }} />
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
            if (value && !stringNotContainsOnlyNumbers(value)) {
              return "Only numbers are not allowed";
            } else {
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
          <Stack horizontal tokens={horizontalGapStackToken}>
            {isDataLoaded && <Spinner size={SpinnerSize.small} />}
            <PrimaryButton
              onClick={onClickSend}
              text="Send"
              disabled={
                !(
                  formInputs.numOfUnits &&
                  formInputs.buyerAddress &&
                  formInputs.buyerPhoneNumber &&
                  formInputs.buyerPostalCode
                )
              }
            />
          </Stack>
        </Stack>
      </Stack>
    </Stack>
  );
};
