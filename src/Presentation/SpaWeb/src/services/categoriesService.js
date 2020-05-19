import api from "../utils/helpers/api";

const getAll = () => {
  return api.get("/categories").then((response) => response);
};

export default {
  getAll,
};
