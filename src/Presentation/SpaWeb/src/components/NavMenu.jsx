import React, { Fragment } from "react";
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

  return (
    <header>
      <Navbar bg="dark" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand href="/">AuctionSystem</Navbar.Brand>
          <Navbar.Toggle />
          <Navbar.Collapse>
            <Nav className="mr-auto">
              <Nav.Link href="/items">All Items</Nav.Link>
              <Nav.Link href="/contact">Contact us</Nav.Link>
            </Nav>
            <Form className="my-2 my-lg-0 mx-auto" inline>
              <FormControl
                type="text"
                placeholder="Search item"
                className="mr-sm-2"
              />
              <Button variant="outline-info">Search</Button>
            </Form>
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
                <Nav className="mr-auto">
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
