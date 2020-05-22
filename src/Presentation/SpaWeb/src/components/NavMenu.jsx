import React, { Fragment, useState } from "react";
import {
  Nav,
  Navbar,
  Form,
  FormControl,
  Button,
  Container,
} from "react-bootstrap";
import { history } from "..";
import { useAuth } from "../utils/helpers/authHook";

export const NavMenu = () => {
  const auth = useAuth();
  const [title, setTitle] = useState("");

  return (
    <header>
      <Navbar bg="dark" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand href="/">AuctionSystem</Navbar.Brand>
          <Navbar.Toggle />
          <Navbar.Collapse>
            <Nav className="mr-auto">
              <Nav.Link href="/items">Items</Nav.Link>
              <Nav.Link href="/contact">Contact us</Nav.Link>
            </Nav>
            {auth.user ? (
              <Button
                onClick={() => {
                  auth.signOut();
                  history.push("/");
                  window.location.reload();
                }}
              >
                logout
              </Button>
            ) : (
              <Fragment>
                <Nav>
                  <Nav.Link href="/sign-up">Sign up</Nav.Link>
                  <Nav.Link href="/sign-in">Login</Nav.Link>
                </Nav>
              </Fragment>
            )}
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
