import React from "react";
import { Link } from "react-router-dom";

import "../Pictures/PictureContainer.css";

export const PictureContainer = ({ pictures, itemSlug }) => {
  const pictureClassNameRetriever = (index) => {
    if (index === 1) {
      return "primary";
    }

    return "secondary";
  };

  return (
    <div className="picture-container">
      {pictures.slice(0, 3).map((picture, index) => {
        return (
          <Link
            key={index}
            to={itemSlug}
            className={`${pictureClassNameRetriever(index)}-picture-container`}
          >
            <img
              alt="primary"
              src={picture.url}
              className={`card-img-top card-img-listing ${pictureClassNameRetriever(
                index
              )}-picture`}
            />
          </Link>
        );
      })}
    </div>
  );
};
