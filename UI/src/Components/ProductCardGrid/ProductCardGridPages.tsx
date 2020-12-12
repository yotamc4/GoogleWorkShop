import * as React from "react";
import { Pagination } from "@material-ui/lab";

import { ArrayOfMockProducts } from "../../Modal/MockProducts";
import { ProductDetails } from "../../Modal/ProductDetails";
import { ProductPage } from "../../ProductPage/ProductPage";
import { ProductCardGrid } from "./ProductCardGrid";
import { Spinner, Stack } from "@fluentui/react";
import { genericGapStackTokens } from "./ProductCardGridStyles";

export const ProductCardGridPages: React.FunctionComponent<ProductCardGridPagesPros> = (
  props
) => {
  const [currentPageNumber, setCurrentPageNumber] = React.useState<number>(1);
  const [currentProducts, setCurrentProducts] = React.useState<
    ProductDetails[]
  >();

  //TODO - We need to fetch the number of Pages from the BEPagination
  const [numberOfPages, setNumberOfPages] = React.useState<number>(10);

  React.useEffect(() => {
    async function updateCurrentProducts() {
      const tempCurrentProducts: ProductDetails[] = await getCurrentProducts(
        currentPageNumber
      );

      setCurrentProducts(tempCurrentProducts);
    }

    updateCurrentProducts();
  }, [currentPageNumber]);

  return (
    <Stack horizontalAlign="center" tokens={genericGapStackTokens(20)}>
      {currentProducts ? (
        <ProductCardGrid {...{ productDetailsList: currentProducts }} />
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
};

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
