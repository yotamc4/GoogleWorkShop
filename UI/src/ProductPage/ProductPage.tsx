import React, { useState } from "react";
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
import { BidDetails } from "../Modal/ProductDetails";
import { useParams } from "react-router-dom";
import { PaymentsTable } from "../PaymentTable/PaymentTable";
import { ISupplierProposalRequest } from "./Suppliers/SupplierSection.interface";
import { ShareProductBar } from "./ShareProductBar";

export const ProductPage: React.FunctionComponent = () => {
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
    axios
      .all([
        axios.get(`https://localhost:5001/api/v1/bids/${id}`),
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
  }, []);

  //cunsome from the server needs to know that about the user
  const [isJoinButtonCliked, setisJoinButtonCliked] = React.useState<number>(0);

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
              {numberOfParticipants} pepole have joined to the group
            </Text>
          </Stack>
          {new Date().getTime() <
          (new Date(
            bidDetails?.expirationDate as string
          ).getUTCMonth() as number) ? (
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
            />
          ) : (
            <Text styles={Styles.newBuyersCantJoinTheGroup}>
              New Buyers can't Join the group, The Expirtaion date setted by the
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
