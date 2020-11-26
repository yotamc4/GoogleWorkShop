import React from "react"; // importing FunctionComponent
import * as mockProducts from "../Modal/MockProducts";
import {
  IStackTokens,
  Stack,
  SearchBox,
  ISearchBoxStyles,
  DefaultButton,
  IStackStyles,
} from "@fluentui/react";
import { ProductCard } from "./ProductCard/ProductCard";
import { NavigationPane } from "./NavigationPane/NavigationPane";

export const Home: React.FunctionComponent = () => {
  const genericGapStackTokens: (gap: number) => IStackTokens = (gap) => ({
    childrenGap: gap,
  });

  const verticalGapStackTokens: IStackTokens = {
    padding: 20,
    childrenGap: 20,
  };
  const searchBoxStyles: Partial<ISearchBoxStyles> = {
    root: { height: "2.6rem", width: "25rem", alignSelf: "flex-end" },
  };

  return (
    <Stack tokens={verticalGapStackTokens}>
      <Stack
        horizontal
        horizontalAlign="center"
        tokens={genericGapStackTokens(60)}
      >
        <Stack tokens={{ childrenGap: 5 }}>
          <NavigationPane />
        </Stack>
        <Stack tokens={genericGapStackTokens(20)}>
          <Stack
            horizontal
            horizontalAlign="center"
            tokens={genericGapStackTokens(120)}
          >
            <DefaultButton
              text={"Create a new group-buy"}
              primary
              href={"/createNewGroup"}
              iconProps={{
                iconName: "Add",
                styles: { root: { color: "darkgrey" } },
              }}
              styles={{
                root: { borderRadius: 25, height: "2.5rem" },
                textContainer: { padding: "1rem", fontSize: "1.5rem" },
              }}
            ></DefaultButton>
            <SearchBox
              styles={searchBoxStyles}
              placeholder="Search for product group"
            />
          </Stack>
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
  );
};
