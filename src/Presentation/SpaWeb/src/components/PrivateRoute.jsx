import React, { Fragment } from "react";
import { Route } from "react-router-dom";
import { useAuth } from "../utils/hooks/authHook";
import { toast } from "react-toastify";
import { history } from "..";

export const PrivateRoute = ({ component: Component, ...rest }) => {
  const { user } = useAuth();

  return (
    <Route
      {...rest}
      render={(props) =>
        user ? (
          <Component {...props} />
        ) : (
          <Fragment>
            {history.push("/sign-in")}
            {toast.warning("Please sign in!")}
          </Fragment>
        )
      }
    />
  );
};
