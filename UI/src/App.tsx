import { initializeIcons, Stack } from "@fluentui/react";
import React from "react";
import "./App.css";
import { Home } from "./HomePage/Home";
import ButtonAppBar from "./LoginBar";
import { Route, Switch } from "react-router-dom";
import { NewGroupBuyingForm } from "./NewGroupBuyingForm/NewGroupBuyingForm";
import { ProductPage } from "./ProductPage/ProductPage";

initializeIcons();

function App() {
  return (
    <>
      <Stack horizontalAlign="center">
        <ButtonAppBar />
      </Stack>
      <Switch>
        <Route exact path="/"><Home/> </Route>
        <Route path="/createNewGroup"><NewGroupBuyingForm /></Route>
        <Route path="/products/:id"><ProductPage /></Route>
      </Switch>
    </>
  );
}

export default App;
