import * as React from "react";
import { Pagination } from "@material-ui/lab";

import * as BidsControllerService from "../../Services/BidsControllerService";
import { ProductCardGrid } from "./ProductCardGrid";
import { Spinner, Stack } from "@fluentui/react";
import { genericGapStackTokens } from "./ProductCardGridStyles";
import { Bid } from "../../Modal/GroupDetails";
import { useLocation, useParams } from "react-router-dom";

export const ProductCardGridPages: React.FunctionComponent<ProductCardGridPagesPros> = React.memo(
  (props) => {
    const [currentPageNumber, setCurrentPageNumber] = React.useState<number>(1);
    const [currentBids, setCurrentBids] = React.useState<Bid[]>();
    const [numberOfPages, setNumberOfPages] = React.useState<number>(1);
    const category = React.useRef<string | null>(null);
    const subCategory = React.useRef<string | null>(null);

    const searchParams = new URLSearchParams(useLocation().search);
    category.current = searchParams.get("category");
    subCategory.current = searchParams.get("subCategory");

    async function updateCurrentProductAndPageNumber() {
      const getBidsResponse: BidsControllerService.GetBidsResponse = await BidsControllerService.getBids(
        currentPageNumber - 1,
        category.current,
        subCategory.current
      );

      if (getBidsResponse.numberOfPages != numberOfPages) {
        setNumberOfPages(getBidsResponse.numberOfPages);
      }

      setCurrentBids(getBidsResponse.bidsPage);
    }

    React.useEffect(() => {
      updateCurrentProductAndPageNumber();
    }, [currentPageNumber, category.current, subCategory.current]);

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
