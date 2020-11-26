import React from "react";
import * as Styles from "./ProductPageStyles";
import {
  DefaultButton,
  FontIcon,
  Icon,
  Image,
  Separator,
  Stack,
  Text,
} from "@fluentui/react";
import { SuppliersList } from "./SupplierList";
import { ProductDetails } from "../Modal/ProductDeatils";
import { LenovoThinkPadProduct } from "../Modal/MockProducts";
import { GroupDetails } from "../Modal/GroupDetails";

export const ProductPage: React.FunctionComponent = () => {
  const [productDetails, setProductDetails] = React.useState<ProductDetails>(
    LenovoThinkPadProduct
  );

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
          <Text
            className="Bold"
            styles={Styles.headerStyle}
            variant="xLargePlus"
          >
            {productDetails.name}
          </Text>
          <Separator />
          <Text styles={Styles.subHeaderStyle}>
            Maximum Acceptable Price: {productDetails.maximumAcceptablePrice}₪
          </Text>
          <Text styles={Styles.subHeaderStyle}>
            Group's expiration date: {productDetails.groupExpirationDate}
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
            <Text>
              {groupDetails.numberOfParticipants} pepole have joined to the
              group so far
            </Text>
          </Stack>
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
        </Stack>
      </Stack>
      <Stack horizontal horizontalAlign="center">
        <SuppliersList />
      </Stack>
    </Stack>
  );
};
