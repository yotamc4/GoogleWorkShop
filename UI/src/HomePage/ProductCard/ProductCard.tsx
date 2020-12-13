import React from "react"; // importing FunctionComponent
import { Card, ICardTokens, ICardSectionTokens } from "@uifabric/react-cards";
import { Text, IImageProps, ImageFit, Image } from "office-ui-fabric-react";
import { useHistory } from "react-router-dom";
import { ProductDetails } from "../../Modal/ProductDetails";
import {
  amoutTextStyles,
  cardStyles,
  descriptionTextStyles,
  nameOfProductTextStyles,
  priceTextStyles,
  divStyles,
} from "./ProductCardStyles";

export const ProductCard: React.FunctionComponent<ProductDetails> = (
  productDetails
) => {
  const history = useHistory();
  const cardTokens: ICardTokens = { childrenMargin: 12 };
  const agendaCardSectionTokens: ICardSectionTokens = { childrenGap: 0 };
  const attendantsCardSectionTokens: ICardSectionTokens = { childrenGap: 6 };
  const imageProps: IImageProps = {
    src: productDetails.imageUrl,
    imageFit: ImageFit.contain,
  };
  const changeHistory = () => {
    history.push(`/products/${productDetails.mockId}`);
  };

  return (
    <Card tokens={cardTokens} styles={cardStyles} onClick={changeHistory}>
      <Circle />
      <Card.Section fill horizontalAlign="center" horizontal>
        <Image {...imageProps} width={300} height={200} />
      </Card.Section>
      <Card.Section
        horizontalAlign="center"
        styles={{ root: { flexBasis: "10rem" } }}
      >
        <Text variant="large" styles={nameOfProductTextStyles}>
          {productDetails.name}
        </Text>
        <Text styles={descriptionTextStyles}>
          {productDetails.description.slice(0, 200) + "..."}
        </Text>
      </Card.Section>
      <Card.Section horizontalAlign="center" tokens={agendaCardSectionTokens}>
        <Text variant="mediumPlus" styles={priceTextStyles}>
          Max Acceptable Price: {productDetails.maximumAcceptablePrice}₪
        </Text>
        <Text variant="small" styles={descriptionTextStyles}>
          Expiration Date:{" "}
          {productDetails.groupExpirationDate.getUTCMonth() + 1}/
          {productDetails.groupExpirationDate.getUTCDate() + 1}/
          {productDetails.groupExpirationDate.getUTCFullYear()}
        </Text>
      </Card.Section>
      <Card.Section
        horizontalAlign="center"
        horizontal
        tokens={attendantsCardSectionTokens}
      >
        <Text variant="small" styles={amoutTextStyles}>
          {17} Suplliers proposals
        </Text>
        <Text variant="small" styles={amoutTextStyles}>
          |
        </Text>
        <Text variant="small" styles={amoutTextStyles}>
          {118} Requested items
        </Text>
      </Card.Section>
    </Card>
  );
};

const Circle: React.FunctionComponent = () => {
  return <div style={divStyles}>New</div>;
};
