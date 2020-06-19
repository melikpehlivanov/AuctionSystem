import React, { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { Container, Form, InputGroup, Button, Spinner } from "react-bootstrap";
import categoriesService from "../../../services/categoriesService";
import moment from "moment";
import {
  StartTimeDatePicker,
  EndTimeDatePicker,
} from "../../DateTimePicker/DateTimePicker";
import itemsService from "../../../services/itemsService";
import { ImageUploader } from "../../ImageUploader/ImageUploader";
import { useParams } from "react-router-dom";
import { history } from "../../..";
import { useTime } from "../../../utils/hooks/useTime";

export const Edit = () => {
  const { register, handleSubmit, errors } = useForm();
  const [isLoading, setIsLoading] = useState(true);
  const [item, setItem] = useState();
  const [categories, setCategories] = useState([]);
  const [pictures, setPictures] = useState([]);
  const [removedPictures, setRemovedPictures] = useState([]);

  const { currentTime } = useTime();

  const [startTime, setStartTime] = useState(
    moment(currentTime).add(2, "minutes").toDate()
  );
  const [endTime, setEndTime] = useState(
    moment(currentTime).add(1, "months").add(10, "m").toDate()
  );

  const { id } = useParams();

  useEffect(() => {
    setStartTime(moment(currentTime).add(1, "minutes").toDate());
  }, [currentTime]);

  useEffect(() => {
    async function getCategories() {
      const categories = await categoriesService.getAll();
      setCategories(categories.data.data);
    }

    async function getItem() {
      const item = (await itemsService.getItemById(id)).data.data;
      setItem(item);
      const urls = await Promise.all(
        item.pictures.map(async ({ id, url }) => {
          return { id, url: await toDataURL(url) };
        })
      );
      setPictures((prev) => [...prev, ...urls]);
    }

    async function fetch() {
      await getCategories();
      await getItem();
      setIsLoading(false);
    }

    fetch();
  }, [id]);

  const toDataURL = (url) =>
    fetch(url)
      .then((response) => response.blob())
      .then(
        (blob) =>
          new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onloadend = () => resolve(reader.result);
            reader.onerror = reject;
            reader.readAsDataURL(blob);
          })
      );

  const onSubmit = (data, e) => {
    console.log("xixix0");
    setIsLoading(true);
    var formData = new FormData(e.target);
    pictures.forEach((file) => {
      formData.append("picturesToAdd", file, file.name);
    });
    removedPictures.forEach((id) => {
      formData.append("picturesToRemove", id);
    });

    formData.append("id", id);
    formData.append("startTime", startTime.toISOString("dd/mm/yyyy HH:mm"));
    formData.append("endTime", endTime.toISOString("dd/mm/yyyy HH:mm"));

    itemsService
      .editItem(id, formData)
      .then(() => {
        history.push(`/items/details/${id}`);
      })
      .catch(() => setIsLoading(false));
  };

  const onDrop = (pictureFiles) => {
    setPictures(pictureFiles);
  };

  return (
    <Container>
      <div className="text-center w-md-75 w-lg-50 mx-auto my-4">
        {!isLoading ? (
          <Form
            onSubmit={handleSubmit(onSubmit)}
            style={{
              border: "1px solid #e3e6ef",
              background: "#fff",
              padding: "2rem",
            }}
          >
            <h1>Edit</h1>
            <Form.Group controlId="title">
              <Form.Label>Title</Form.Label>
              <Form.Control
                name="title"
                placeholder="Some really cool item"
                ref={register({
                  required: "Title field is required",
                  maxLength: 120,
                })}
                defaultValue={item.title}
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
                defaultValue={item.description}
              />
              {errors.description && (
                <Form.Control.Feedback type="invalid">
                  {errors.description.message}
                </Form.Control.Feedback>
              )}
            </Form.Group>
            <Form.Group controlId="StartTime">
              <p>Start time</p>
              <StartTimeDatePicker
                startTime={startTime}
                setStartTime={setStartTime}
                endTime={endTime}
              />
            </Form.Group>
            <Form.Group controlId="EndTime">
              <p>End time</p>
              <EndTimeDatePicker
                endTime={endTime}
                setEndTime={setEndTime}
                startTime={startTime}
              />
            </Form.Group>
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
                  defaultValue={item.startingPrice}
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
                  defaultValue={item.minIncrease}
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
                name="subCategoryId"
                ref={register({ required: "Category is required" })}
                as="select"
                defaultValue={item.subCategoryId}
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
              <ImageUploader
                onChange={onDrop}
                images={pictures}
                onRemove={setRemovedPictures}
              />
            </Form.Group>
            <div>
              <Button
                variant="outline-info"
                className="mr-3"
                onClick={() => history.goBack()}
              >
                Cancel
              </Button>
              <Button variant="outline-danger" type="submit">
                Submit
              </Button>
            </div>
          </Form>
        ) : (
          <Spinner animation="border" />
        )}
      </div>
    </Container>
  );
};
