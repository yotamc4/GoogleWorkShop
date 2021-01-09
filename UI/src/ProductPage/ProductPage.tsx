import React, { useCallback, useMemo, useState } from "react";
import axios from "axios";
import * as Styles from "./ProductPageStyles";
import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import * as MockBuyers from "../Modal/MockBuyers";
import {
  FontIcon,
  Image,
  Separator,
  Spinner,
  SpinnerSize,
  Stack,
  Text,
} from "@fluentui/react";
import { SuppliersSection } from "./Suppliers/SupplierSection";
import { BidDetails, Phase } from "../Modal/ProductDetails";
import { useParams } from "react-router-dom";
import { PaymentsTable } from "../PaymentTable/PaymentTable";
import { ISupplierProposalRequest } from "./Suppliers/SupplierSection.interface";
import { ShareProductBar } from "./ShareProductBar";
import { useAuth0 } from "@auth0/auth0-react";
import { JoinTheGroupButton } from "./JoinTheGroupButton";
import configData from "../config.json";
import { getDate } from "./utils";
import ButtonAppBar from "../LoginBar";
import {
  getBidParticipations,
  getBidParticipationsFullDetails,
  getBidSpecific,
  getProposals,
} from "../Services/BidsControllerService";
import { IParticipancyFullDetails } from "../PaymentTable/PaymentTable.interface";
import { JoinTheGroupForm } from "./JoinTheGroupForm";

