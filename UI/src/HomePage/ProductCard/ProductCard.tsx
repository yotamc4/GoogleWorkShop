import React from 'react'; // importing FunctionComponent
import { Card, ICardTokens, ICardSectionTokens, ICardStyles } from "@uifabric/react-cards";
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
} from 'office-ui-fabric-react';

type ProductCardProps = {
  image: string,
  category: string,
  nameOfProduct: string,
  maxPrice: number,
  creationDate: string,
  expirationDate: string,
  description: string,
  potenialSuplliersCounter: number,
  unitsCounter: number,   
}

const theme: ITheme =  getTheme();

export const ProductCard: React.FunctionComponent<ProductCardProps> = ({ 
  image,
  category,
  nameOfProduct,
  maxPrice,
  creationDate,
  expirationDate,
  description,
  potenialSuplliersCounter,
  unitsCounter
 }) => {
  
  const cardTokens: ICardTokens = { childrenMargin: 12 };
  const agendaCardSectionTokens: ICardSectionTokens = { childrenGap: 0 };
  const attendantsCardSectionTokens: ICardSectionTokens = { childrenGap: 6 };
  const imageProps: IImageProps = {
    src:image,
    imageFit: ImageFit.contain,
  };
  initializeIcons();
    return ( 
    <Card
      tokens={cardTokens}
      styles={cardStyles}
    >
      <Circle/>
      <Card.Section
        fill
        horizontalAlign="center"
        horizontal
      >
        <Image
        {...imageProps}
        width={300}
        height={200}
        />
      </Card.Section>
      <Card.Section horizontalAlign="center">
        <Text variant="large" styles={nameOfProductTextStyles}>
          {nameOfProduct}
        </Text>
        <Text styles={descriptionTextStyles}>{description}</Text>
      </Card.Section>
      <Card.Section horizontalAlign="center" tokens={agendaCardSectionTokens}>
        <Text variant="mediumPlus" styles={priceTextStyles}>
          Max Price: {maxPrice}₪
        </Text>
        <Text variant="small" styles={descriptionTextStyles}>
          Expiration Date: {expirationDate}
        </Text>
      </Card.Section>
      <Card.Section horizontalAlign="center" horizontal tokens={attendantsCardSectionTokens}>
        <Text variant="small" styles={amoutTextStyles}>
          {potenialSuplliersCounter} Potential Suplliers
        </Text>
        <Text variant="small" styles={amoutTextStyles}>
          |
        </Text>
        <Text variant="small" styles={amoutTextStyles}>
          {unitsCounter} Units counter
        </Text>
      </Card.Section>
    </Card>)

}

const Circle: React.FunctionComponent = () => {
  return(<div style={{width:"36px", height:"36px", lineHeight:"36px", borderRadius:"50%", fontSize:"75%", color:"#fff", textAlign:"center",
      background:theme.palette.blue, position:"absolute", zIndex:100, marginLeft:"10px", marginTop:"20px", fontWeight:600,
       }}>New</div>)
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
    color: '#333333',
    fontWeight: FontWeights.semibold
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
    color: '#666666',
  },
};

const cardStyles: ICardStyles = {
  root: {
    selectors: {
      ':hover': {
        cursor: "pointer",
      },
    }
  },
};