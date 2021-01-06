import { IStackTokens } from "@fluentui/react";

export const genericGapStackTokens: (gap: number) => IStackTokens = (gap) => ({
  childrenGap: gap,
});

export const genericGapStackTokensString: (gap: string) => IStackTokens = (
  gap
) => ({
  childrenGap: gap,
});
