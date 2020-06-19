import React, { useState, Fragment } from "react";
import {
  Spinner,
  Col,
  Card,
  Button,
  OverlayTrigger,
  Tooltip,
} from "react-bootstrap";
import { useCounter } from "../../../utils/hooks/useCounter";
import { useAuth } from "../../../utils/hooks/authHook";
import { UserActionsContainer } from "./UserActionsContainer";
import { Bid } from "../../Bid/Bid";
import { toast } from "react-toastify";
import { history } from "../../..";

export const SideBar = (props) => {
  const { counter, currentUtcTime } = useCounter(props);
  const { user } = useAuth();
  const [showBidding, setShowBidding] = useState(false);

  const handleShowBidding = () => {
    if (user) {
      setShowBidding(true);
      return;
    }

    toast.error(
      "Bidding is available only for signed in users. Please sign in!"
    );
    history.push("/sign-in", history.location.pathname);
  };

  const Counter = () => {
    return counter ? (
      <p style={{ color: "red" }}>
        {counter.days().toFixed()}d {counter.hours()}h {counter.minutes()}m{" "}
        {counter.seconds()}s
      </p>
    ) : (
      <Spinner className="float-right" animation="border" />
    );
  };

  const CounterContainer = () => {
    return props.endTime > currentUtcTime &&
      props.startTime < currentUtcTime ? (
      <div className="d-flex w-100 justify-content-end">
        <div>Remaining time:&nbsp;</div>
        <div>
          <Counter />
        </div>
      </div>
    ) : props.startTime > currentUtcTime ? (
      <div className="d-flex w-100 justify-content-end">
        <div>Time until start:&nbsp;</div>
        <div>
          <Counter />
        </div>
      </div>
    ) : (
      <span className="w-100 w-100 justify-content-end">
        <span style={{ color: "red" }}>Auction ended</span>
      </span>
    );
  };

  return (
    <Fragment>
      {!showBidding ? (
        props.item ? (
          <Col md={4}>
            <div className="pt-2">
              <CounterContainer />
            </div>
            <div className="pt-3 pb-5">
              <Card border="dark">
                <Card.Body>
                  <Card.Text>
                    Est. Price: &euro; {props.item?.startingPrice} -{" "}
                    {props.item.startingPrice * 10}
                  </Card.Text>
                  <div className="m-2">
                    {props.startTime < currentUtcTime &&
                    props.endTime > currentUtcTime ? (
                      <Button
                        variant="primary"
                        size="lg"
                        className="btn-block"
                        onClick={handleShowBidding}
                      >
                        <span className="d-inline d-md-none d-lg-inline">
                          GO LIVE
                        </span>
                        BIDDING
                      </Button>
                    ) : (
                      <OverlayTrigger
                        overlay={
                          <Tooltip id="tooltip-disabled">
                            Live bidding is disabled until auction starts!
                          </Tooltip>
                        }
                      >
                        <span className="btn-block">
                          <Button
                            variant="primary"
                            size="lg"
                            disabled
                            className="btn-block"
                            style={{ pointerEvents: "none" }}
                          >
                            <span className="d-none d-lg-inline">GO LIVE</span>
                            BIDDING
                          </Button>
                        </span>
                      </OverlayTrigger>
                    )}
                  </div>
                </Card.Body>
              </Card>
            </div>
            <UserActionsContainer userId={user?.id} item={props.item} />
          </Col>
        ) : (
          ""
        )
      ) : (
        <Fragment>
          <Bid
            itemId={props.item.id}
            startingPrice={props.item.startingPrice}
            minPriceIncrease={props.item.minIncrease}
            CounterContainer={CounterContainer}
          />
        </Fragment>
      )}
    </Fragment>
  );
};
