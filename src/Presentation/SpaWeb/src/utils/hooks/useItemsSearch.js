import { useEffect, useState } from "react";
import itemsService from "../../services/itemsService";

const useItemsSearch = (query, pageNumber) => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);
  const [items, setItems] = useState([]);
  const [totalItemsCount, setTotalItemsCount] = useState(0);
  const [hasMore, setHasMore] = useState(false);

  useEffect(() => {
    setItems([]);
  }, [query]);

  useEffect(() => {
    setLoading(true);
    setError(false);

    query.pageNumber = pageNumber;
    query.pageSize = 10;

    itemsService
      .getItems(query)
      .then((result) => {
        setItems((previtems) => {
          return [...previtems, ...result.data.data];
        });
        setTotalItemsCount(result.data.totalDataCount);
        setHasMore(result.data.data.length > 0);
        setLoading(false);
      })
      .catch(() => setError(true));
  }, [query, pageNumber]);

  return { loading, error, items, totalItemsCount, hasMore };
};

export default useItemsSearch;
