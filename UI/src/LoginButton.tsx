import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { CommandBarButton } from "@fluentui/react";

const LoginButton = () => {
  const { loginWithRedirect } = useAuth0();

  return (
    <CommandBarButton
      text="Login"
      disabled={false}
      onClick={() => loginWithRedirect()}
      checked={false}
    />
  );
};

export default LoginButton;
