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
import { useHistory, useLocation } from "react-router";
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
  const [showWelcomeBanner, setShowWelcomeBanner] = React.useState<boolean>(
    true
  );

  const history = useHistory();
  const location = useLocation();

  //The home component is also been used for the categories and subCategories view
  const isHomePage: boolean = location.pathname === "/";
  const changeHistory = () => {
    history.push("/createNewGroup");
  };

  React.useEffect(() => {
    setTimeout(() => {
      setShowWelcomeBanner(false);
    }, 15000);
  });

  return (
    <AuthContextProvider>
      <Stack tokens={verticalGapStackTokens}>
        <Stack horizontal horizontalAlign="center">
          {isHomePage && showWelcomeBanner && (
            <Image {...imagePropsSubLogo} width="71rem" height="20rem" />
          )}
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
          <Stack
            horizontal
            horizontalAlign="center"
            tokens={{ childrenGap: "2rem" }}
          >
            <Stack tokens={{ childrenGap: "5rem" }}>
              <NavigationPane />
            </Stack>
            <ProductCardGridPages />
          </Stack>
        </Stack>
      </Stack>
    </AuthContextProvider>
  );
};
