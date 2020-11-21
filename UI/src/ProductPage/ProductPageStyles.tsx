import {
  FontWeights,
  IDropdownStyles,
  ITextStyles,
  mergeStyles,
  mergeStyleSets,
} from "@fluentui/react";

export const inputWidth: string = "100%";

export const headerStyle: ITextStyles = {
  root: { fontWeight: FontWeights.regular },
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

export const classNames = mergeStyleSets({
  greenYellow: [{ color: "greenyellow" }, iconClass],
});
