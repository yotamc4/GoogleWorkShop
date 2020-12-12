import React from "react"; // importing FunctionComponent
import * as mockProducts from "../Modal/MockProducts";
import {
  IStackTokens,
  Stack,
  SearchBox,
  ISearchBoxStyles,
  DefaultButton,
  IImageProps,
  ImageFit,
  Image,
} from "@fluentui/react";

import { NavigationPane } from "./NavigationPane/NavigationPane";
import { useHistory } from "react-router";
import { ProductCardGridPages } from "../Components/ProductCardGrid/ProductCardGridPages";
import { AuthContextProvider } from "../Context/AuthContext";

const imagePropsSubLogo: IImageProps = {
  src: "/Images/subLogo2.PNG",
  imageFit: ImageFit.cover,
};

export const Home: React.FunctionComponent = () => {
  const genericGapStackTokens: (gap: number) => IStackTokens = (gap) => ({
    childrenGap: gap,
  });
  const history = useHistory();

  const changeHistory = () => {
    history.push("/createNewGroup");
  };

  const verticalGapStackTokens: IStackTokens = {
    childrenGap: 20,
  };
  const searchBoxStyles: Partial<ISearchBoxStyles> = {
    root: { height: "2.6rem", width: "40rem", marginRight: "10rem" },
  };

  return (
    <AuthContextProvider>
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
              onClick={changeHistory}
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
            <SearchBox
              styles={searchBoxStyles}
              placeholder="Search for group"
            />
          </Stack>
          <Stack horizontal horizontalAlign="center">
            <Stack tokens={{ childrenGap: 5 }}>
              <NavigationPane />
            </Stack>
            <ProductCardGridPages />
          </Stack>
        </Stack>
      </Stack>
    </AuthContextProvider>
  );
};
