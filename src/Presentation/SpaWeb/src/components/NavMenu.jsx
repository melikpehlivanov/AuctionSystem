import React from "react";
import {
  Nav,
  Navbar,
  Form,
  FormControl,
  Button,
  Container,
} from "react-bootstrap";

export const NavMenu = () => {
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
            <Nav className="mr-auto">
              <Nav.Link href="/register">Register</Nav.Link>
              <Nav.Link href="/login">Login</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
