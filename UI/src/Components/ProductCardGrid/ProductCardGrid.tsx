import { Stack, StackItem } from "@fluentui/react";
import * as React from "react";
import { ProductCard } from "../../HomePage/ProductCard/ProductCard";
import { Bid } from "../../Modal/GroupDetails";
import { genericGapStackTokens } from "./ProductCardGridStyles";

export const ProductCardGrid: React.FunctionComponent<ProductCardGridProps> = ({
  bids,
}) => {
  if (bids.length === 0) {
    return (
      <Stack verticalAlign="center" horizontalAlign="center">
        No Groups have found.
      </Stack>
    );
  }

  return (
    <Stack horizontal wrap horizontalAlign={"space-between"}>
      <ProductCard {...bids[0]} />
      <ProductCard {...bids[1]} />
      <ProductCard {...bids[2]} />
      <ProductCard {...bids[3]} />
      <ProductCard {...bids[4]} />
      <ProductCard {...bids[5]} />
      <ProductCard {...bids[6]} />
      <ProductCard {...bids[7]} />
      <ProductCard {...bids[8]} />
    </Stack>
  );
};

interface ProductCardGridProps {
  bids: Bid[];
}
