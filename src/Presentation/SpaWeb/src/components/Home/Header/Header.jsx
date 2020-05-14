import React, { useState, useEffect } from "react";
import { Nav, Navbar, NavDropdown } from "react-bootstrap";
import categoriesService from "../../../services/categoriesService";

import "./Header.css";

export const Header = () => {
  const [categories, setCategories] = useState([]);
  const [hoveredDropdowns, setHoveredDropdowns] = useState({});

  useEffect(() => {
    retrieveCategories();
  }, []);

  const retrieveCategories = () => {
    categoriesService.getAll().then((response) => {
      setCategories(response.data.data);
    });
  };

  const handleMouseOver = (e) => {
    setHoveredDropdowns({
      [e.target.id]: true,
    });
  };

  const handleMouseOut = (e) => {
    setHoveredDropdowns({
      [e.target.id]: false,
    });
  };

  return (
    <Navbar className="shadow mt-3" bg="light" expand="lg">
      <Navbar.Brand>Categories</Navbar.Brand>
      <Navbar.Toggle />
      <Navbar.Collapse>
        <Nav className="mx-auto">
          {categories.map((category, index) => {
            return (
              <NavDropdown
                key={index}
                id={index}
                title={category.name}
                show={hoveredDropdowns[index]}
                onMouseOver={(e) => handleMouseOver(e)}
                onMouseOut={(e) => handleMouseOut(e)}
              >
                {category.subCategories.map((subcategory, index) => {
                  return (
                    <NavDropdown.Item key={index}>
                      {subcategory.name}
                    </NavDropdown.Item>
                  );
                })}
              </NavDropdown>
            );
          })}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};
