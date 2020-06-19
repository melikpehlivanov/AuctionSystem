import React, { useState, Fragment, useEffect } from "react";
import moment from "moment";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./DateTimePicker.css";
import { Form } from "react-bootstrap";

export const StartTimeDatePicker = ({
  startTime,
  setStartTime,
  endTime,
  disabled,
  errorHideTimeOut = 3000,
}) => {
  const startTimeConf = createDatePickerConf(
    startTime,
    30,
    moment().endOf("day")
  );
  const [error, setError] = useState();

  useEffect(() => {
    const interval = setTimeout(() => setError(null), errorHideTimeOut);
    return () => {
      clearTimeout(interval);
    };
  }, [error, errorHideTimeOut]);

  const handleOnChange = (date) => {
    if (date <= moment()) {
      setError("Start time cannot be before current time.");
      return;
    }
    if (date >= endTime) {
      setError("Start time cannot be after end time.");
      return;
    }

    setError(null);
    setStartTime(date);
  };

  return (
    <Fragment>
      <DatePicker
        className="form-control custom-select"
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
  errorHideTimeOut = 3000,
}) => {
  const [error, setError] = useState();
  const endTimeConf = createDatePickerConf(endTime, 30, moment().endOf("day"));

  useEffect(() => {
    const interval = setTimeout(() => setError(null), errorHideTimeOut);
    return () => {
      clearTimeout(interval);
    };
  }, [error, errorHideTimeOut]);

  const handleOnChange = (date) => {
    if (date < moment()) {
      setError("End time cannot be after current time.");
      return;
    }
    if (date <= startTime) {
      setError("End time cannot be before start time.");
      return;
    }

    setEndTime(date);
  };

  return (
    <Fragment>
      <DatePicker
        className="form-control custom-select"
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
