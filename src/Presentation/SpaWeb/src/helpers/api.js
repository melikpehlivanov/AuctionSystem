import Axios from "axios";
import { toast } from "react-toastify";
import { history } from "..";

const api = Axios.create({
  baseURL: "https://localhost:5001/api",
});

const refreshTokenUrl = "/identity/refresh";

function getAccessToken() {
  return JSON.parse(localStorage.getItem("user"))?.token;
}

api.interceptors.request.use((request) => {
  request.headers["Authorization"] = `Bearer ${getAccessToken()}`;
  return request;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    // network error
    const errorResponse = error.response;
    if (!errorResponse) {
      history.push("/error/network");
      return Promise.reject(error);
    }

    let originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      // if the error is 401 and hasn't already been retried
      return refreshToken(error, originalRequest);
    }

    if (errorResponse.status) {
      // validation errors
      if (errorResponse.status === 400 && errorResponse.data.errors) {
        return handleValidationErrors(errorResponse, error);
      } else if (errorResponse.status === 400 && errorResponse.data.error) {
        toast.error(errorResponse.data.error);
        return Promise.reject(error);
      }

      return Promise.reject(error);
    }
  }
);

function handleValidationErrors(errorResponse, error) {
  Object.keys(errorResponse.data.errors).forEach(function (key) {
    errorResponse.data.errors[key].forEach(function (value) {
      toast.error(value, {
        autoClose: 8000,
      });
    });
  });
  return Promise.reject(error);
}

function refreshToken(error, request) {
  request._retry = true; // now it can be retried
  const tokens = JSON.parse(localStorage.getItem("user"));

  if (tokens) {
    return api
      .post(refreshTokenUrl, tokens)
      .then((tokenRefreshResponse) => {
        localStorage.setItem(
          "user",
          JSON.stringify(tokenRefreshResponse.data.data)
        );

        request.headers["Authorization"] = "Bearer " + getAccessToken(); // new header new token
        return api(request);
      })
      .catch((error) => {
        return Promise.reject(error);
      });
  }

  toast.error("Please login");
  history.push("/sign-in");
  return Promise.reject(error);
}

export default api;
