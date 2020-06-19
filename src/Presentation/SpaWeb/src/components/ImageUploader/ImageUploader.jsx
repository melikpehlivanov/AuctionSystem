import React, { useState, useEffect } from "react";
import FlipMove from "react-flip-move";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faServer } from "@fortawesome/free-solid-svg-icons";
import "./ImageUploader.css";

const styles = {
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  flexWrap: "wrap",
  width: "100%",
};

const ERROR = {
  NOT_SUPPORTED_EXTENSION: "NOT_SUPPORTED_EXTENSION",
  FILESIZE_TOO_LARGE: "FILESIZE_TOO_LARGE",
};

export const ImageUploader = (props) => {
  const [pictures, setPictures] = useState([]);
  const [files, setFiles] = useState([]);
  const [fileErrors, setFileErrors] = useState([]);
  const [
    shouldAcceptInitialPictures,
    setShouldAcceptInitialPictures,
  ] = useState(true);
  let inputElement = "";

  const { onChange, images, onRemove } = props;

  useEffect(() => {
    if (shouldAcceptInitialPictures && images) {
      setPictures(images);
      setShouldAcceptInitialPictures(false);
    }
  }, [shouldAcceptInitialPictures, images]);

  useEffect(() => {
    onChange(files, pictures);
  }, [pictures, files, onChange]);

  const hasExtension = (fileName) => {
    const pattern =
      "(" + props.imgExtensions.join("|").replace(/\./g, "\\.") + ")$";
    return new RegExp(pattern, "i").test(fileName);
  };

  const onDropFile = (e) => {
    const eventFiles = e.target.files;
    const allFilePromises = [];
    const errors = [];

    for (let i = 0; i < eventFiles.length; i++) {
      let file = eventFiles[i];
      let fileError = {
        name: file.name,
      };
      if (!hasExtension(file.name)) {
        fileError = Object.assign(fileError, {
          type: ERROR.NOT_SUPPORTED_EXTENSION,
        });
        errors.push(fileError);
        continue;
      }
      if (file.size > props.maxFileSize) {
        fileError = Object.assign(fileError, {
          type: ERROR.FILESIZE_TOO_LARGE,
        });
        errors.push(fileError);
        continue;
      }

      allFilePromises.push(readFile(file));
    }

    setFileErrors(errors);

    const { singleImage } = props;

    Promise.all(allFilePromises).then((newFilesData) => {
      const dataURLs = singleImage ? [] : pictures.slice();
      const newFiles = singleImage ? [] : files.slice();

      newFilesData.forEach((newFileData) => {
        dataURLs.push(newFileData.dataURL);
        newFiles.push(newFileData.file);
      });

      setPictures(dataURLs);
      setFiles(newFiles);
    });
  };

  const onUploadClick = (e) => {
    e.target.value = null;
  };

  const readFile = (file) => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = function (e) {
        let dataURL = e.target.result;
        dataURL = dataURL.replace(";base64", `;name=${file.name};base64`);
        resolve({ file, dataURL });
      };

      reader.readAsDataURL(file);
    });
  };

  const removeImage = (picture) => {
    const removeIndex = pictures.findIndex((e) => e === picture);
    if (picture.id) {
      onRemove((prev) => [...prev, picture.id]);
    } else
      setFiles((oldFiles) =>
        oldFiles.filter((e, index) => index !== removeIndex)
      );

    setPictures((oldPictures) =>
      oldPictures.filter((e, index) => index !== removeIndex)
    );
    onChange(files, pictures);
  };

  const renderErrors = () => {
    return fileErrors.map((fileError, index) => {
      return (
        <div
          className={"errorMessage " + props.errorClass}
          key={index}
          style={props.errorStyle}
        >
          * {fileError.name}{" "}
          {fileError.type === ERROR.FILESIZE_TOO_LARGE
            ? props.fileSizeError
            : props.fileTypeError}
        </div>
      );
    });
  };

  const renderIcon = () => {
    if (props.withIcon) {
      return <FontAwesomeIcon icon={faServer} className="uploadIcon" />;
    }
  };

  const renderLabel = () => {
    if (props.withLabel) {
      return (
        <p className={props.labelClass} style={props.labelStyles}>
          {props.label}
        </p>
      );
    }
  };

  const renderPreview = () => {
    return (
      <div className="uploadPicturesWrapper">
        <FlipMove enterAnimation="fade" leaveAnimation="fade" style={styles}>
          {renderPreviewPictures()}
        </FlipMove>
      </div>
    );
  };

  const renderPreviewPictures = () => {
    return pictures.map((picture, index) => {
      return (
        <div key={index} className="uploadPictureContainer">
          <div className="deleteImage" onClick={() => removeImage(picture)}>
            X
          </div>
          <img
            src={picture.url ? picture.url : picture}
            className="uploadPicture"
            alt="preview"
          />
        </div>
      );
    });
  };

  const triggerFileUpload = () => {
    inputElement.click();
  };

  return (
    <div className="fileUploader">
      <div className="fileContainer">
        {renderIcon()}
        {renderLabel()}
        <div className="errorsContainer">{renderErrors()}</div>
        <button
          type={props.buttonType}
          className={"chooseFileButton " + props.buttonClassName}
          style={props.buttonStyles}
          onClick={triggerFileUpload}
        >
          {props.buttonText}
        </button>
        <input
          type="file"
          ref={(input) => (inputElement = input)}
          name={props.name}
          multiple={!props.singleImage}
          onChange={onDropFile}
          onClick={onUploadClick}
          accept={props.accept}
        />
        {props.withPreview ? renderPreview() : null}
      </div>
    </div>
  );
};

ImageUploader.defaultProps = {
  className: "",
  fileContainerStyle: {},
  buttonClassName: "",
  buttonStyles: {},
  withPreview: true,
  accept: "image/*",
  name: "",
  withIcon: true,
  buttonText: "Choose images",
  buttonType: "button",
  withLabel: true,
  label: "Max file size: 5mb, accepted: jpg|jpeg|gif|png",
  labelStyles: {},
  labelClass: "",
  imgExtensions: [".jpg", ".jpeg", ".gif", ".png"],
  maxFileSize: 5242880,
  fileSizeError: " file size is too big",
  fileTypeError: " is not a supported file extension",
  errorClass: "",
  style: {},
  errorStyle: {},
  singleImage: false,
  onChange: () => {},
  defaultImages: [],
};
