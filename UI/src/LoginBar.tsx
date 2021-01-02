import React from "react";
import {
  getTheme,
  IImageProps,
  ImageFit,
  ITheme,
  Image,
  Stack,
  StackItem,
  Separator,
  Label,
  CommandBarButton,
  Persona,
} from "@fluentui/react";
import { useHistory } from "react-router";
import FacebookLogin from "react-facebook-login";
import { AuthContext } from "./Context/AuthContext";
import {
  SeperatorStyles,
  StackItemStyles,
  StacStyles,
  StacStyles2,
} from "./LoginBarStyles";
import LoginButton from "./LoginButton";
import { useAuth0 } from "@auth0/auth0-react";
import LogoutButton from "./LogoutButton";

const theme: ITheme = getTheme();

const imagePropsLogo: IImageProps = {
  src: "/Images/logo.PNG",
  imageFit: ImageFit.contain,
};

export default function ButtonAppBar() {
  const { isAuthenticated, user } = useAuth0();

  const [picture, setPicture] = React.useState<string>("");
  const [name, setName] = React.useState<string>("");

  React.useEffect(()=>{
    if (isAuthenticated){
      setName(user?.name);
      setPicture(user?.picture);
    }
    else{
      
    }
  },[isAuthenticated]);

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
        <Separator theme={theme} styles={{ content: { width: "75rem" } }} />
        {isAuthenticated ? 
        (
          <Stack horizontal horizontalAlign="space-between" styles={StacStyles}>
            <Persona imageUrl={picture} text={name} />
            <LogoutButton/>
          </Stack>
        )
        :
        (
          <Stack horizontal horizontalAlign="space-between" styles={StacStyles}>
            <Label>Hello Guest!</Label>
            <LoginButton/>
          </Stack>
        ) }
        <Separator styles={SeperatorStyles} />
      </Stack>
    </Stack>
  );
}
