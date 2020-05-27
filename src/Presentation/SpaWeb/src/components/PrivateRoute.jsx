import React, { Fragment } from "react";
import { Redirect, Route } from "react-router-dom";
import { useAuth } from "../utils/hooks/authHook";
import { toast } from "react-toastify";

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
            <Redirect to="/sign-in" />
            {toast.warning("Please sign in!")}
          </Fragment>
        )
      }
    />
  );
};
