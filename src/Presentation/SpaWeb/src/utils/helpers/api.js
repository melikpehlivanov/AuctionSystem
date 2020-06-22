import Axios from "axios";
import { toast } from "react-toastify";
import { history } from "../..";
import { setUserInLocalStorage } from "./localStorage";

const api = Axios.create({
  baseURL: process.env.REACT_APP_API_URL,
  withCredentials: true,
});
let authTokenRequest;

// This function makes a call to get the auth token
// or it returns the same promise as an in-progress call to get the auth token
function getAuthToken() {
  if (!authTokenRequest) {
    authTokenRequest = api
      .post(process.env.REACT_APP_API_REFRESH_TOKENS_ENDPOINT, {})
      .then((tokenRefreshResponse) => tokenRefreshResponse);
    authTokenRequest.then(resetAuthTokenRequest, resetAuthTokenRequest);
  }

  return authTokenRequest;
}

function resetAuthTokenRequest() {
  authTokenRequest = null;
}

export const setupAxiosInterceptor = (signOut) => {
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      // network error
      const errorResponse = error.response;
      if (!errorResponse) {
        history.push("/error/network", history.location.pathname);
        return Promise.reject(error);
      }

      if (
        error.config.url === process.env.REACT_APP_API_REFRESH_TOKENS_ENDPOINT
      ) {
        signOut();
        history.push("/sign-in");
        toast.error("Please sign in");
        return Promise.reject(error);
      }

      if (error.response.status === 401 && !error.config._retry) {
        return getAuthToken().then((response) => {
          setUserInLocalStorage(response);
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
        } else if (errorResponse.status === 404) {
          toast.error(
            "Oops, looks like the item you are searching for actually does not exist."
          );
          history.push("/");
          return Promise.reject(error);
        }

        return Promise.reject(error);
      }
    }
  );
};

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
