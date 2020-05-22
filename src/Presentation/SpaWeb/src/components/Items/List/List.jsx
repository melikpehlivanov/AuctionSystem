import React, { Fragment, useState } from "react";
import { Container, Row, Col, Dropdown } from "react-bootstrap";
import "react-input-range/lib/css/index.css";
import "./List.css";
import useItemsSearch from "../../../utils/hooks/useItemsSearch";
import useDebounce from "../../../utils/hooks/useDebounce";
import { ItemsContainer } from "./ItemsContainer";
import { Search } from "./Search";

export const List = () => {
  const [state, setState] = useState({
    title: null,
    getLiveItems: false,
    minPrice: null,
    maxPrice: null,
    startTime: null,
    endTime: null,
  });

  const [pageNumber, setPageNumber] = useState(1);

  const query = useDebounce(state, 500);
  const { items, totalItemsCount, hasMore, loading, error } = useItemsSearch(
    query,
    pageNumber
  );

  return (
    <Fragment>
      <Container>
        <Row>
          <Col lg={12}>
            <div className="generic-header">
              <div>
                <h4>All Items</h4>
                <p>Total Listing Found: {totalItemsCount}</p>
              </div>
              <div className="generic-header-toolbar" role="toolbar">
                <Dropdown>
                  <Dropdown.Toggle
                    className="sort-btn"
                    variant="light"
                    id="dropdown-basic"
                  >
                    sort by
                  </Dropdown.Toggle>

                  <Dropdown.Menu>
                    <Dropdown.Item href="#/action-1">TODO</Dropdown.Item>
                  </Dropdown.Menu>
                </Dropdown>
              </div>
            </div>
          </Col>
          <Col lg={12}>
            <Row>
              <Search state={state} setState={setState} />
              <Col lg={8}>
                <ItemsContainer
                  items={items}
                  totalItemsCount={totalItemsCount}
                  hasMore={hasMore}
                  loading={loading}
                  error={error}
                  setPageNumber={setPageNumber}
                />
              </Col>
            </Row>
          </Col>
        </Row>
      </Container>
    </Fragment>
  );
};
