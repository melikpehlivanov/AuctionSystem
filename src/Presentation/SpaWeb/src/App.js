import React, { Fragment } from "react";
import { NavMenu } from "./components/NavMenu";
import { Home } from "./components/Home/Home";
import { Switch, Route } from "react-router-dom";
import { Container } from "react-bootstrap";
import { ToastContainer } from "react-toastify";
import { NetworkError } from "./components/Error/NetworkError";
import { NotFound } from "./components/Error/NotFound";
import { Register } from "./components/Register/Register";
import { Login } from "./components/Login/Login";

import "./App.css";
import "react-toastify/dist/ReactToastify.css";
import { ProvideAuth } from "./utils/hooks/authHook";
import { List } from "./components/Items/List/List";
import { Details } from "./components/Items/Details/Details";
import { Create } from "./components/Items/Create/Create";

function App() {
  return (
    <Fragment>
      <ProvideAuth>
        <NavMenu />
        <Container className="pt-3">
          <ToastContainer />
          <Switch>
            <Route exact path={["/", "/home"]} component={Home} />
            <Route exact path="/sign-in" component={Login} />
            <Route exact path="/sign-up" component={Register} />
            <Route exact path="/error/network" component={NetworkError} />
            <Route exact path="/items/create" component={Create} />
            <Route exact path="/items" component={List} />
            <Route path="/items/:slug/:id" component={Details} />
            <Route path="*" component={NotFound} />
          </Switch>
        </Container>
      </ProvideAuth>
    </Fragment>
  );
}

export default App;
