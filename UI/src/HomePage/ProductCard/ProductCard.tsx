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
  divStyles,
} from "./ProductCardStyles";
import { Bid } from "../../Modal/GroupDetails";

export const ProductCard: React.FunctionComponent<Bid> = (bid) => {
  const history = useHistory();
  const cardTokens: ICardTokens = { childrenMargin: 7 };
  const attendantsCardSectionTokens: ICardSectionTokens = { childrenGap: 6 };

  if (bid.id == undefined) {
    return <div />;
  }

  const imageProps: IImageProps = {
    src: bid.product?.image,
    imageFit: ImageFit.contain,
  };

  return (
    <Card
      tokens={cardTokens}
      styles={cardStyles}
      onClick={() => {
        history.push(`/products/${bid.id}`);
      }}
    >
      <Circle />
      <Card.Section fill horizontalAlign="center" horizontal>
        <Image {...imageProps} width={"18rem"} height={"10rem"} />
      </Card.Section>
      <Card.Section
        horizontalAlign="center"
        styles={{ root: { flexBasis: "10rem" } }}
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
          <Text variant="small" styles={descriptionTextStyles}>
            Expiration Date: {bid.expirationDate.getUTCMonth() + 1}/
            {bid.expirationDate.getUTCDate() + 1}/
            {bid.expirationDate.getUTCFullYear()}
          </Text>
        )}
      </Card.Section>
      <Card.Section
        horizontalAlign="center"
        horizontal
        tokens={attendantsCardSectionTokens}
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
      </Card.Section>
    </Card>
  );
};

const Circle: React.FunctionComponent = () => {
  return <div style={divStyles}>New</div>;
};
