import React from "react"; // importing FunctionComponent
import {
  Stack,
  SearchBox,
  DefaultButton,
  IImageProps,
  ImageFit,
  Image,
} from "@fluentui/react";

import { NavigationPane } from "./NavigationPane/NavigationPane";
import { useHistory } from "react-router";
import { ProductCardGridPages } from "../Components/ProductCardGrid/ProductCardGridPages";
import { AuthContextProvider } from "../Context/AuthContext";
import {
  defaultButtonStyles,
  genericGapStackTokens,
  searchBoxStyles,
  verticalGapStackTokens,
} from "./HomeStyles";

const imagePropsSubLogo: IImageProps = {
  src: "/Images/subLogo2.PNG",
  imageFit: ImageFit.cover,
};

export const Home: React.FunctionComponent = () => {
  const history = useHistory();

  const changeHistory = () => {
    history.push("/createNewGroup");
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
              styles={defaultButtonStyles}
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
