import React, { useEffect, useState } from "react";
import ReactCardCarousel from "react-card-carousel";
import itemsService from "../../../services/itemsService";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFire } from "@fortawesome/free-solid-svg-icons";
import { Container, Card } from "react-bootstrap";
import { itemDetailsSlug } from "../../../utils/helpers/slug";

import "../HottestItems/HottestItems.css";

export const HottestItems = () => {
  const [items, setItems] = useState([]);
  useEffect(() => {
    retrieveHottestUpcomingItems();
  }, []);

  const retrieveHottestUpcomingItems = () => {
    itemsService.getHottestUpcomingItems().then((response) => {
      setItems(response.data.data);
    });
  };

  return items.length !== 0 ? (
    <Container className="pt-5">
      <h2>
        Hottest upcoming items{" "}
        <FontAwesomeIcon icon={faFire} pulse color="red" />
      </h2>
      <div className="react-card-carousel-container">
        <ReactCardCarousel autoplay={true} autoplay_speed={2500}>
          {items.map((item, index) => {
            return (
              <Card key={index} className="react-card">
                <Card.Img
                  variant="top"
                  src={item.pictures[0]?.url}
                  alt="Item image"
                />
                <Card.Body>
                  <Card.Title>{item.title}</Card.Title>
                  <p className="card-text">
                    {process.env.REACT_APP_CURRENCY_SIGN}
                    {item.startingPrice}
                  </p>
                  <Link
                    className="btn btn-primary"
                    to={itemDetailsSlug(item.title, item.id)}
                  >
                    View
                  </Link>
                </Card.Body>
              </Card>
            );
          })}
        </ReactCardCarousel>
      </div>
    </Container>
  ) : (
    ""
  );
};
