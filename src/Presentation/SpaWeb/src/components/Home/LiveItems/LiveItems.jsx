import React, { useEffect, useState } from "react";
import itemsService from "../../../services/itemsService";
import slugify from "react-slugify";
import { Container, Card } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCaretRight } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { PictureContainer } from "./Pictures/PictureContainer";

export const LiveItems = () => {
  const [items, setItems] = useState([]);
  const [totalItems, setTotalItems] = useState(0);

  useEffect(() => {
    retrieveLiveItems();
  }, []);

  const retrieveLiveItems = () => {
    itemsService.getLiveItems().then((response) => {
      setItems(response.data.data);
      setTotalItems(response.data.totalDataCount);
    });
  };

  const generateSlug = (title, id) => {
    return `/items/${slugify(title)}/${id}`;
  };

  return (
    <Container className="pt-3">
      <h3>{totalItems} Live Items</h3>
      {items.length !== 0 ? (
        <div className="row">
          {items.map((item, index) => {
            return (
              <div key={index} className="col-12 col-md-6 col-lg-4 p-1">
                <Card className="shadow m-2">
                  {item.pictures.length === 1 ? (
                    <Link to={generateSlug(item.title, item.id)}>
                      <Card.Img
                        height="240px"
                        variant="top"
                        alt="item image"
                        src={item.pictures[0]?.url}
                      />
                    </Link>
                  ) : (
                    <PictureContainer
                      pictures={item.pictures}
                      itemSlug={generateSlug(item.title, item.id)}
                    />
                  )}
                  <Card.Body>
                    <Card.Title>
                      <Link to={generateSlug(item.title, item.id)}>
                        {item.title}
                      </Link>
                    </Card.Title>
                    <Link to={`/bids/${slugify(item.title)}/${item.id}`}>
                      <span className="float-right" style={{ color: "red" }}>
                        Bid now <FontAwesomeIcon icon={faCaretRight} />
                      </span>
                    </Link>
                  </Card.Body>
                </Card>
              </div>
            );
          })}
        </div>
      ) : (
        ""
      )}
    </Container>
  );
};
