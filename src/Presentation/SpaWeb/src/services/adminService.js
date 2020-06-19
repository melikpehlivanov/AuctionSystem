import api from "../utils/helpers/api";

const getUsers = (query) => {
  return api
    .get(process.env.REACT_APP_API_ADMINISTRATION_ENDPOINT, { params: query })
    .then((response) => response);
};

const addToRole = (email, role) => {
  return api
    .post(process.env.REACT_APP_API_ADMINISTRATION_ENDPOINT, {
      email,
      role,
    })
    .then((response) => response);
};

const removeFromRole = (email, role) => {
  return api
    .delete(process.env.REACT_APP_API_ADMINISTRATION_ENDPOINT, {
      data: { email, role },
    })
    .then((response) => response);
};

export default {
  getUsers,
  addToRole,
  removeFromRole,
};
