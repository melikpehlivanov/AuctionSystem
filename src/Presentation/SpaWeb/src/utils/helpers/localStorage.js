//######################## WARNING ############# JWT isn't safely stored atm.
/* TODO:
To safely store the token, I have to use a combination of 2 cookies as described below:

A JWT token has the following structure: header.payload.signature

In general a useful information is present in the payload such as the user roles 
(that can be used to adapt/hide parts of the UI). So it's important to keep that part available to the Javascript code.

Once the authentication flow finished and JWT token created in the backend, the idea is to:
  1. Store the header.payload part in a SameSite Secure Cookie (so availbale only through https and still availble to the JS code)
  2. Store the signature part in a SameSite Secure HttpOnly Cookie
  3. Implement a middleware in your backend to resconstruct the JWT token from those 2 cookies and put it in the header: 
  Authorization: Bearer your_token
*/

const user = "user";

export const setUserInLocalStorage = (response) => {
  const data = response.data.data;

  const token = data.token;
  const jwtParams = JSON.parse(atob(token.split(".")[1]));
  const id = jwtParams.id;
  const isAdmin = jwtParams.role?.toLowerCase().includes("admin");

  let dataToStore = {};
  dataToStore.id = id;
  dataToStore.token = token;
  dataToStore.isAdmin = isAdmin ?? false;

  localStorage.setItem(user, JSON.stringify(dataToStore));
  return dataToStore;
};

export const removeUserFromLocalStorage = () => {
  localStorage.removeItem(user);
};

export const getUserFromLocalStorage = () => {
  return JSON.parse(localStorage.getItem(user));
};
