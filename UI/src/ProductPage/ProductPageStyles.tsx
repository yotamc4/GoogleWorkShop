import {
  FontWeights,
  getTheme,
  IDropdownStyles,
  ITextStyles,
  ITheme,
  mergeStyles,
  mergeStyleSets,
} from "@fluentui/react";

const theme: ITheme = getTheme();

export const inputWidth: string = "100%";

export const headerStyle: ITextStyles = {
  root: { fontWeight: FontWeights.regular },
};

export const subHeaderStyle: ITextStyles = {
  root: { fontWeight: FontWeights.semibold },
};

export const descriptionStyle: ITextStyles = {
  root: { maxWidth: "35rem" },
};

export const dropdownStyles: Partial<IDropdownStyles> = {
  dropdown: { width: inputWidth },
};

export const verticalGapStackTokens = {
  childrenGap: 10000,
  padding: 10,
};

export const horizontalGapStackTokens = {
  childrenGap: "20rem",
  padding: 5,
};

export const imageStyles = {
  childrenGap: "20rem",
  padding: 5,
};

const iconClass = mergeStyles({
  fontSize: 50,
  height: 50,
  width: 50,
  margin: "0 25px",
});

export const priceTextStyles: ITextStyles = {
  root: {
    color: theme.palette.red,
    fontWeight: FontWeights.semibold,
    paddingRight: "8rem",
  },
};

export const amoutTextStyles: ITextStyles = {
  root: {
    color: theme.palette.blue,
    fontWeight: FontWeights.semibold,
  },
};

export const classNames = mergeStyleSets({
  greenYellow: [{ color: "#000c8b" }, iconClass],
});

export const newBuyersCantJoinTheGroup: ITextStyles = {
  root: {
    fontSize: "1.2rem",
    marginTop: "1rem",
    color: "#8b002a",
    fontWeight: FontWeights.semibold,
  },
};
