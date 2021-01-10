import React, { useCallback, useMemo, useState } from "react";

import {
  DefaultButton,
  Label,
  MessageBar,
  MessageBarType,
  Spinner,
  SpinnerSize,
  Stack,
} from "@fluentui/react";
import { BidBuyerJoinRequest } from "../Modal/ProductDetails";
import { useParams } from "react-router-dom";

import { useAuth0 } from "@auth0/auth0-react";
import { addBuyer, deleteBuyer } from "../Services/BidsControllerService";
import configData from "../config.json";
import { horizontalGapStackToken } from "../FormStyles/FormsStyles";

interface IJoinTheGroupButtonProps {
  changeNumberOfParticipants: (addedNumber: number) => void;
  handleClickOpen: () => void;
  setIsJoinTheGroupButtomClicked: React.Dispatch<React.SetStateAction<boolean>>;
  isJoinTheGroupButtomClicked: boolean;
}

export const JoinTheGroupButton: React.FunctionComponent<IJoinTheGroupButtonProps> = ({
  changeNumberOfParticipants,
  handleClickOpen,
  setIsJoinTheGroupButtomClicked,
  isJoinTheGroupButtomClicked,
}) => {
  const [errorMessage, setErrorMessage] = React.useState<string>();
  const [isDeleteButtonClicked, setIsDeleteButtonClicked] = React.useState<
    boolean
  >(false);
  const { id } = useParams<{ id: string }>();
  const { isAuthenticated, user, getAccessTokenSilently } = useAuth0();

  const onClickConsumerJoingButton = async () => {
    if (isJoinTheGroupButtomClicked) {
      setIsDeleteButtonClicked(true);
      try {
        const url = `/${id}/buyers`;
        await deleteBuyer(url, getAccessTokenSilently);
        setIsJoinTheGroupButtomClicked(false);
        changeNumberOfParticipants(-1);
      } catch {
        setErrorMessage(
          "An error occurred while trying to delete your participation."
        );
      }
      setIsDeleteButtonClicked(false);
    } else {
      handleClickOpen();
    }
  };

  return isAuthenticated ? (
    user[configData.roleIdentifier] === "Supplier" ? (
      <></>
    ) : (
      <Stack tokens={horizontalGapStackToken}>
        <Stack horizontal tokens={horizontalGapStackToken}>
          <DefaultButton
            text={
              isJoinTheGroupButtomClicked
                ? "Cancel my participation"
                : "Join the group"
            }
            primary
            styles={{
              root: { borderRadius: 25, height: "4rem" },
              textContainer: { padding: "1rem", fontSize: "1.5rem" },
            }}
            height={"4rem"}
            onClick={onClickConsumerJoingButton}
          />
          {isDeleteButtonClicked && <Spinner size={SpinnerSize.small} />}
        </Stack>
        {errorMessage && (
          <MessageBar
            styles={{ root: { width: "", height: "2rem" } }}
            messageBarType={MessageBarType.error}
            onDismiss={() => setErrorMessage("")}
            isMultiline={true}
          >
            {errorMessage}
          </MessageBar>
        )}
      </Stack>
    )
  ) : (
    <DefaultButton
      text="Join the group"
      primary
      styles={{
        root: { borderRadius: 25, height: "4rem" },
        textContainer: { padding: "1rem", fontSize: "1.5rem" },
      }}
      height={"4rem"}
      disabled={true}
    />
  );
};
