import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";

export const NotFound = () => {
  return (
    <div className="error-box">
      <FontAwesomeIcon className="fa-4x fa-spin mb-4" icon={faSearch} />
      <h1 className="error-heading">404</h1>
      <h5>The page you were looking for could not be found</h5>
      <small className="text-muted">
        ...either that, or our server is on fire...
      </small>
      <Link className="btn btn-outline-primary mt-3" to="/">
        Home
      </Link>
    </div>
  );
};
