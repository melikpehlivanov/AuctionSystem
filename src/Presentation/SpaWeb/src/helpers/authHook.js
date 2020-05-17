import React, { useState, useEffect, useContext, createContext } from "react";
import api from "./api";

const registerUserApiPath = "/identity/register";
const loginUserApiPath = "/identity/login";
const localStorageUser = "user";

const authContext = createContext();

export function ProvideAuth({ children }) {
  const auth = useProvideAuth();

  return <authContext.Provider value={auth}>{children}</authContext.Provider>;
}

export const useAuth = () => {
  return useContext(authContext);
};

function useProvideAuth() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    setUser(getCurrentUser());
  }, []);

  const getCurrentUser = () => {
    return JSON.parse(localStorage.getItem(localStorageUser));
  };

  const signIn = (body) => {
    return api.post(loginUserApiPath, body).then((response) => {
      if (response.status === 200) {
        setUser(response.data.data);
        localStorage.setItem(
          localStorageUser,
          JSON.stringify(response.data.data)
        );
      }

      return response.data;
    });
  };

  const signUp = (body) => {
    return api.post(registerUserApiPath, body).then((response) => response);
  };

  const signOut = () => {
    localStorage.removeItem(localStorageUser);
    setUser(null);
  };

  return {
    user,
    signIn,
    signUp,
    signOut,
  };
}