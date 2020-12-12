import { Stack } from "@fluentui/react";
import * as React from "react";
import { ProductCard } from "../../HomePage/ProductCard/ProductCard";
import { ProductDetails } from "../../Modal/ProductDetails";
import { genericGapStackTokens } from "./ProductCardGridStyles";

export const ProductCardGrid: React.FunctionComponent<ProductCardGridProps> = ({
  productDetailsList,
}) => {
  return (
    <Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[0])} />
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[1])} />
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[2])} />
      </Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[3])} />
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[4])} />
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[5])} />
      </Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[6])} />
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[7])} />
        <ProductCard {...returnProductCardOrUndefined(productDetailsList[8])} />
      </Stack>
    </Stack>
  );
};

interface ProductCardGridProps {
  productDetailsList: ProductDetails[];
}
function returnProductCardOrUndefined(productDetails: ProductDetails) {
  return productDetails ?? undefined;
}
