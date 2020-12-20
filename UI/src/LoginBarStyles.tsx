import { ISeparatorStyles, IStackItemStyles, IStackStyles } from "@fluentui/react";

export const StacStyles: IStackStyles = {
  root: {
    paddingLeft: "0.7rem",
    paddingRight: "0.7rem",
  },
};

export const StacStyles2: IStackStyles = {
  root: {
    paddingTop: "3rem",
  },
};

export const StackItemStyles: IStackItemStyles = {
  root: {
    width: "7rem",
    marginLeft: "32rem",
    marginTop: "-3rem",
    ":hover": { cursor: "pointer" },
  },
};

export const SeperatorStyles: ISeparatorStyles = {
  root: { width: "100%" },
  content: {},
};