import * as React from "react";
import { Pagination } from "@material-ui/lab";

import * as BidsControllerService from "../../Services/BidsControllerService";
import { ArrayOfMockProducts } from "../../Modal/MockProducts";
import { ProductDetails } from "../../Modal/ProductDetails";
import { ProductCardGrid } from "./ProductCardGrid";
import { Spinner, Stack } from "@fluentui/react";
import { genericGapStackTokens } from "./ProductCardGridStyles";
import { Bid } from "../../Modal/GroupDetails";

export const ProductCardGridPages: React.FunctionComponent<ProductCardGridPagesPros> = React.memo(
  (props) => {
    const [currentPageNumber, setCurrentPageNumber] = React.useState<number>(1);
    const [currentBids, setCurrentBids] = React.useState<Bid[]>();

    //TODO - We need to fetch the number of Pages from the BEPagination
    const [numberOfPages, setNumberOfPages] = React.useState<number>(10);

    async function updateCurrentProductAndPageNumber() {
      const getBidsResponse: BidsControllerService.GetBidsResponse = await BidsControllerService.getBids(
        currentPageNumber - 1
      );

      if (getBidsResponse.numberOfPages != numberOfPages) {
        setNumberOfPages(getBidsResponse.numberOfPages);
      }

      setCurrentBids(getBidsResponse.bidsPage);
    }

    React.useEffect(() => {
      updateCurrentProductAndPageNumber();
    }, [currentPageNumber]);

    return (
      <Stack horizontalAlign="center" tokens={genericGapStackTokens(20)}>
        {currentBids ? (
          <ProductCardGrid {...{ bids: currentBids }} />
        ) : (
          <Spinner />
        )}
        <Pagination
          count={numberOfPages}
          page={currentPageNumber}
          onChange={(event, page) => {
            setCurrentPageNumber(page);
          }}
        />
      </Stack>
    );
  }
);

interface ProductCardGridPagesPros {
  Category?: string;
  SubCategory?: string;
}

// TODO - We need to implement a function that fetch from the BE the matching product according
// To the function's arguments
async function getCurrentProducts(
  pageNumber: number,
  category?: string,
  subCategory?: string
): Promise<ProductDetails[]> {
  return ArrayOfMockProducts;
}
