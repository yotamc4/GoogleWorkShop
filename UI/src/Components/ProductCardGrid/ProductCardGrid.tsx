import { Stack } from "@fluentui/react";
import * as React from "react";
import { ProductCard } from "../../HomePage/ProductCard/ProductCard";
import { Bid } from "../../Modal/GroupDetails";
import { genericGapStackTokens } from "./ProductCardGridStyles";

export const ProductCardGrid: React.FunctionComponent<ProductCardGridProps> = ({
  bids,
}) => {
  return (
    <Stack styles={{ root: { minWidth: "60rem" } }}>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...bids[0]} />
        <ProductCard {...bids[1]} />
        <ProductCard {...bids[2]} />
      </Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...bids[3]} />
        <ProductCard {...bids[4]} />
        <ProductCard {...bids[5]} />
      </Stack>
      <Stack horizontal tokens={genericGapStackTokens(20)}>
        <ProductCard {...bids[6]} />
        <ProductCard {...bids[7]} />
        <ProductCard {...bids[8]} />
      </Stack>
    </Stack>
  );
};

interface ProductCardGridProps {
  bids: Bid[];
}
