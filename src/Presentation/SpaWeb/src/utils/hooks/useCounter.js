import { useState, useEffect } from "react";
import moment from "moment";

export function useCounter(props) {
  const [counter, setCounter] = useState(null);
  const currentTime = moment().toISOString("dd/mm/yyyy HH:mm");
  const currentUtcTime = moment().utc().toISOString("dd/mm/yyyy HH:mm");

  useEffect(() => {
    let duration;
    if (props.startTime > currentTime) {
      // Calculate remaining time until auction start
      let eventTime = moment.utc(props.startTime).local();
      duration = moment.duration(eventTime.diff(currentTime));
    } else if (props.startTime < currentTime && props.endTime > currentTime) {
      let eventTime = moment.utc(moment(props.endTime)).local();
      duration = moment.duration(eventTime.diff(currentTime));
    }

    const interval = 1000;

    let timeout;
    if (!counter) {
      timeout = setTimeout(() => {
        setCounter(duration);
      }, 500);
    }

    const timer =
      counter &&
      setInterval(() => {
        if (counter.milliseconds() < 0 && duration < 0) {
          return clearInterval(timer);
        }
        duration = moment.duration(duration - interval, "milliseconds");
        setCounter(duration);
      }, interval);
    return () => {
      clearInterval(timer);
      clearTimeout(timeout);
    };
  }, [counter, props, currentTime]);

  return {
    counter,
    currentUtcTime,
  };
}
