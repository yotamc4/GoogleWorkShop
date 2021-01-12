import React from "react"; // importing FunctionComponent
import { Card, ICardTokens, ICardSectionTokens } from "@uifabric/react-cards";
import { Text, IImageProps, ImageFit, Image } from "office-ui-fabric-react";
import { useHistory } from "react-router-dom";
import {
  amoutTextStyles,
  cardStyles,
  descriptionTextStyles,
  nameOfProductTextStyles,
  priceTextStyles,
  circleStyle,
} from "./ProductCardStyles";
import { Bid } from "../../Modal/GroupDetails";
import { Stack, StackItem } from "@fluentui/react";

export const ProductCard: React.FunctionComponent<Bid> = (bid) => {
  function isNewBid(creationDate: Date): boolean {
    const newD: Date = new Date(
      creationDate.getFullYear(),
      creationDate.getMonth(),
      creationDate.getDate() + 2
    );
    return (
      // Bid's creation date plus 2 days
      !(
        new Date(
          creationDate.getFullYear(),
          creationDate.getMonth(),
          creationDate.getDate() + 2
        ) < new Date()
      )
    );
  }

  const history = useHistory();
  const attendantsCardSectionTokens: ICardSectionTokens = { childrenGap: 6 };

  if (bid.id == undefined) {
    return <div />;
  }

  const imageProps: IImageProps = {
    src: bid.product?.image,
    imageFit: ImageFit.contain,
  };

  return (
    <StackItem
      styles={{
        root: {
          flexBasis: "32%",
          marginBottom: "1rem",
          boxShadow:
            "rgba(0, 0, 0, 0.133) 0px 1.6px 3.6px 0px, rgba(0, 0, 0, 0.11) 0px 0.3px 0.9px 0px",
        },
      }}
    >
      <Stack
        styles={cardStyles}
        onClick={() => {
          history.push(`/products/${bid.id}`);
        }}
      >
        <Stack>
          {bid.id !== undefined && isNewBid(bid.creationDate) && (
            <NewTagCircle />
          )}
          <StackItem align="center">
            <Image
              {...imageProps}
              width={"14rem"}
              height={"10rem"}
              styles={{ root: { marginTop: "1rem" } }}
            />
          </StackItem>
        </Stack>
        <Stack
          horizontalAlign="center"
          styles={{ root: { padding: "0.5rem" } }}
        >
          <Text variant="large" styles={nameOfProductTextStyles}>
            {bid.product!.name.length > 30
              ? bid.product!.name.slice(0, 30) + "..."
              : bid.product!.name}
          </Text>
          <Text styles={descriptionTextStyles}>
            {bid.product!.description.length > 199
              ? bid.product!.description.slice(0, 200) + "..."
              : bid.product!.description}
          </Text>
        </Stack>
        <Stack horizontalAlign="center">
          <Text variant="mediumPlus" styles={priceTextStyles}>
            Max Acceptable Price: {bid.maxPrice}₪
          </Text>
          {bid.expirationDate && (
            <Text
              variant="small"
              styles={{
                root: {
                  color: "#666666",
                },
              }}
            >
              Last Day To Join: {bid.expirationDate.getUTCMonth() + 1}/
              {bid.expirationDate.getUTCDate()}/
              {bid.expirationDate.getUTCFullYear()}
            </Text>
          )}
        </Stack>
        <Stack
          horizontalAlign="space-around"
          horizontal
          styles={{ root: { padding: "0.5rem" } }}
          wrap
        >
          <Text variant="small" styles={amoutTextStyles}>
            {bid.potenialSuplliersCounter} Suppliers proposals
          </Text>
          <Text variant="small" styles={amoutTextStyles}>
            {bid.unitsCounter} Requested items
          </Text>
        </Stack>
      </Stack>
    </StackItem>
  );
};

const NewTagCircle: React.FunctionComponent = () => {
  return <div style={circleStyle}>New</div>;
};
