import { IStackTokens } from "@fluentui/react";

export const genericGapStackTokens: (gap: number) => IStackTokens = (gap) => ({
  childrenGap: gap,
});
