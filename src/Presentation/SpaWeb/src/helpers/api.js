import axios from "axios";
import { toast } from "react-toastify";
import { history } from "..";

const Axios = axios.create({
  baseURL: "https://localhost:5001/api",
});

export let networkErrorMessage = "";

Axios.interceptors.response.use(
  (response) => response,
  (error) => {
    // network error
    const errorResponse = error.response;
    if (!errorResponse) {
      history.push("/error/network");
      return Promise.reject(error);
    }

    if (errorResponse.status) {
      // validation errors
      if (errorResponse.status === 400 && errorResponse.data.errors) {
        Object.keys(errorResponse.data.errors).forEach(function (key) {
          errorResponse.data.errors[key].forEach(function (value) {
            toast.error(value, {
              autoClose: 10000,
            });
          });
        });

        return Promise.reject(error);
      }

      toast.error(errorResponse.data.error);
      return Promise.reject(error);
    }
  }
);

export default Axios;
