const user = "user";

export const setUserInLocalStorage = (response) => {
  const data = response.data.data;

  const token = data.token;
  const jwtParams = JSON.parse(atob(token.split(".")[1]));
  const id = jwtParams.id;
  const isAdmin = jwtParams.role?.toLowerCase().includes("admin");

  data.id = id;
  data.isAdmin = isAdmin ?? false;
  localStorage.setItem(user, JSON.stringify(data));

  return data;
};

export const removeUserFromLocalStorage = () => {
  localStorage.removeItem(user);
};

export const getUserFromLocalStorage = () => {
  return JSON.parse(localStorage.getItem(user));
};
