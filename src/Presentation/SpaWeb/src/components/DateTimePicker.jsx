import React, { useState, Fragment } from "react";
import moment from "moment";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { Form } from "react-bootstrap";

export const StartTimeDatePicker = ({
  startTime,
  setStartTime,
  endTime,
  disabled,
}) => {
  const startTimeConf = createDatePickerConf(
    startTime,
    30,
    moment().endOf("day")
  );
  const [error, setError] = useState();

  const handleOnChange = (date) => {
    console.log(date);
    if (date <= moment()) {
      setError({
        startTime: "Start time cannot be before current time.",
      });
      return;
    }
    if (date >= endTime) {
      setError({
        startTime: "Start time cannot be after end time.",
      });
      return;
    }

    setStartTime(date);
  };

  return (
    <Fragment>
      <DatePicker
        disabled={disabled}
        selected={startTime}
        onChange={(date) => handleOnChange(date)}
        filterDate={(date) => {
          return moment().subtract(1, "day").toDate() < date;
        }}
        minDate={moment().toDate()}
        maxDate={endTime}
        showTimeSelect
        timeFormat="HH:mm"
        timeCaption="time"
        dateFormat="MMMM d, yyyy h:mm aa"
        {...startTimeConf}
      />
      {error && (
        <Form.Control.Feedback type="invalid">{error}</Form.Control.Feedback>
      )}
    </Fragment>
  );
};

export const EndTimeDatePicker = ({
  endTime,
  setEndTime,
  startTime,
  disabled,
}) => {
  const [error, setError] = useState();
  const endTimeConf = createDatePickerConf(endTime, 30, moment().endOf("day"));

  const handleOnChange = (date) => {
    if (date < moment()) {
      setError({
        endTime: "End time cannot be after current time.",
      });
      return;
    }
    if (date <= startTime) {
      setError({
        endTime: "End time cannot be before start time.",
      });
      return;
    }

    setEndTime(date);
  };

  return (
    <Fragment>
      <DatePicker
        disabled={disabled}
        selected={endTime}
        onChange={(date) => handleOnChange(date)}
        filterDate={(date) => {
          return moment().subtract(1, "day").toDate() < date;
        }}
        minDate={startTime}
        showTimeSelect
        timeFormat="HH:mm"
        timeCaption="time"
        dateFormat="MMMM d, yyyy h:mm aa"
        {...endTimeConf}
      />
      {error && (
        <Form.Control.Feedback type="invalid">{error}</Form.Control.Feedback>
      )}
    </Fragment>
  );
};

const createDatePickerConf = (date, ceilNumber, endDay) => {
  const conf = {};
  if (moment(date).isSame(moment(), "day")) {
    conf.minTime = Math.ceil(
      moment().hours(moment().hour()).minutes(moment().minute()),
      ceilNumber
    );
    conf.maxTime = Math.ceil(
      moment().hours(endDay.hour()).minutes(endDay.minute()),
      ceilNumber
    );
  }

  return conf;
};
