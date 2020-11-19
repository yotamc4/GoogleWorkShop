import { initializeIcons, Stack } from "@fluentui/react";
import React from "react";
import "./App.css";
import { Home } from "./HomePage/Home";
import ButtonAppBar from "./LoginBar";
import { Route, Switch } from "react-router-dom";
import { NewGroupBuyingForm } from "./NewGroupBuyingForm/NewGroupBuyingForm";

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
        <Route path="/products/:id"><NewGroupBuyingForm /></Route>
      </Switch>
    </>
  );
}

export default App;
