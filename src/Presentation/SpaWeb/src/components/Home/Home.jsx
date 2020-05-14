import React, { Fragment } from "react";
import { Header } from "./Header/Header";
import { HottestItems } from "./HottestItems/HottestItems";
import { LiveItems } from "./LiveItems/LiveItems";

export const Home = () => {
  return (
    <Fragment>
      <Header />
      <HottestItems />
      <LiveItems />
    </Fragment>
  );
};
