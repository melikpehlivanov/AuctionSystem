import React, { useState, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faServer } from "@fortawesome/free-solid-svg-icons";
import { history } from "../..";
import "./Error.css";

export const NetworkError = () => {
  const [secondsCounter, setSecondsCounter] = useState(60);

  useEffect(() => {
    const interval =
      secondsCounter >= 0 &&
      setInterval(() => setSecondsCounter(secondsCounter - 1), 1000);

    if (secondsCounter === 0) {
      history.push(history.location.state);
    }
    return () => clearInterval(interval);
  }, [secondsCounter]);
  return (
    <div className="error-box">
      <FontAwesomeIcon className="fa-4x fa-spin mb-4" icon={faServer} />
      <h5>
        We're sorry for the inconvenience but unfortunately our server is down.
        Please try again later!
      </h5>
      <h4>
        Auto retry in <i>{secondsCounter}</i> seconds...
      </h4>
    </div>
  );
};
