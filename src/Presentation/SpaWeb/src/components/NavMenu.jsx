import React, { Fragment } from "react";
import { Nav, Navbar, Container, NavDropdown } from "react-bootstrap";
import { history } from "..";
import { useAuth } from "../utils/hooks/authHook";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser } from "@fortawesome/free-solid-svg-icons";

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
              <Nav.Link onClick={() => history.push("/items")}>Items</Nav.Link>
              <Nav.Link onClick={() => history.push("/contact")}>
                Contact us
              </Nav.Link>
            </Nav>
            {auth.user ? (
              <Nav>
                <NavDropdown
                  title={<FontAwesomeIcon icon={faUser} />}
                  style={{ textColor: "white" }}
                >
                  {auth.user.isAdmin ? (
                    <NavDropdown.Item
                      onClick={() => {
                        history.push("/administration");
                      }}
                    >
                      Administration
                    </NavDropdown.Item>
                  ) : (
                    ""
                  )}
                  <NavDropdown.Item
                    onClick={() => {
                      history.push("/items/create");
                    }}
                  >
                    Create Item
                  </NavDropdown.Item>
                  <NavDropdown.Divider />
                  <NavDropdown.Item
                    onClick={() => {
                      auth.signOut();
                      history.push("/");
                    }}
                  >
                    Logout
                  </NavDropdown.Item>
                </NavDropdown>
              </Nav>
            ) : (
              <Fragment>
                <Nav>
                  <Nav.Link onClick={() => history.push("/sign-up")}>
                    Sign up
                  </Nav.Link>
                  <Nav.Link onClick={() => history.push("/sign-in")}>
                    Login
                  </Nav.Link>
                </Nav>
              </Fragment>
            )}
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
