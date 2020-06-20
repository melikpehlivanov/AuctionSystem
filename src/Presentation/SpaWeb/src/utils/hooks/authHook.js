import React, { useState, useEffect, useContext, createContext } from "react";
import api, { setupAxiosInterceptor } from "../helpers/api";
import {
  setUserInLocalStorage,
  removeUserFromLocalStorage,
  getUserFromLocalStorage,
} from "../helpers/localStorage";

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
      setUser(getUserFromLocalStorage());
      setIsLoading(false);
    }
  }, [user, isLoading]);

  useEffect(() => {
    setupAxiosInterceptor(signOut);
  }, []);

  const signIn = (body) => {
    return api
      .post(process.env.REACT_APP_API_LOGIN_ENDPOINT, body)
      .then((response) => {
        if (response.status === 200) {
          const data = setUserInLocalStorage(response);
          setUser(data);
        }

        return response.data;
      });
  };

  const signUp = (body) => {
    return api
      .post(process.env.REACT_APP_API_REGISTER_ENDPOINT, body)
      .then((response) => response);
  };

  const confirmAccount = (body) => {
    return api
      .post(process.env.REACT_APP_API_CONFIRM_ACCOUNT_ENDPOINT, body)
      .then((response) => response);
  };

  const signOut = () => {
    api.post(process.env.REACT_APP_API_LOGOUT_ENDPOINT, {}).then(() => {
      removeUserFromLocalStorage();
      setUser(null);
    });
  };

  return {
    user,
    isLoading,
    signIn,
    signUp,
    confirmAccount,
    signOut,
  };
}
