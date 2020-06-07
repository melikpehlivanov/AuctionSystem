import React, { Fragment, useEffect, useState } from "react";
import { Row, Col, Button, Card } from "react-bootstrap";
import ImageGallery from "react-image-gallery";
import itemsService from "../../../services/itemsService";
import { useParams } from "react-router-dom";
import "moment-timezone";
import { SideBar } from "./SideBar";
import "./Details.css";

export const Details = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [item, setItem] = useState({});
  const [images, setImages] = useState([]);

  let { id } = useParams();

  useEffect(() => {
    setIsLoading(true);
    itemsService.getItemById(id).then(({ data: response }) => {
      setItem(response.data);
      setImages(createImageObj(response.data.pictures));
      setIsLoading(false);
    });
  }, [id]);

  const createImageObj = (pictures) => {
    let data = [];
    pictures.forEach((element) => {
      data.push({
        original: element.url,
        thumbnail: element.url,
      });
    });

    return data;
  };

  return (
    <Fragment>
      {!isLoading ? (
        <Row>
          <Col md={8}>
            <h1 style={{ wordBreak: "break-word" }}>{item.title}</h1>
            <ImageGallery showPlayButton={false} items={images} />
            <Card className="mb-3">
              <Card.Header>
                <Button>Details</Button>
              </Card.Header>
              <Card.Body>
                <Card.Text>{item.description}</Card.Text>
                <h5 className="card-title">Starting Bid</h5>
                <Card.Text>&euro;{item.startingPrice}</Card.Text>
              </Card.Body>
            </Card>
          </Col>
          <SideBar
            item={item}
            startTime={item.startTime}
            endTime={item.endTime}
            startingPrice={item.startingPrice}
          />
          <SideBar />
        </Row>
      ) : (
        "Loading..."
      )}
    </Fragment>
  );
};
