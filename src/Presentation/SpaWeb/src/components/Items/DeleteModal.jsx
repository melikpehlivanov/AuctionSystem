import React, { useState } from "react";
import { Modal, Button, Spinner } from "react-bootstrap";
import itemsService from "../../services/itemsService";
import { history } from "../..";
import { toast } from "react-toastify";

export const DeleteModal = ({ show, handleClose, item }) => {
  const [isLoading, setIsLoading] = useState(false);

  const handleDelete = (id) => {
    setIsLoading(true);
    itemsService.deleteItem(id).then(() => {
      toast.success("Delete operation was successful");
      setIsLoading(false);
      history.push("/");
    });
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>
          Are you sure you want to delete {item?.title}?{" "}
          <span className="text-muted">This operation is irreversible!</span>
        </Modal.Title>
      </Modal.Header>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>
          Close
        </Button>
        {isLoading ? (
          <Button variant="outline-danger" disabled>
            <Spinner
              as="span"
              animation="grow"
              size="sm"
              role="status"
              aria-hidden="true"
            />
            Deleting...
          </Button>
        ) : (
          <Button
            variant="outline-danger"
            onClick={() => handleDelete(item?.id)}
          >
            Delete
          </Button>
        )}
      </Modal.Footer>
    </Modal>
  );
};
