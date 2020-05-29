import React, { Fragment } from "react";
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

export const SideBar = (props) => {
  const { counter, currentUtcTime } = useCounter(props);
  const { user } = useAuth();

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

  return (
    <Fragment>
      {props?.item ? (
        <Col md={4}>
          <h1 className="d-none d-md-block" style={{ wordBreak: "break-word" }}>
            {props.item.title}
          </h1>
          <div className="pt-2">
            {props.endTime > currentUtcTime &&
            props.startTime < currentUtcTime ? (
              <div className="d-flex float-right">
                Remaining time:&nbsp;
                <Counter />
              </div>
            ) : props.startTime > currentUtcTime ? (
              <div className="d-flex float-right">
                Time until start:&nbsp;
                <Counter />
              </div>
            ) : (
              <span className="float-right">
                <span style={{ color: "red" }}>Auction ended</span>
              </span>
            )}
          </div>
          <div className="pt-5 pb-5">
            <Card border="dark">
              <Card.Body>
                <Card.Text>
                  Est. Price: &euro; {props.item?.startingPrice} -{" "}
                  {props.item?.startingPrice * 10}
                </Card.Text>
                <div className="m-2">
                  {props.startTime < currentUtcTime &&
                  props.endTime > currentUtcTime ? (
                    <Button variant="primary" size="lg" className="btn-block">
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
          <UserActionsContainer userId={user?.id} item={props?.item} />
        </Col>
      ) : (
        ""
      )}
    </Fragment>
  );
};
