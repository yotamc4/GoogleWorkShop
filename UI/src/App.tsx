import { initializeIcons, Stack } from "@fluentui/react";
import React from "react";
import "./App.css";
import { Home } from "./HomePage/Home";
import ButtonAppBar from "./LoginBar";
import { Route, Switch } from "react-router-dom";
import { NewGroupBuyingForm } from "./NewGroupBuyingForm/NewGroupBuyingForm";
import { ProductPage } from "./ProductPage/ProductPage";
import { UserProfile } from "./UserProfile/UserProfile";
import { SuppliersSurvey } from "./ProductPage/Suppliers/SupplierSurvey";

initializeIcons();

function App() {
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
        <Route path="/products/:id">
          <ProductPage mockProductId={10} />
        </Route>
        <Route path="/user/:user">
          <UserProfile />
        </Route>
      </Switch>
    </>
  );
}

export default App;
