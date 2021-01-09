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
  const [isNewSuggestion, setIsNewSuggestion] = React.useState<boolean>(
    bid.id !== undefined && isNewBid(bid.creationDate)
  );

  function isNewBid(creationDate: Date): boolean {
    return (
      // Bid's creation date plus 2 days
      !(
        new Date(
          bid.creationDate.getFullYear(),
          bid.creationDate.getMonth(),
          bid.creationDate.getDate() + 2
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
      grow={true}
      styles={{ root: { flexBasis: "32%", marginBottom: "1rem" } }}
    >
      <Card
        styles={cardStyles}
        onClick={() => {
          history.push(`/products/${bid.id}`);
        }}
      >
        <Stack>
          {isNewSuggestion && <NewTagCircle />}
          <StackItem align="center">
            <Image {...imageProps} width={"14rem"} height={"10rem"} />
          </StackItem>
        </Stack>
        <Card.Section
          horizontalAlign="center"
          styles={{ root: { padding: "0.5rem" } }}
        >
          <Text variant="large" styles={nameOfProductTextStyles}>
            {bid.product?.name}
          </Text>
          <Text styles={descriptionTextStyles}>
            {bid.product!.description.length > 199
              ? bid.product!.description.slice(0, 200) + "..."
              : bid.product!.description}
          </Text>
        </Card.Section>
        <Card.Section horizontalAlign="center">
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
              Expiration Date: {bid.expirationDate.getUTCMonth() + 1}/
              {bid.expirationDate.getUTCDate() + 1}/
              {bid.expirationDate.getUTCFullYear()}
            </Text>
          )}
        </Card.Section>
        <Stack
          horizontalAlign="space-between"
          horizontal
          styles={{ root: { padding: "0.5rem" } }}
          wrap
        >
          <Text variant="small" styles={amoutTextStyles}>
            {bid.potenialSuplliersCounter} Suppliers proposals
          </Text>
          <Text variant="small" styles={amoutTextStyles}>
            |
          </Text>
          <Text variant="small" styles={amoutTextStyles}>
            {bid.unitsCounter} Requested items
          </Text>
        </Stack>
      </Card>
    </StackItem>
  );
};

const NewTagCircle: React.FunctionComponent = () => {
  return <div style={circleStyle}>New</div>;
};
