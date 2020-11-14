import { initializeIcons, Stack } from "@fluentui/react";
import React from "react";
import "./App.css";
import { Home } from "./HomePage/Home";
import ButtonAppBar from "./LoginBar";
import { Route } from "react-router-dom";
import { NewGroupBuyingForm } from "./NewGroupBuyingForm/NewGroupBuyingForm";

initializeIcons();

function App() {
  return (
    <>
      <Stack horizontalAlign="center">
        <ButtonAppBar />
      </Stack>

      <Route exact path="/">
        <Home />
      </Route>
      <Route path="/createNewGroup">
        <NewGroupBuyingForm />
      </Route>
    </>
  );
}

export default App;
