import { FontWeights, IDropdownStyles, ITextStyles } from "@fluentui/react";

export const headerStyle: ITextStyles = {
  root: { fontWeight: FontWeights.semibold, padding: 10 },
};

export const dropdownStyles: Partial<IDropdownStyles> = {
  dropdown: { width: 300 },
};

export const verticalGapStackTokens = {
  childrenGap: 10,
  padding: 10,
};

export const horizontalGapStackTokens = {
  childrenGap: 120,
  padding: 5,
};
