import React, { useCallback, useMemo, useState } from "react";
import axios from "axios";
import * as Styles from "./ProductPageStyles";
import * as MockBuyers from "../Modal/MockBuyers";
import {
  DefaultButton,
  FontIcon,
  Image,
  Separator,
  Spinner,
  SpinnerSize,
  Stack,
  Text,
} from "@fluentui/react";
import { SuppliersSection } from "./Suppliers/SupplierSection";
import { BidBuyerJoinRequest, BidDetails } from "../Modal/ProductDetails";
import { useParams } from "react-router-dom";
import { PaymentsTable } from "../PaymentTable/PaymentTable";
import { ISupplierProposalRequest } from "./Suppliers/SupplierSection.interface";
import { ShareProductBar } from "./ShareProductBar";
import { useAuth0 } from "@auth0/auth0-react";

export const ProductPage: React.FunctionComponent = () => {
  const { isAuthenticated, user } = useAuth0();
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
  const [isDataLoaded, setIsDataLoaded] = useState<boolean>(false);
  //TODO: depends on the context if the user has clicked on the buttom before
  const [
    isJoinTheGroupButtomClicked,
    setIsJoinTheGroupButtomClicked,
  ] = useState<boolean>(false);
  const { id } = useParams<{ id: string }>();
  React.useEffect(() => {
    let role: string;
    let idUser: string;
    if (isAuthenticated) {
      role = user["https://UniBuyClient.workshop.com/role"];
      idUser = user.sub;
    } else {
      role = "Anonymous";
      idUser = "";
    }
    axios
      .all([
        axios.get(
          `https://localhost:5001/api/v1/bids/${id}?role=${role}&id=${id}`
        ),
        axios.get(`https://localhost:5001/api/v1/bids/${id}/proposals`),
      ])
      .then(
        axios.spread(
          (bidDetailsResponse, supplierProposalRequestListResponse) => {
            setBidDetails(bidDetailsResponse.data as BidDetails);
            setnumberOfParticipants(bidDetailsResponse.data.unitsCounter);
            setsupplierProposalRequestList(
              supplierProposalRequestListResponse.data as ISupplierProposalRequest[]
            );
            setIsDataLoaded(true);
          }
        )
      );
  }, [isAuthenticated]);

  const onClickJoinTheGroupButton = React.useCallback(() => {
    const bidBuyerJoinRequest: BidBuyerJoinRequest = {
      //TODO: take the buyerId from the context
      buyerId: "OfekDavid123",
      bidId: id,
      items: 1,
    };
    axios
      .post(
        `https://localhost:5001/api/v1/bids/${id}/buyers`,
        bidBuyerJoinRequest
      )
      .then((response) => {
        console.log(response);
      })
      .catch((error) => {
        console.error(error);
      });
    setIsJoinTheGroupButtomClicked(true);
    setnumberOfParticipants(numberOfParticipants + 1);
  }, [numberOfParticipants]);

  const onClickCancelButton = React.useCallback(() => {
    axios
      .delete(
        //TODO: the buyerId should be taken from the context!
        `https://localhost:5001/api/v1/bids/${id}/buyers/OfekDavid123`
      )
      .then((response) => {
        console.log(response);
      })
      .catch((error) => {
        console.error(error);
      });
    setIsJoinTheGroupButtomClicked(false);
    setnumberOfParticipants(numberOfParticipants - 1);
  }, [numberOfParticipants]);

  return !isDataLoaded ? (
    <Stack horizontalAlign={"center"}>
      <Spinner size={SpinnerSize.large} />
    </Stack>
  ) : (
    <Stack horizontalAlign={"center"}>
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
            Group's expiration date:{" "}
            {(new Date(
              bidDetails?.expirationDate as string
            ).getUTCMonth() as number) + 1}
            /
            {(new Date(
              bidDetails?.expirationDate as string
            ).getUTCDate() as number) + 1}
            /
            {
              new Date(
                bidDetails?.expirationDate as string
              ).getUTCFullYear() as number
            }
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
              {numberOfParticipants} people have joined to the group
            </Text>
          </Stack>
          {new Date().getTime() >
          (new Date(
            bidDetails?.expirationDate as string
          ).getUTCMonth() as number) ? (
            isJoinTheGroupButtomClicked ? (
              <DefaultButton
                text="Cancel bid participation"
                primary
                styles={{
                  root: { borderRadius: 25, height: "4rem" },
                  textContainer: { padding: "1rem", fontSize: "1.5rem" },
                }}
                height={"4rem"}
                onClick={onClickCancelButton}
              />
            ) : (
              <DefaultButton
                text="Join The Group"
                primary
                iconProps={{
                  iconName: "AddFriend",
                  styles: { root: { fontSize: "1.5rem" } },
                }}
                styles={{
                  root: { borderRadius: 25, height: "4rem" },
                  textContainer: { padding: "1rem", fontSize: "1.5rem" },
                }}
                height={"4rem"}
                onClick={onClickJoinTheGroupButton}
              />
            )
          ) : (
            <Text styles={Styles.newBuyersCantJoinTheGroup}>
              New Buyers can't Join the group, The Expiration date set by the
              group's creator has reached.
            </Text>
          )}
          <Separator />
        </Stack>
      </Stack>
      <Stack horizontalAlign="center">
        <Separator />
        {false ? (
          <PaymentsTable
            payers={[
              MockBuyers.Adi,
              MockBuyers.Guy,
              MockBuyers.Or,
              MockBuyers.Yam,
            ]}
          />
        ) : (
          <SuppliersSection
            supplierProposalRequestList={supplierProposalRequestList}
            numberOfParticipants={numberOfParticipants as number}
            expirationDate={bidDetails?.expirationDate as string}
          />
        )}
      </Stack>
    </Stack>
  );
};
