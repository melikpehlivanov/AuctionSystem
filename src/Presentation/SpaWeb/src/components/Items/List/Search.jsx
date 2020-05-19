import React, { useState } from "react";
import { Col, Form } from "react-bootstrap";
import InputRange from "react-input-range";
import "react-input-range/lib/css/index.css";
import DateTimePicker from "react-datetime-picker";

export const Search = ({ state, setState }) => {
  const [price, setPrice] = useState({ min: 1, max: 15000 });
  const [startTime, setstartTime] = useState(new Date());
  const dateAfterTenDays = new Date();
  dateAfterTenDays.setDate(dateAfterTenDays.getDate() + 10);
  const [endTime, setendTime] = useState(dateAfterTenDays);
  const [dateEnabled, setDateEnabled] = useState(true);

  return (
    <Col className="mt-5 mt-lg-0" lg={4}>
      <div>
        <div className="search-area default-item-search">
          <p>
            <i>Filters</i>
          </p>

          <Form>
            <Form.Group controlId="Title">
              <Form.Control
                onChange={(e) => setState({ title: e.target.value })}
                type="input"
                placeholder="Search for given item by title"
                aria-label="Item search"
                aria-describedby="basic-addon1"
              />
            </Form.Group>
            <Form.Group controlId="liveItems">
              <Form.Check
                onChange={(e) =>
                  setState({ getLiveItems: !state.getLiveItems })
                }
                type="switch"
                label="Live items"
              />
            </Form.Group>
            <Form.Group controlId="Price">
              <p className="mb-4">Price</p>
              <InputRange
                formatLabel={(value) => `€${value}`}
                step={100}
                value={price}
                maxValue={15000}
                minValue={1}
                onChange={(value) => {
                  setPrice(value);
                  setState({
                    startingPrice: value.min < 1 ? 1 : value.min,
                    maxPrice: price.max,
                  });
                }}
              />
            </Form.Group>

            <Form.Group className="mt-5" controlId="dateEnabled">
              <Form.Check
                onChange={() => {
                  setDateEnabled(!dateEnabled);
                  if (!dateEnabled) {
                    setState({
                      startTime: null,
                      endTime: null,
                    });
                  }
                }}
                type="switch"
                label="Modify date"
              />
            </Form.Group>
            <Form.Group controlId="StartingTime">
              <p>Starting time</p>
              <DateTimePicker
                disabled={dateEnabled}
                readOnly={true}
                format="dd-MM-yyyy HH:mm"
                value={startTime}
                onChange={(value) => {
                  setstartTime(value);
                  setState({
                    startTime: startTime,
                  });
                }}
              />
            </Form.Group>
            <Form.Group controlId="EndTime">
              <p>End time</p>
              <DateTimePicker
                disabled={dateEnabled}
                format="dd-MM-yyyy HH:mm"
                value={endTime}
                onChange={(value) => {
                  setendTime(value);
                  setState({
                    endTime: endTime,
                  });
                }}
              />
            </Form.Group>
          </Form>
        </div>
      </div>
    </Col>
  );
};
