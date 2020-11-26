import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Button from "@material-ui/core/Button";
import {
  getTheme,
  IImageProps,
  ImageFit,
  ITheme,
  Image,
  Stack,
  IStackTokens,
  StackItem,
  IStackItemStyles,
  Separator,
  Label,
  CommandBarButton,
  IStackStyles,
} from "@fluentui/react";
import IconButton from "@material-ui/core/IconButton";

const theme: ITheme = getTheme();
const useStyles = makeStyles((theme) => ({
  title: {
    flexGrow: 1,
    alignItems: "left",
  },
}));

const serachFilterGapStackTokens: IStackTokens = {
  padding: 10,
};

const imagePropsLogo: IImageProps = {
  src: "/Images/logo.PNG",
  imageFit: ImageFit.contain,
};

const imagePropsSubLogo: IImageProps = {
  src: "/Images/subLogo2.PNG",
  imageFit: ImageFit.cover,
};

const StacStyles: IStackStyles = {
  root: {
    paddingLeft: "0.7rem",
    paddingRight: "0.7rem",
  },
};

const StacStyles3: IStackStyles = {
  root: {
    paddingTop: "3rem",
  },
};

const StacStyles2: IStackStyles = {
  root: {
    height: "20rem",
  },
};
export default function ButtonAppBar() {
  const classes = useStyles();

  const StackItemStyles: IStackItemStyles = {
    root: {
      marginLeft: "28rem",
      marginTop: "-3rem",
    },
  };

  return (
    <Stack tokens={serachFilterGapStackTokens} styles={StacStyles3}>
      <StackItem styles={StackItemStyles}>
        <Image {...imagePropsLogo} width={200} height={140} />
      </StackItem>
      <Stack>
        <Separator theme={theme} styles={{ content: { width: "67rem" } }} />
        <Stack horizontal horizontalAlign="space-between" styles={StacStyles}>
          <Label>Hello Guest!</Label>
          <CommandBarButton text="Login" disabled={false} checked={false} />
        </Stack>
        <Separator styles={{ root: { width: "100%" } }} />
      </Stack>
      <StackItem styles={StacStyles2}>
        <Image {...imagePropsSubLogo} width={1150} height={300} />
      </StackItem>
    </Stack>
  );
}
