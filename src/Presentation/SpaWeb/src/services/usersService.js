import api from "../helpers/api";

const registerApiPath = "/identity/register";

const registerUser = (body) => {
  return api.post(registerApiPath, body).then((response) => response);
};

export default {
  registerUser,
};
