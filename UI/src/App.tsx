import { initializeIcons, Stack } from "@fluentui/react";
import React from "react";
import "./App.css";
import { Home } from "./HomePage/Home";
import ButtonAppBar from "./LoginBar";
import { Route, Switch } from "react-router-dom";
import { NewGroupBuyingForm } from "./NewGroupBuyingForm/NewGroupBuyingForm";
import { ProductPage } from "./ProductPage/ProductPage";
import { UserProfile } from "./UserProfile/UserProfile";
import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";

initializeIcons();

function App() {
  const { isAuthenticated, user, getAccessTokenSilently } = useAuth0();

  React.useEffect(() => {
    const registerNewUser = async () => {
      if (
        isAuthenticated &&
        user["https://UniBuyClient.workshop.com/isFirstLogin"] === "true"
      ) {
        try {
          const accessToken = await getAccessTokenSilently({
            audience: `https://UniBuyBackend.workshop.com`,
          });
          const config = {
            headers: { Authorization: `Bearer ${accessToken}` },
          };
          const newUserRequest: NewUserRequest = {
            name: user.name,
            email: user.email,
            profilePicture: user.picture,
            userId: user.sub,
          };
          if (user["https://UniBuyClient.workshop.com/role"] === "Consumer") {
            axios
              .post(
                `https://localhost:5001/api/v1/buyers`,
                newUserRequest,
                config
              )
              .then((response) => {
                console.log(response);
              })
              .catch((error) => {
                console.error(error);
              });
          } else {
            axios
              .post(
                `https://localhost:5001/api/v1/Suppliers`,
                newUserRequest,
                config
              )
              .then((response) => {
                console.log(response);
              })
              .catch((error) => {
                console.error(error);
              });
          }
        } catch (e) {
          console.log(e.message);
        }
      }
    };
    registerNewUser();
  }, [isAuthenticated]);
  return (
    <>
      <Stack horizontalAlign="center">
        <ButtonAppBar />
      </Stack>
      <Switch>
        <Route exact path="/">
          <Home />
        </Route>
        <Route path="/createNewGroup">
          <NewGroupBuyingForm />
        </Route>
        <Route path="/groups">
          <Home />
        </Route>
        <Route path="/products/:id">
          <ProductPage />
        </Route>
        <Route path="/user/:user">
          <UserProfile />
        </Route>
      </Switch>
    </>
  );
}

export default App;

export interface NewUserRequest {
  name: string;
  userId: string;
  email: string;
  profilePicture: string;
}
