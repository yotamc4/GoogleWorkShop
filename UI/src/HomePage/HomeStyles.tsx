import {
  IButtonStyles,
  IIconStyles,
  ISearchBoxStyles,
  IStackTokens,
} from "@fluentui/react";

export const searchBoxStyles: Partial<ISearchBoxStyles> = {
  root: { height: "2.6rem", width: "35rem" },
};

export const marginForBothSides: Partial<ISearchBoxStyles> = {
  root: { marginLeft: "13rem", marginRight: "13rem" },
};
export const verticalGapStackTokens: IStackTokens = {
  childrenGap: 20,
};

export const defaultButtonStyles: IButtonStyles = {
  root: {
    borderRadius: 25,
    height: "2.5rem",
    marginRight: "15rem",
    minWidth: "12rem",
  },
  textContainer: {
    fontSize: "1rem",
  },
};

export const genericGapStackTokens: (gap: number) => IStackTokens = (gap) => ({
  childrenGap: gap,
});
