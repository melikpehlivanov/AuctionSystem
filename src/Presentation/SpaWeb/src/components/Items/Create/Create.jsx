import React, { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import {
  Container,
  Form,
  InputGroup,
  Button,
  Spinner,
  Row,
} from "react-bootstrap";
import categoriesService from "../../../services/categoriesService";
import moment from "moment";
import "./Create.css";
import {
  StartTimeDatePicker,
  EndTimeDatePicker,
} from "../../DateTimePicker/DateTimePicker";
import itemsService from "../../../services/itemsService";
import { ImageUploader } from "../../ImageUploader/ImageUploader";
import { history } from "../../..";
import { useTime } from "../../../utils/hooks/useTime";

export const Create = () => {
  const { register, handleSubmit, errors } = useForm();
  const [isLoading, setIsLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [pictures, setPictures] = useState([]);
  const { currentTime } = useTime();
  const [startTime, setStartTime] = useState();
  const [endTime, setEndTime] = useState();

  useEffect(() => {
    setStartTime(moment(currentTime).add(2, "minutes").toDate());
    setEndTime(moment(currentTime).add(1, "week").add(10, "m").toDate());
  }, [currentTime]);

  useEffect(() => {
    categoriesService.getAll().then((response) => {
      setCategories(response.data.data);
    });
  }, []);

  const onSubmit = (data, e) => {
    setIsLoading(true);
    var formData = new FormData(e.target);
    pictures.forEach((file) => {
      formData.append("pictures", file, file.name);
    });
    formData.append("startTime", startTime.toISOString("dd/mm/yyyy HH:mm"));
    formData.append("endTime", endTime.toISOString("dd/mm/yyyy HH:mm"));

    itemsService
      .createItem(formData)
      .then((response) => {
        history.push(`/items/details/${response.data.data.id}`);
      })
      .catch(() => setIsLoading(false));
  };

  const onDrop = (picture) => {
    setPictures(picture);
  };

  return (
    <Container>
      <div className="text-center w-md-75 w-lg-50 mx-auto my-4">
        <Form
          onSubmit={handleSubmit(onSubmit)}
          style={{
            border: "1px solid #e3e6ef",
            background: "#fff",
            padding: "2rem",
          }}
        >
          <h1>Create item</h1>
          <Form.Group controlId="title">
            <Form.Label>Title</Form.Label>
            <Form.Control
              name="title"
              placeholder="Some really cool item"
              ref={register({
                required: "Title field is required",
                maxLength: 120,
              })}
            />
            {errors.title && (
              <Form.Control.Feedback type="invalid">
                {errors.title.message}
              </Form.Control.Feedback>
            )}
          </Form.Group>
          <Form.Group controlId="description">
            <Form.Label>Description</Form.Label>
            <Form.Control
              name="description"
              as="textarea"
              placeholder="This item was owned by Hitler!!! WOW!"
              ref={register({
                required: "Description field is required",
                maxLength: 500,
              })}
            />
            {errors.description && (
              <Form.Control.Feedback type="invalid">
                {errors.description.message}
              </Form.Control.Feedback>
            )}
          </Form.Group>
          <Row>
            <Form.Group className="col" controlId="StartTime">
              <Form.Label>Start time</Form.Label>
              <StartTimeDatePicker
                startTime={startTime}
                setStartTime={setStartTime}
                endTime={endTime}
              />
            </Form.Group>
            <Form.Group className="col" controlId="EndTime">
              <Form.Label>End time</Form.Label>
              <EndTimeDatePicker
                endTime={endTime}
                setEndTime={setEndTime}
                startTime={startTime}
              />
            </Form.Group>
          </Row>
          <Form.Group controlId="startingPrice">
            <Form.Label>Starting Price</Form.Label>
            <InputGroup className="mb-3">
              <InputGroup.Prepend>
                <InputGroup.Text id="starting-price">
                  {process.env.REACT_APP_CURRENCY_SIGN}
                </InputGroup.Text>
              </InputGroup.Prepend>
              <Form.Control
                name="startingPrice"
                type="number"
                placeholder="100"
                aria-describedby="starting-price"
                ref={register({
                  required: "Starting Price field is required",
                  min: 0.01,
                  max: 1000000000,
                })}
              />
            </InputGroup>
            {errors.startingPrice && (
              <Form.Control.Feedback type="invalid">
                {errors.startingPrice.message}
              </Form.Control.Feedback>
            )}
          </Form.Group>
          <Form.Group controlId="minIncrease">
            <Form.Label>
              Min price increase (
              {
                <span className="text-muted">
                  We suggest to input the 10% value of the starting price
                </span>
              }
              )
            </Form.Label>
            <InputGroup className="mb-3">
              <InputGroup.Prepend>
                <InputGroup.Text id="min-increase">
                  {process.env.REACT_APP_CURRENCY_SIGN}
                </InputGroup.Text>
              </InputGroup.Prepend>
              <Form.Control
                name="minIncrease"
                type="number"
                aria-describedby="min-increase"
                ref={register({
                  required: "Min price increase field is required",
                  min: 0.01,
                  max: 1000000000,
                })}
              />
            </InputGroup>
            {errors.minIncrease && (
              <Form.Control.Feedback type="invalid">
                {errors.minIncrease.message}
              </Form.Control.Feedback>
            )}
          </Form.Group>
          <Form.Group controlId="category">
            <Form.Label>Category</Form.Label>
            <Form.Control
              custom
              name="subCategoryId"
              ref={register({ required: "Category is required" })}
              as="select"
            >
              <option disabled>Select category</option>
              {categories.map((category) => {
                return category.subCategories.map((subCategory, index) => {
                  return (
                    <option key={index} value={subCategory.id}>
                      {subCategory.name}
                    </option>
                  );
                });
              })}
            </Form.Control>
          </Form.Group>
          <Form.Group>
            <ImageUploader onChange={onDrop} />
          </Form.Group>
          {!isLoading ? (
            <Button variant="primary" type="submit">
              Submit
            </Button>
          ) : (
            <Button variant="primary" disabled>
              <Spinner
                as="span"
                animation="grow"
                size="sm"
                role="status"
                aria-hidden="true"
              />
              Creating...
            </Button>
          )}
        </Form>
      </div>
    </Container>
  );
};
