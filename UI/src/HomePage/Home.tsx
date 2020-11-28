import React from "react"; // importing FunctionComponent
import * as mockProducts from "../Modal/MockProducts";
import {
  IStackTokens,
  Stack,
  SearchBox,
  ISearchBoxStyles,
  DefaultButton,
  IStackStyles,
  IImageProps,
  ImageFit,
  StackItem,
  Image,
} from "@fluentui/react";
import { ProductCard } from "./ProductCard/ProductCard";
import { NavigationPane } from "./NavigationPane/NavigationPane";

const StacStyles2: IStackStyles = {
  root: {
    height: "20rem",
    marginBottom: "-0.3rem",
  },
};

const imagePropsSubLogo: IImageProps = {
  src: "/Images/subLogo2.PNG",
  imageFit: ImageFit.cover,
};

export const Home: React.FunctionComponent = () => {
  const genericGapStackTokens: (gap: number) => IStackTokens = (gap) => ({
    childrenGap: gap,
  });

  const verticalGapStackTokens: IStackTokens = {
    childrenGap: 20,
  };
  const searchBoxStyles: Partial<ISearchBoxStyles> = {
    root: { height: "2.6rem", width: "40rem", marginRight: "10rem" },
  };

  return (
    <Stack tokens={verticalGapStackTokens}>
      <Stack horizontal horizontalAlign="center">
        <Image {...imagePropsSubLogo} width="71rem" height="20rem" />
      </Stack>
      <Stack tokens={genericGapStackTokens(20)}>
        <Stack
          horizontal
          horizontalAlign="center"
          tokens={genericGapStackTokens(-200)}
        >
          <DefaultButton
            text={"Create a new group-buy"}
            primary
            href={"/createNewGroup"}
            iconProps={{
              iconName: "Add",
              styles: { root: { color: "darkgrey", marginRight: "-0.6rem" } },
            }}
            styles={{
              root: {
                borderRadius: 25,
                height: "2.5rem",
                marginRight: "15rem",
              },
              textContainer: {
                padding: "1rem",
                fontSize: "1.2rem",
                marginBottom: "0.4rem",
              },
            }}
          ></DefaultButton>
          <SearchBox styles={searchBoxStyles} placeholder="Search for group" />
        </Stack>
        <Stack horizontal horizontalAlign="center">
          <Stack tokens={{ childrenGap: 5 }}>
            <NavigationPane />
          </Stack>
          <Stack>
            <Stack horizontal tokens={genericGapStackTokens(20)}>
              <ProductCard {...mockProducts.AirPodsProProduct} />
              <ProductCard {...mockProducts.AppleWatchSeries6GPSProduct} />
              <ProductCard {...mockProducts.GooglePixelProduct} />
            </Stack>
            <Stack horizontal tokens={genericGapStackTokens(20)}>
              <ProductCard {...mockProducts.InokimMini2WhiteProduct} />
              <ProductCard {...mockProducts.LenovoThinkPadProduct} />
              <ProductCard {...mockProducts.MicrosoftSurfacePro7Product} />
            </Stack>
            <Stack horizontal tokens={genericGapStackTokens(20)}>
              <ProductCard {...mockProducts.PowerbeatsProRedProduct} />
              <ProductCard {...mockProducts.SamsungUN70TU6980FXZAProduct} />
              <ProductCard {...mockProducts.SonyPlaystation5DigitalProduct} />
            </Stack>
          </Stack>
        </Stack>
      </Stack>
    </Stack>
  );
};
