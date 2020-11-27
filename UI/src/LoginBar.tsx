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
  IImageStyles,
} from "@fluentui/react";
import { useHistory } from "react-router";

const theme: ITheme = getTheme();
const useStyles = makeStyles((theme) => ({
  title: {
    flexGrow: 1,
    alignItems: "left",
  },
}));

const imagePropsLogo: IImageProps = {
  src: "/Images/logo.PNG",
  imageFit: ImageFit.contain,
};




const StacStyles: IStackStyles = {
  root: {
    paddingLeft: "0.7rem",
    paddingRight: "0.7rem",
  },
};

const StacStyles2: IStackStyles = {
  root: {
    paddingTop: "3rem",
  },
};

const StackItemStyles: IStackItemStyles = {
  root: {
    width:"7rem",
    marginLeft: "28rem",
    marginTop: "-3rem",
    ":hover":{cursor: "pointer"}
  },
};


export default function ButtonAppBar() {
  const history = useHistory();
  const changeHistory = () => {
    //history.push(`/products/${productDetails.name.replace(/\s/g, "")}`);
    history.push(`/`);
  };

  return (
    <Stack styles={StacStyles2}>
      <StackItem styles={StackItemStyles}>
        <Image {...imagePropsLogo} width={200} height={140} onClick={changeHistory}/>
      </StackItem>
      <Stack>
        <Separator theme={theme} styles={{ content: { width: "69rem" } }} />
        <Stack horizontal horizontalAlign="space-between" styles={StacStyles}>
          <Label>Hello Guest!</Label>
          <CommandBarButton text="Login" disabled={false} checked={false} />
        </Stack>
        <Separator styles={{ root: { width: "100%" } }} />
      </Stack>
    </Stack>
  );
}
