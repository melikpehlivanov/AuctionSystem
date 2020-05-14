import React, { Fragment } from "react";
import { NavMenu } from "./components/NavMenu";
import { Home } from "./components/Home/Home";
import { Switch, Route } from "react-router-dom";
import { Container } from "react-bootstrap";
import { ToastContainer } from "react-toastify";
import { NetworkError } from "./components/Error/NetworkError";
import { NotFound } from "./components/Error/NotFound";

import "./App.css";
import "react-toastify/dist/ReactToastify.css";

function App() {
  return (
    <Fragment>
      <NavMenu />
      <Container>
        <ToastContainer />
        <Switch>
          <Route exact path={["/", "/home"]} component={Home} />
          {/* <Route exact path="/login" component={Login} />
        <Route exact path="/register" component={Register} /> */}
          <Route exact path="/error/network" component={NetworkError} />
          <Route path="*" component={NotFound} />
        </Switch>
      </Container>
    </Fragment>
  );
}

export default App;
