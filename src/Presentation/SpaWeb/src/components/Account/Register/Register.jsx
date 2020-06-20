import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { Form, Button } from "react-bootstrap";
import { useAuth } from "../../../utils/hooks/authHook";
import { ConfirmAccount } from "../ConfirmAccount";

export const Register = () => {
  const { register, handleSubmit, errors, watch, formState } = useForm({
    mode: "onblur",
  });
  const [showConfirmationPage, setShowConfirmationPage] = useState(false);

  const { touched } = formState;
  const password = watch("password");
  const email = watch("email");
  const auth = useAuth();

  const onSubmit = (data) => {
    auth.signUp(data).then((response) => {
      setShowConfirmationPage(true);
    });
  };

  const hasError = (touched, errors) => {
    if (touched && !errors) {
      return false;
    } else if (touched && errors) {
      return true;
    }
  };

  return !showConfirmationPage ? (
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

      <Form.Group controlId="formBasicFullName">
        <Form.Label>Full Name</Form.Label>
        <Form.Control
          name="fullName"
          type="text"
          placeholder="Enter your full name"
          ref={register({
            required: "Full name field is required",
            maxLength: {
              value: 50,
              message: "Full name exceeds the maximum limit of 50 characters",
            },
          })}
          isValid={
            hasError(touched.fullName, errors.fullName) === false ?? true
          }
          isInvalid={hasError(touched.fullName, errors.fullName)}
        />
        {errors.fullName && (
          <Form.Control.Feedback type="invalid">
            {errors.fullName.message}
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

      <Form.Group controlId="formBasicConfirmPassword">
        <Form.Label>Confirm password</Form.Label>
        <Form.Control
          name="confirmPassword"
          type="password"
          placeholder="Retype your password"
          ref={register({
            required: "Confirm password field is required",
            minLength: {
              value: 6,
              message: "Password should be between 6 and 100 characters long",
            },
            maxLength: {
              value: 100,
              message: "Password should be between 6 and 100 characters long",
            },
            validate: (value) =>
              value === password || "Passwords do not match!",
          })}
          isValid={
            hasError(touched.confirmPassword, errors.confirmPassword) ===
              false ?? true
          }
          isInvalid={hasError(touched.confirmPassword, errors.confirmPassword)}
        />
        {errors.confirmPassword && (
          <Form.Control.Feedback type="invalid">
            {errors.confirmPassword.message}
          </Form.Control.Feedback>
        )}
      </Form.Group>

      <Button variant="primary" type="submit">
        Submit
      </Button>
    </Form>
  ) : (
    <ConfirmAccount auth={auth} email={email} />
  );
};
