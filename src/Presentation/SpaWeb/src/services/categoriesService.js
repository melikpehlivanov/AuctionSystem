import api from "../utils/helpers/api";

const getAll = () => {
  return api
    .get(process.env.REACT_APP_API_CATEGORIES_ENDPOINT)
    .then((response) => response);
};

export default {
  getAll,
};
