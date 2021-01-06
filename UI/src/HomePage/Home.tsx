import React from "react"; // importing FunctionComponent
import {
  Stack,
  SearchBox,
  DefaultButton,
  IImageProps,
  ImageFit,
  Image,
} from "@fluentui/react";
import { useHistory, useLocation } from "react-router";

import { NavigationPane } from "./NavigationPane/NavigationPane";
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

  const currentSearchParams: URLSearchParams = new URLSearchParams(
    useLocation().search
  );

  // The home component is also been used for the categories and subCategories view
  const isHomePage: boolean = window.location.pathname === "/";

  React.useEffect(() => {
    setTimeout(() => {
      setShowWelcomeBanner(false);
    }, 10000);
  });

  const onSearchBoxEnterPressed = (newValue: any) => {
    const url: URL = new URL("/groups", window.location.origin);
    const newUrlParams = new URLSearchParams();
    newUrlParams.set("search", newValue);

    const currentSearchParams: URLSearchParams = new URLSearchParams(
      window.location.search
    );

    if (currentSearchParams.get("category")) {
      newUrlParams.set("category", currentSearchParams.get("category")!);
    }

    if (currentSearchParams.get("subCategory")) {
      newUrlParams.set("subCategory", currentSearchParams.get("subCategory")!);
    }

    window.location.href = url.href + "?" + newUrlParams.toString();
  };

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
              onClick={() => {
                history.push("/createNewGroup");
              }}
              iconProps={{
                iconName: "Add",
                styles: { root: { color: "darkgrey", marginRight: "-0.6rem" } },
              }}
              styles={defaultButtonStyles}
            ></DefaultButton>
            <SearchBox
              styles={searchBoxStyles}
              placeholder="Search for group"
              onSearch={onSearchBoxEnterPressed}
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
