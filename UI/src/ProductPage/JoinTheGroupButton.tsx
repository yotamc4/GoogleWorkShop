import React, { useCallback, useMemo, useState } from "react";

import { DefaultButton, Label } from "@fluentui/react";
import { BidBuyerJoinRequest } from "../Modal/ProductDetails";
import { useParams } from "react-router-dom";

import { useAuth0 } from "@auth0/auth0-react";
import { addBuyer, deleteBuyer } from "../Services/BidsControllerService";
import configData from "../config.json";

interface IJoinTheGroupButtonProps {
  isUserInBid: boolean;
  changeNumberOfParticipants: (addedNumber: number) => void;
}

export const JoinTheGroupButton: React.FunctionComponent<IJoinTheGroupButtonProps> = ({
  isUserInBid,
  changeNumberOfParticipants,
}) => {
  const { id } = useParams<{ id: string }>();
  const { isAuthenticated, user, getAccessTokenSilently } = useAuth0();
  const [
    isJoinTheGroupButtomClicked,
    setIsJoinTheGroupButtomClicked,
  ] = useState<boolean>(isUserInBid);
  const [isErrorMessageShowed, setIsErrorMessageShowed] = useState<boolean>(
    false
  );

  const onClickConsumerJoingButton = () => {
    if (isJoinTheGroupButtomClicked) {
      const url = `/${id}/buyers/${user.sub}`;
      deleteBuyer(url, getAccessTokenSilently);
      setIsJoinTheGroupButtomClicked(false);
      changeNumberOfParticipants(-1);
    } else {
      const bidBuyerJoinRequest: BidBuyerJoinRequest = {
        buyerId: user.sub,
        bidId: id,
        items: 1,
      };
      const url = `/${id}/buyers`;
      addBuyer(bidBuyerJoinRequest, url, getAccessTokenSilently);
      setIsJoinTheGroupButtomClicked(true);
      changeNumberOfParticipants(1);
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
    <>
      <DefaultButton
        text="Join the group"
        primary
        styles={{
          root: { borderRadius: 25, height: "4rem" },
          textContainer: { padding: "1rem", fontSize: "1.5rem" },
        }}
        height={"4rem"}
        onClick={() => {
          setIsErrorMessageShowed(true);
        }}
      />
      {isErrorMessageShowed ? (
        <Label
          styles={{ root: { color: "red", padding: "0", marginLeft: "2rem" } }}
        >
          Only login users can join the group{" "}
        </Label>
      ) : (
        <></>
      )}
    </>
  );
};
