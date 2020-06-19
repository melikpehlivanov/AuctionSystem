import React, { useState, Fragment } from "react";
import { Card, Button } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import { DeleteModal } from "../DeleteModal";
import { itemEditSlug } from "../../../utils/helpers/slug";
import { history } from "../../..";

export const UserActionsContainer = ({ userId, item }) => {
  const [showModal, setShowModal] = useState(false);

  const handleClose = () => setShowModal(false);
  const handleShow = () => setShowModal(true);

  return userId === item.userId ? (
    <Fragment>
      <Card className="text-center">
        <Card.Header>Actions</Card.Header>
        <div className="list-group list-group-flush">
          <Button
            className="list-group-item list-group-item-action text-primary"
            onClick={() =>
              history.push(
                itemEditSlug(item.title, item.id),
                history.location.pathname
              )
            }
          >
            <FontAwesomeIcon icon={faEdit} /> Edit
          </Button>
        </div>
        <div className="list-group list-group-flush">
          <Button
            onClick={handleShow}
            className="list-group-item list-group-item-action text-danger"
          >
            <FontAwesomeIcon icon={faTrash} /> Delete
          </Button>
        </div>
      </Card>
      <DeleteModal show={showModal} handleClose={handleClose} item={item} />
    </Fragment>
  ) : (
    ""
  );
};
