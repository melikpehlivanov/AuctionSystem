import React from "react";
import { useForm } from "react-hook-form";
import { Form, Button } from "react-bootstrap";
import { history } from "../../index";
import { toast } from "react-toastify";
import { useAuth } from "../../utils/helpers/authHook";

export const Login = () => {
  const { register, handleSubmit, errors, formState } = useForm({
    mode: "onblur",
  });

  const { touched } = formState;
  const auth = useAuth();

  const onSubmit = (data) => {
    auth.signIn(data).then(() => {
      history.push("/");
      toast.success("You've logged in successfully.");
    });
  };

  const hasError = (touched, errors) => {
    if (touched && !errors) {
      return false;
    } else if (touched && errors) {
      return true;
    }
  };

  return (
    <Form noValidate onSubmit={handleSubmit(onSubmit)}>
      <Form.Group controlId="formBasicEmail">
        <Form.Label>Email address</Form.Label>
        <Form.Control
          name="email"
          type="email"
          placeholder="Enter email"
          ref={register({
            required: "Email field is required",
            pattern: {
              value: /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/,
              message: "Please enter valid email address",
            },
          })}
          isValid={hasError(touched.email, errors.email) === false ?? true}
          isInvalid={hasError(touched.email, errors.email)}
        />
        {errors.email && (
          <Form.Control.Feedback type="invalid">
            {errors.email.message}
          </Form.Control.Feedback>
        )}
      </Form.Group>

      <Form.Group controlId="formBasicPassword">
        <Form.Label>Password</Form.Label>
        <Form.Control
          name="password"
          type="password"
          placeholder="Password"
          ref={register({
            required: "Password field is required",
            minLength: {
              value: 6,
              message: "Password should be between 6 and 100 characters long",
            },
            maxLength: {
              value: 100,
              message: "Password should be between 6 and 100 characters long",
            },
          })}
          isValid={
            hasError(touched.password, errors.password) === false ?? true
          }
          isInvalid={hasError(touched.password, errors.password)}
        />
        {errors.password && (
          <Form.Control.Feedback type="invalid">
            {errors.password.message}
          </Form.Control.Feedback>
        )}
      </Form.Group>

      <Button variant="primary" type="submit">
        Login
      </Button>
    </Form>
  );
};
