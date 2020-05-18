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

let authTokenRequest;

// This function makes a call to get the auth token
// or it returns the same promise as an in-progress call to get the auth token
function getAuthToken() {
  if (!authTokenRequest) {
    const tokens = JSON.parse(localStorage.getItem("user"));
    authTokenRequest = api
      .post(refreshTokenUrl, tokens)
      .then((tokenRefreshResponse) => tokenRefreshResponse);
    authTokenRequest.then(resetAuthTokenRequest, resetAuthTokenRequest);
  }

  return authTokenRequest;
}

function resetAuthTokenRequest() {
  authTokenRequest = null;
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

    if (error.response.status === 401 && !error.config._retry) {
      return getAuthToken().then((response) => {
        localStorage.setItem("user", JSON.stringify(response.data.data));
        error.response.config.__isRetryRequest = true;
        return api(error.response.config);
      });
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

export default api;
