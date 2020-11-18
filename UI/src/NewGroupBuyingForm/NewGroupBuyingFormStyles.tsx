import { FontWeights, IDropdownStyles, ITextStyles } from "@fluentui/react";

export const inputWidth: string = "100%";

export const headerStyle: ITextStyles = {
  root: { fontWeight: FontWeights.semibold, padding: 10 },
};

export const dropdownStyles: Partial<IDropdownStyles> = {
  dropdown: { width: inputWidth },
};

export const verticalGapStackTokens = {
  childrenGap: 10,
  padding: 10,
};

export const horizontalGapStackTokens = {
  childrenGap: "20rem",
  padding: 5,
};
