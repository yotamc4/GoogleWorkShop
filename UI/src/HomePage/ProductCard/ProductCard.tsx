import React from "react"; // importing FunctionComponent
import {
  Card,
  ICardTokens,
  ICardSectionTokens,
  ICardStyles,
} from "@uifabric/react-cards";
import {
  FontWeights,
  Text,
  ITextStyles,
  initializeIcons,
  getTheme,
  IImageProps,
  ImageFit,
  Image,
  ITheme,
} from "office-ui-fabric-react";
import { useHistory } from "react-router-dom";
import { ProductDetails } from "../../Modal/ProductDeatils";

const theme: ITheme = getTheme();

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
    //history.push(`/products/${productDetails.name.replace(/\s/g, "")}`);
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
          Expiration Date: {productDetails.groupExpirationDate}
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
  return (
    <div
      style={{
        width: "36px",
        height: "36px",
        lineHeight: "36px",
        borderRadius: "50%",
        fontSize: "75%",
        color: "#fff",
        textAlign: "center",
        background: theme.palette.blue,
        position: "absolute",
        zIndex: 100,
        marginLeft: "10px",
        marginTop: "20px",
        fontWeight: 600,
      }}
    >
      New
    </div>
  );
};

//Styles for Card component

const amoutTextStyles: ITextStyles = {
  root: {
    color: theme.palette.blue,
    fontWeight: FontWeights.semibold,
  },
};

const nameOfProductTextStyles: ITextStyles = {
  root: {
    color: "#333333",
    fontWeight: FontWeights.semibold,
  },
};

const priceTextStyles: ITextStyles = {
  root: {
    color: theme.palette.red,
    fontWeight: FontWeights.semibold,
  },
};

const descriptionTextStyles: ITextStyles = {
  root: {
    color: "#666666",
  },
};

const cardStyles: ICardStyles = {
  root: {
    selectors: {
      ":hover": {
        cursor: "pointer",
      },
    },
  },
};