export const ProductPage: React.FunctionComponent = () => {
  const {
    isAuthenticated,
    user,
    isLoading,
    getAccessTokenSilently,
  } = useAuth0();
  const [numberOfParticipants, setnumberOfParticipants] = React.useState<
    number
  >(0);
  const [
    supplierProposalRequestList,
    setsupplierProposalRequestList,
  ] = React.useState<ISupplierProposalRequest[] | undefined>(undefined);
  const [bidDetails, setBidDetails] = useState<BidDetails | undefined>(
    undefined
  );
  const [paymentList, setPaymentList] = useState<
    Partial<IParticipancyFullDetails>[] | undefined
  >(undefined);
  const [isDataLoaded, setIsDataLoaded] = useState<boolean>(false);
  const [isChosenSupplier, setIsChosenSupplier] = useState<boolean>(false);
  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };
  const [
    isJoinTheGroupButtomClicked,
    setIsJoinTheGroupButtomClicked,
  ] = useState<boolean>(bidDetails?.isUserInBid as boolean);

  const [open, setOpen] = React.useState(false);
  const { id } = useParams<{ id: string }>();
  React.useEffect(() => {
    const getBid = async () => {
      let role: string;
      if (isAuthenticated) {
        role = user[configData.roleIdentifier];
      } else {
        role = "Anonymous";
      }
      const [
        bidDetailsResponse,
        supplierProposalRequestListResponse,
      ] = await Promise.all([
        getBidSpecific(
          `/${id}?role=${role}&id=${user?.sub}`,
          isAuthenticated,
          getAccessTokenSilently
        ),
        getProposals(
          `/${id}/proposals`,
          isAuthenticated,
          getAccessTokenSilently
        ),
      ]);
      const bidDetailsResponseJson: BidDetails = await bidDetailsResponse.json();
      const supplierProposalRequestListResponseJson: ISupplierProposalRequest[] = await supplierProposalRequestListResponse.json();

      setBidDetails(bidDetailsResponseJson);
      setnumberOfParticipants(bidDetailsResponseJson.unitsCounter);
      setsupplierProposalRequestList(supplierProposalRequestListResponseJson);

      if (
        bidDetailsResponseJson.phase === Phase.Payment &&
        bidDetailsResponseJson.isUserInBid &&
        user[configData.roleIdentifier] === "Supplier"
      ) {
        const response = await getBidParticipations(
          `/${id}/participantsFullDetails`,
          isAuthenticated,
          getAccessTokenSilently
        );
        const bidParticipationsListJson: Partial<
          IParticipancyFullDetails
        >[] = await response.json();
        setPaymentList(bidParticipationsListJson);
        setIsChosenSupplier(true);
      } else if (bidDetailsResponseJson.phase === Phase.Payment) {
        const response = await getBidParticipationsFullDetails(
          `/${id}/participants`,
          isAuthenticated,
          getAccessTokenSilently
        );
        const bidParticipationsListJson: Partial<
          IParticipancyFullDetails
        >[] = await response.json();
        setPaymentList(bidParticipationsListJson);
      }
      setIsDataLoaded(true);
    };
    if (!isLoading) {
      getBid();
    }
  }, [isAuthenticated, isLoading]);

  const changeNumberOfParticipants = React.useCallback(
    (addedNumber: number) => {
      setnumberOfParticipants(numberOfParticipants + addedNumber);
    },
    [numberOfParticipants]
  );

  return !isDataLoaded ? (
    <Stack horizontalAlign={"center"}>
      <Spinner size={SpinnerSize.large} />
    </Stack>
  ) : (
    <Stack horizontalAlign={"center"}>
      <ButtonAppBar />
      <Stack
        horizontal
        horizontalAlign="center"
        tokens={{
          childrenGap: "2rem",
          padding: 10,
        }}
      >
        <Image
          src={bidDetails?.product.image}
          height="30rem"
          width="30rem"
        ></Image>
        <Stack
          tokens={{
            childrenGap: "1rem",
            padding: 10,
          }}
        >
          <Text className="semiBold" variant="xLargePlus">
            {bidDetails?.product.name}
          </Text>
          <ShareProductBar />
          <Separator />
          <Text styles={Styles.priceTextStyles}>
            Maximum Acceptable Price: {bidDetails?.maxPrice}₪
          </Text>
          <Text styles={Styles.subHeaderStyle}>
            Group's expiration date: {getDate(bidDetails?.expirationDate)}
          </Text>
          <Text styles={Styles.subHeaderStyle} variant="large">
            Description
          </Text>
          <Text styles={Styles.descriptionStyle}>
            {bidDetails?.product.description}
          </Text>
          <Separator />
          <Stack horizontal verticalAlign="center">
            <FontIcon
              iconName="AddGroup"
              className={Styles.classNames.greenYellow}
            />
            <Text styles={Styles.amoutTextStyles}>
              {numberOfParticipants} Requested items
            </Text>
          </Stack>
          {bidDetails?.phase === Phase.Join ? (
            <>
              <JoinTheGroupButton
                handleClickOpen={handleClickOpen}
                changeNumberOfParticipants={changeNumberOfParticipants}
                setIsJoinTheGroupButtomClicked={setIsJoinTheGroupButtomClicked}
                isJoinTheGroupButtomClicked={isJoinTheGroupButtomClicked}
              />
              <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
              >
                <DialogContent>
                  <JoinTheGroupForm
                    handleClose={handleClose}
                    changeNumberOfParticipants={changeNumberOfParticipants}
                    setIsJoinTheGroupButtomClicked={
                      setIsJoinTheGroupButtomClicked
                    }
                  />
                </DialogContent>
              </Dialog>
            </>
          ) : (
            bidDetails?.phase == Phase.Vote && (
              <Text styles={Styles.newBuyersCantJoinTheGroup}>
                New Buyers can't Join the group, The Expiration date set by the
                group's creator has reached.
              </Text>
            )
          )}
          <Separator />
        </Stack>
      </Stack>
      <Stack horizontalAlign="center">
        <Separator />
        {bidDetails?.phase == Phase.Payment ? (
          <PaymentsTable
            isChosenSupplier={isChosenSupplier}
            participancyList={
              paymentList as Partial<IParticipancyFullDetails>[]
            }
          />
        ) : (
          <SuppliersSection
            supplierProposalRequestList={supplierProposalRequestList}
            numberOfParticipants={numberOfParticipants as number}
            bidPhase={bidDetails?.phase as number}
            isUserInBid={bidDetails?.isUserInBid}
            hasVoted={bidDetails?.hasVoted}
          />
        )}
      </Stack>
    </Stack>
  );
};
