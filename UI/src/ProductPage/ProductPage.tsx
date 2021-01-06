import React, { useCallback, useMemo, useState } from "react";
import axios from "axios";
import * as Styles from "./ProductPageStyles";
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

export const ProductPage: React.FunctionComponent = () => {
  const { isAuthenticated, user, getAccessTokenSilently } = useAuth0();
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

  const { id } = useParams<{ id: string }>();
  React.useEffect(() => {
    let role: string;
    if (isAuthenticated) {
      role = user[configData.roleIdentifier];
    } else {
      role = "Anonymous";
    }
    axios
      .all([
        axios.get(
          `https://localhost:5001/api/v1/bids/${id}?role=${role}&id=${user?.sub}`
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
              {numberOfParticipants} people have joined to the group
            </Text>
          </Stack>
          {bidDetails?.phase === Phase.Join ? (
            <JoinTheGroupButton
              isUserInBid={bidDetails.isUserInBid}
              changeNumberOfParticipants={changeNumberOfParticipants}
            />
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
            bidPhase={bidDetails?.phase as number}
            isUserInBid={bidDetails?.isUserInBid}
          />
        )}
      </Stack>
    </Stack>
  );
};
