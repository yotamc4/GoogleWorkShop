import React from "react";

export const AuthContext: React.Context<AuthContextProps> = React.createContext<
  AuthContextProps
>({
  isLoggedIn: false,
  updateAuthContext: () => {},
});

export const AuthContextProvider: React.FunctionComponent = (props) => {
  const [authInfo, setAuthInfo] = React.useState<AuthContextProps>({
    isLoggedIn: false,
    updateAuthContext: () => {},
  });

  function updateAuthContext(newAuthInfo: AuthContextProps) {
    // Need to extract the token from facebook response and then send it
    // to the BE and receiving from the BE a token/information
  }

  return (
    <AuthContext.Provider
      value={{
        updateAuthContext: updateAuthContext,
        isLoggedIn: false,
      }}
    >
      {props.children}
    </AuthContext.Provider>
  );
};

interface AuthContextProps {
  isLoggedIn: boolean;
  updateAuthContext: Function;
  userId?: string;
  isSupplier?: boolean;
  facebookAuthResponse?: FacebookAuthResponse;
}

// Need to modify this interface according to the info we get as response from
// facebook redirect
interface FacebookAuthResponse {
  token?: string;
}
