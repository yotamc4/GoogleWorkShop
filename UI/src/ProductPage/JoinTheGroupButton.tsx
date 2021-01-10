import React, { useCallback, useMemo, useState } from "react";

import { DefaultButton, Label } from "@fluentui/react";
import { BidBuyerJoinRequest } from "../Modal/ProductDetails";
import { useParams } from "react-router-dom";

import { useAuth0 } from "@auth0/auth0-react";
import { addBuyer, deleteBuyer } from "../Services/BidsControllerService";
import configData from "../config.json";

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
  const { id } = useParams<{ id: string }>();
  const { isAuthenticated, user, getAccessTokenSilently } = useAuth0();

  const onClickConsumerJoingButton = () => {
    if (isJoinTheGroupButtomClicked) {
      const url = `/${id}/buyers`;
      deleteBuyer(url, getAccessTokenSilently);
      setIsJoinTheGroupButtomClicked(false);
      changeNumberOfParticipants(-1);
    } else {
      handleClickOpen();
    }
  };

  return isAuthenticated ? (
    user[configData.roleIdentifier] === "Supplier" ? (
      <></>
    ) : (
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
