import React from "react";
import { Button, Spinner } from "react-bootstrap";

export const BidHigherButton = ({
  isLoading,
  handleOnClick,
  amount,
  percentage,
}) => {
  const bid = percentage <= 0 ? amount : amount + (amount * percentage) / 100;
  const suggestedBid = parseFloat(bid).toFixed(2);

  return !isLoading ? (
    <Button
      onClick={() => {
        handleOnClick(suggestedBid);
      }}
      block
      variant="primary"
      size="lg"
    >
      Bid {process.env.REACT_APP_CURRENCY_SIGN}
      {suggestedBid ? suggestedBid : "0.00"}
    </Button>
  ) : (
    <Button block variant="primary" size="lg" disabled>
      <Spinner
        as="span"
        animation="grow"
        size="sm"
        role="status"
        aria-hidden="true"
      />
      Creating bid...
    </Button>
  );
};
