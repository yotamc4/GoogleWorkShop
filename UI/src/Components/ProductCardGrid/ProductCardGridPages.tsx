import * as React from "react";
import { Pagination } from "@material-ui/lab";

import * as BidsControllerService from "../../Services/BidsControllerService";
import { ProductCardGrid } from "./ProductCardGrid";
import { Spinner, Stack, StackItem } from "@fluentui/react";
import { genericGapStackTokens } from "./ProductCardGridStyles";
import { Bid } from "../../Modal/GroupDetails";

export const ProductCardGridPages: React.FunctionComponent<ProductCardGridPagesPros> = (
  props
) => {
  const [currentPageNumber, setCurrentPageNumber] = React.useState<number>(1);
  const [currentBids, setCurrentBids] = React.useState<Bid[]>();
  const [numberOfPages, setNumberOfPages] = React.useState<number>(1);
  const category = React.useRef<string | null>(null);
  const subCategory = React.useRef<string | null>(null);
  const searchString = React.useRef<string | null>(null);

  const searchParams = new URLSearchParams(window.location.search);

  category.current = searchParams.get("category");
  subCategory.current = searchParams.get("subCategory");
  searchString.current = searchParams.get("search");

  async function updateCurrentProductAndPageNumber() {
    const getBidsResponse: BidsControllerService.GetBidsResponse = await BidsControllerService.getBids(
      currentPageNumber - 1,
      category.current,
      subCategory.current,
      searchString.current
    );

    if (getBidsResponse.numberOfPages != numberOfPages) {
      setNumberOfPages(getBidsResponse.numberOfPages);
    }

    setCurrentBids(getBidsResponse.bidsPage);
  }

  React.useEffect(() => {
    updateCurrentProductAndPageNumber();
  }, [
    currentPageNumber,
    category.current,
    subCategory.current,
    searchString.current,
  ]);

  return (
    <Stack tokens={genericGapStackTokens(20)}>
      {currentBids ? (
        <ProductCardGrid {...{ bids: currentBids }} />
      ) : (
        <Stack grow verticalAlign="center">
          <Spinner />
        </Stack>
      )}
      <Stack horizontalAlign={"center"}>
        {currentBids && currentBids.length !== 0 && (
          <Pagination
            count={numberOfPages}
            page={currentPageNumber}
            onChange={(event, page) => {
              setCurrentPageNumber(page);
            }}
          />
        )}
      </Stack>
    </Stack>
  );
};

interface ProductCardGridPagesPros {
  Category?: string;
  SubCategory?: string;
}
