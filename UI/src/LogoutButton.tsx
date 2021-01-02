import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { CommandBarButton } from "@fluentui/react";

const LogoutButton = () => {
  const { logout } = useAuth0();

  return (
    <CommandBarButton
      text="Logout"
      disabled={false}
      onClick={() => logout()}
      checked={false}
    />
  );
};

export default LogoutButton;
