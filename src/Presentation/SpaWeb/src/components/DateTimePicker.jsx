import React from "react";
import moment from "moment";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

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

  const handleOnChange = (date) => {
    console.log(date);
    if (date <= moment()) {
      return;
    }
    if (date >= endTime) {
      return;
    }

    setStartTime(date);
  };

  return (
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
  );
};

export const EndTimeDatePicker = ({
  endTime,
  setEndTime,
  startTime,
  disabled,
}) => {
  const endTimeConf = createDatePickerConf(endTime, 30, moment().endOf("day"));

  const handleOnChange = (date) => {
    if (date < moment()) {
      return;
    }
    if (date <= startTime) {
      return;
    }

    setEndTime(date);
  };

  return (
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
