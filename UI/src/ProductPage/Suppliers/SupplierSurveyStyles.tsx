import {
  FontWeights,
  IButtonStyles,
  IChoiceGroupStyles,
  IStackTokens,
  ITextStyles,
} from "@fluentui/react";

export const choiceGroupStyles: IChoiceGroupStyles = {
  root: {
    marginLeft: "-22rem",
  },
};

export const textStyles: ITextStyles = {
  root: {
    marginLeft: "-22rem",
  },
};

export const verticalGapStackTokens: IStackTokens = {
  childrenGap: 30,
};

export const defaultButtonVoteStyles: IButtonStyles = {
  root: {
    borderRadius: 25,
    height: "2.5rem",
    width: "20rem",
    marginLeft: "-10rem",
  },
  textContainer: { padding: "1rem", fontSize: "1.5rem" },
};

export const thankForYourVote: ITextStyles = {
  root: {
    fontSize: "1.2rem",
    marginTop: "1rem",
    color: "#8b002a",
    fontWeight: FontWeights.semibold,
    paddingBottom:"5rem"
  },
};
