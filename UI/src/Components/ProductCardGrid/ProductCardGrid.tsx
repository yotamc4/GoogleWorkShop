import { Stack } from "@fluentui/react";
import * as React from "react";
import { ProductCard } from "../../HomePage/ProductCard/ProductCard";
import { Bid } from "../../Modal/GroupDetails";
import { genericGapStackTokens } from "./ProductCardGridStyles";

export const ProductCardGrid: React.FunctionComponent<ProductCardGridProps> = ({
  bids,
}) => {
  return (
    <Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...returnProductCardOrUndefined(bids[0])} />
        <ProductCard {...returnProductCardOrUndefined(bids[1])} />
        <ProductCard {...returnProductCardOrUndefined(bids[2])} />
      </Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...returnProductCardOrUndefined(bids[3])} />
        <ProductCard {...returnProductCardOrUndefined(bids[4])} />
        <ProductCard {...returnProductCardOrUndefined(bids[5])} />
      </Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...returnProductCardOrUndefined(bids[6])} />
        <ProductCard {...returnProductCardOrUndefined(bids[7])} />
        <ProductCard {...returnProductCardOrUndefined(bids[8])} />
      </Stack>
    </Stack>
  );
};

interface ProductCardGridProps {
  bids: Bid[];
}
function returnProductCardOrUndefined(bid: Bid) {
  return bid ?? undefined;
}
