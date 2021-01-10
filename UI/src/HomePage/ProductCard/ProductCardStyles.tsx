import { FontWeights, getTheme, ITextStyles, ITheme } from "@fluentui/react";
import { ICardStyles } from "@uifabric/react-cards";
import { CSSProperties } from "react";

const theme: ITheme = getTheme();

export const circleStyle: CSSProperties = {
  width: "36px",
  height: "36px",
  lineHeight: "36px",
  borderRadius: "50%",
  fontSize: "75%",
  color: "#fff",
  textAlign: "center",
  background: theme.palette.blue,
  position: "absolute",
  zIndex: 100,
  marginLeft: "10px",
  marginTop: "20px",
  fontWeight: 600,
};

export const amoutTextStyles: ITextStyles = {
  root: {
    color: theme.palette.blue,
    fontWeight: FontWeights.semibold,
  },
};

export const nameOfProductTextStyles: ITextStyles = {
  root: {
    minHeight: "3rem",
    color: "#333333",
    fontWeight: FontWeights.semibold,
  },
};

export const priceTextStyles: ITextStyles = {
  root: {
    minHeight: "3rem",
    color: theme.palette.red,
    fontWeight: FontWeights.semibold,
  },
};

export const descriptionTextStyles: ITextStyles = {
  root: {
    color: "#666666",
    minHeight: "9rem",
  },
};

export const cardStyles: ICardStyles = {
  root: {
    selectors: {
      ":hover": {
        cursor: "pointer",
      },
    },
  },
};
