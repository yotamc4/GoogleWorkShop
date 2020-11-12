import React from 'react'; // importing FunctionComponent
import { Card, ICardTokens, ICardSectionStyles, ICardSectionTokens } from '@uifabric/react-cards';
import {
  ActionButton,
  FontWeights,
  IButtonStyles,
  Icon,
  IIconStyles,
  Stack,
  Text,
  ITextStyles,
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

  const descriptionTextStyles: ITextStyles = {
    root: {
      color: '#333333',
      fontWeight: FontWeights.semibold,
    },
  };

  const iconStyles: IIconStyles = {
    root: {
      color: '#0078D4',
      fontSize: 16,
      fontWeight: FontWeights.regular,
    },
  };
  const footerCardSectionStyles: ICardSectionStyles = {
    root: {
      borderTop: '1px solid #F3F2F1',
    },
  };
  const backgroundImageCardSectionStyles: ICardSectionStyles = {
    root: {
      backgroundImage: `url(${image})`,
      backgroundPosition: 'center center',
      backgroundSize: 'cover',
      height: 170,
      marginTop:"7px",
      marginLeft:"7px",
      marginRight:"7px",
      marginBottom:"7px",
    },
  };
  const dateTextStyles: ITextStyles = {
    root: {
      color: '#505050',
      fontWeight: 600,
    },
  };
  const subduedTextStyles: ITextStyles = {
    root: {
      color: '#666666',
    },
  };

  const cardTokens: ICardTokens = { childrenMargin: 12 };
  const footerCardSectionTokens: ICardSectionTokens = { padding: '10px 0px 0px' };
  const backgroundImageCardSectionTokens: ICardSectionTokens = { padding: 12 };
  const agendaCardSectionTokens: ICardSectionTokens = { childrenGap: 0 };
  const attendantsCardSectionTokens: ICardSectionTokens = { childrenGap: 6 };

    return ( 
    <Card
      tokens={cardTokens}
    >
      <Card.Section
        fill
        verticalAlign="end"
        styles={backgroundImageCardSectionStyles}
        tokens={backgroundImageCardSectionTokens}
      >
        <Text variant="superLarge" styles={dateTextStyles}>
        </Text>
      </Card.Section>
      <Card.Section>
        <Text variant="mediumPlus" styles={descriptionTextStyles}>
          {nameOfProduct}
        </Text>
        <Text styles={subduedTextStyles}>{description}</Text>
      </Card.Section>
      <Card.Section tokens={agendaCardSectionTokens}>
        <Text variant="medium" styles={descriptionTextStyles}>
          Max Price: {maxPrice}₪
        </Text>
        <Text variant="small" styles={subduedTextStyles}>
          Expiration Date: {expirationDate}
        </Text>
      </Card.Section>
      <Card.Item grow={1}>
        <span />
      </Card.Item>
      <Card.Section horizontalAlign="center" horizontal tokens={attendantsCardSectionTokens}>
        <Text variant="small" styles={descriptionTextStyles}>
          {potenialSuplliersCounter} Potential Suplliers
        </Text>
        <Text variant="small" styles={descriptionTextStyles}>
          {unitsCounter} Units counter
        </Text>
      </Card.Section>
      <Card.Section  horizontalAlign="center" horizontal styles={footerCardSectionStyles} tokens={footerCardSectionTokens}>
        <Icon iconName="RedEye" styles={iconStyles} />
      </Card.Section>
    </Card>)

}