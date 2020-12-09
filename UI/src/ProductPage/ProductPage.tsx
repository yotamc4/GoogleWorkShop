import React from "react";
import * as Styles from "./ProductPageStyles";
import * as mockProducts from "../Modal/MockProducts";
import * as MockBuyers from "../Modal/MockBuyers";
import { useRouteMatch  } from "react-router-dom";
import {
  DefaultButton,
  FontIcon,
  Image,
  Separator,
  Stack,
  Text,
} from "@fluentui/react";
import { SuppliersSection } from "./SupplierSection";
import { ProductDetails } from "../Modal/ProductDeatils";
import { GroupDetails } from "../Modal/GroupDetails";
import { useParams } from "react-router-dom";
import { PaymentsTable } from "../PaymentTable/PaymentTable";

export const ProductPage: React.FunctionComponent<{ mockProductId: number }> = (
  mockProductId
) => {
  //TODO: delete those states not every data from the user should be
  // state because most of the properties from the backend won't change in the productpage
  const { id } = useParams<{ id: string }>();
  const [productDetails, setProductDetails] = React.useState<ProductDetails>(
    getMockProduct(id)
  );
  console.log(useRouteMatch());

  const [groupDetails, setGroupDetails] = React.useState<GroupDetails>({
    numberOfParticipants: 170,
    groupExpirationDate: "",
  });

  return (
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
          src={productDetails.imageUrl}
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
            {productDetails.name}
          </Text>
          <Separator />
          <Text styles={Styles.priceTextStyles}>
            Maximum Acceptable Price: {productDetails.maximumAcceptablePrice}₪
          </Text>
          <Text styles={Styles.subHeaderStyle}>
            Group's expiration date:{" "}
            {productDetails.groupExpirationDate.getUTCMonth() + 1}/
            {productDetails.groupExpirationDate.getUTCDate() + 1}/
            {productDetails.groupExpirationDate.getUTCFullYear()}
          </Text>
          <Text styles={Styles.subHeaderStyle} variant="large">
            Description
          </Text>
          <Text styles={Styles.descriptionStyle}>
            {productDetails.description}
          </Text>
          <Separator />
          <Stack horizontal verticalAlign="center">
            <FontIcon
              iconName="AddGroup"
              className={Styles.classNames.greenYellow}
            />
            <Text styles={Styles.amoutTextStyles}>
              {groupDetails.numberOfParticipants} pepole have joined to the
              group
            </Text>
          </Stack>
          {new Date().getTime() <
          productDetails.groupExpirationDate.getTime() ? (
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
        {productDetails.supplierHasChosen ? (
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
            requestedItems={groupDetails.numberOfParticipants}
            groupExpirationDate={productDetails.groupExpirationDate}
          />
        )}
      </Stack>
    </Stack>
  );
};

function getMockProduct(id: string | undefined): ProductDetails {
  switch (id) {
    case "1":
      return mockProducts.AirPodsProProduct;
    case "2":
      return mockProducts.AppleWatchSeries6GPSProduct;
    case "3":
      return mockProducts.GooglePixelProduct;
    case "4":
      return mockProducts.InokimMini2WhiteProduct;
    case "5":
      return mockProducts.LenovoThinkPadProduct;
    case "6":
      return mockProducts.MicrosoftSurfacePro7Product;
    case "7":
      return mockProducts.PowerbeatsProRedProduct;
    case "8":
      return mockProducts.SamsungUN70TU6980FXZAProduct;
    case "9":
      return mockProducts.SonyPlaystation5DigitalProduct;
    default:
      return mockProducts.XiaomiMiBoxProduct;
  }
}
