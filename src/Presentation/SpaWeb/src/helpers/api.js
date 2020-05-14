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
      toast.error(errorResponse.data.error);
      return Promise.reject(error);
    }
  }
);

export default Axios;
