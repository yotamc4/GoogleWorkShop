import React from "react"; // importing FunctionComponent
import {
  IStackTokens,
  Stack,
  SearchBox,
  ISearchBoxStyles,
  DefaultButton,
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

  const searchBoxStyles: Partial<ISearchBoxStyles> = { root: { width: 400 } };

  return (
    <Stack tokens={verticalGapStackTokens}>
      <Stack
        horizontal
        horizontalAlign="center"
        tokens={genericGapStackTokens(500)}
      >
        <DefaultButton
          text={"Create new group buy"}
          href={"/createNewGroup"}
          iconProps={{
            iconName: "Add",
            styles: { root: { color: "deepskyblue" } },
          }}
        ></DefaultButton>
        <SearchBox
          styles={searchBoxStyles}
          placeholder="Search"
          underlined={true}
        />
      </Stack>
      <Stack
        horizontal
        horizontalAlign="center"
        tokens={genericGapStackTokens(60)}
      >
        <Stack tokens={{ childrenGap: 5 }}>
          <NavigationPane />
        </Stack>
        <Stack tokens={genericGapStackTokens(20)}>
          <Stack horizontal tokens={genericGapStackTokens(20)}>
            {test}
            {test}
            {test}
          </Stack>
          <Stack horizontal tokens={genericGapStackTokens(20)}>
            {test}
            {test}
            {test}
          </Stack>
          <Stack horizontal tokens={genericGapStackTokens(20)}>
            {test}
            {test}
            {test}
          </Stack>
        </Stack>
      </Stack>
    </Stack>
  );
};

const test = (
  <ProductCard
    image="https://bstore.bezeq.co.il/media/20696/740-2-blue.jpg"
    category="computers"
    nameOfProduct="Lenovo ThinkPad T4800"
    maxPrice={4000}
    creationDate="11/11/2020"
    expirationDate="11/13/2020"
    description="Lenovo ThinkPad T480 is a Windows 10 laptop with a 14.00-inch display that has a resolution of 1920x1080 pixels."
    potenialSuplliersCounter={3}
    unitsCounter={95}
  />
);
