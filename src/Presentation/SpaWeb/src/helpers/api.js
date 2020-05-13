import axios from "axios";
import { toast } from "react-toastify";

const Axios = axios.create({
  baseURL: "https://localhost:5001/api",
  headers: {
    "Content-type": "application/json",
  },
});

Axios.interceptors.response.use(
  (response) => response,
  (error) => {
    const errorResponse = error.response;
    if (errorResponse.status) {
      toast.error(errorResponse.data.errors);
      return Promise.reject(error);
    }
  }
);

export default Axios;
