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
  Persona,
} from "@fluentui/react";
import { useHistory } from "react-router";
import FacebookLogin from "react-facebook-login";
import { AuthContext } from "./Context/AuthContext";
const theme: ITheme = getTheme();

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
    width: "7rem",
    marginLeft: "28rem",
    marginTop: "-3rem",
    ":hover": { cursor: "pointer" },
  },
};

export default function ButtonAppBar() {
  const [picture, setPicture] = React.useState<string>("");
  const [name, setName] = React.useState<string>("");
  const { isLoggedIn, updateAuthContext } = React.useContext(AuthContext);

  const responseFacebook = (response: any) => {
    setPicture(response.picture.data.url);
    setName(response.name);
    //updateAuthContext(response);
  };

  const history = useHistory();
  const changeHistory = () => {
    //history.push(`/products/${productDetails.name.replace(/\s/g, "")}`);
    history.push(`/`);
  };

  return (
    <Stack styles={StacStyles2}>
      <StackItem styles={StackItemStyles}>
        <Image
          {...imagePropsLogo}
          width={200}
          height={140}
          onClick={changeHistory}
        />
      </StackItem>
      <Stack>
        <Separator theme={theme} styles={{ content: { width: "69rem" } }} />
        {picture == "" ? (
          <Stack horizontal horizontalAlign="space-between" styles={StacStyles}>
            <Label>Hello Guest!</Label>
            <FacebookLogin
              appId="1018527418646121"
              autoLoad={true}
              fields="name,email,picture"
              callback={responseFacebook}
              buttonStyle={{
                width: "12rem",
                fontSize: "0.6rem",
                height: "2rem",
                padding: "0.7rem",
                paddingTop: "0.4rem",
              }}
            />
          </Stack>
        ) : (
          <Stack horizontal horizontalAlign="space-between" styles={StacStyles}>
            <Persona imageUrl={picture} text={name} />
            <CommandBarButton text="Logout" disabled={false} checked={false} />
          </Stack>
        )}
        <Separator styles={{ root: { width: "100%" } }} />
      </Stack>
    </Stack>
  );
}
