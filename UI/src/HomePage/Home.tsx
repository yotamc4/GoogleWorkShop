import React from "react"; // importing FunctionComponent
import {
  Stack,
  SearchBox,
  DefaultButton,
  IImageProps,
  ImageFit,
  Image,
  Link,
} from "@fluentui/react";
import { useHistory, useLocation } from "react-router";

import { NavigationPane } from "./NavigationPane/NavigationPane";
import { ProductCardGridPages } from "../Components/ProductCardGrid/ProductCardGridPages";
import { AuthContextProvider } from "../Context/AuthContext";
import {
  defaultButtonStyles,
  genericGapStackTokens,
  searchBoxStyles,
  marginForBothSides,
  verticalGapStackTokens,
} from "./HomeStyles";
import ButtonAppBar from "../LoginBar";

const imagePropsSubLogo: IImageProps = {
  src: "/Images/subLogo2.PNG",
  imageFit: ImageFit.cover,
};

export const Home: React.FunctionComponent = () => {
  const history = useHistory();

  const currentSearchParams: URLSearchParams = new URLSearchParams(
    useLocation().search
  );

  // The home component is also been used for the categories and subCategories view
  const isHomePage: boolean = window.location.pathname === "/";

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
      <Stack styles={marginForBothSides}>
        <ButtonAppBar />
        <Stack tokens={verticalGapStackTokens}>
          {isHomePage && (
            <Link href={"/about_us"}>
              <Image {...imagePropsSubLogo} height="13rem" />
            </Link>
          )}
          <Stack tokens={genericGapStackTokens(20)}>
            <Stack horizontal horizontalAlign="space-between">
              <DefaultButton
                text={"New group-buy"}
                primary
                onClick={() => {
                  history.push("/createNewGroup");
                }}
                iconProps={{
                  iconName: "Add",
                  styles: { root: { color: "darkgrey" } },
                }}
                styles={defaultButtonStyles}
              ></DefaultButton>
              <SearchBox
                styles={searchBoxStyles}
                placeholder="Search for group"
                onSearch={onSearchBoxEnterPressed}
              />
            </Stack>
            <Stack horizontal horizontalAlign="space-between">
              <NavigationPane />
              <ProductCardGridPages />
            </Stack>
          </Stack>
        </Stack>
      </Stack>
    </AuthContextProvider>
  );
};
