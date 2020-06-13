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

import React, { useState, useEffect, useContext, createContext } from "react";
import api from "../helpers/api";
import {
  setUserInLocalStorage,
  removeUserFromLocalStorage,
} from "../helpers/localStorage";

const registerUserApiPath = "/identity/register";
const loginUserApiPath = "/identity/login";
const localStorageUser = "user";

const authContext = createContext();

export function ProvideAuth({ children }) {
  const auth = useProvideAuth();

  return (
    <authContext.Provider value={auth}>
      {!auth.isLoading ? children : null}
    </authContext.Provider>
  );
}

export const useAuth = () => {
  return useContext(authContext);
};

function useProvideAuth() {
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (user === null) {
      setUser(getCurrentUser());
      setIsLoading(false);
    }
  }, [user, isLoading]);

  const getCurrentUser = () => {
    return JSON.parse(localStorage.getItem(localStorageUser));
  };

  const signIn = (body) => {
    return api.post(loginUserApiPath, body).then((response) => {
      if (response.status === 200) {
        const data = setUserInLocalStorage(response);
        setUser(data);
      }

      return response.data;
    });
  };

  const signUp = (body) => {
    return api.post(registerUserApiPath, body).then((response) => response);
  };

  const signOut = () => {
    removeUserFromLocalStorage();
    setUser(null);
  };

  return {
    user,
    isLoading,
    signIn,
    signUp,
    signOut,
  };
}
