import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faServer } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";

import "./Error.css";

export const NetworkError = () => {
  return (
    <div className="error-box">
      <FontAwesomeIcon className="fa-4x fa-spin mb-4" icon={faServer} />
      <h1 className="error-heading">500</h1>
      <h5>
        We're sorry for the inconvenience but unfortunately our server is down.
        Please try again later!
      </h5>
    </div>
  );
};
