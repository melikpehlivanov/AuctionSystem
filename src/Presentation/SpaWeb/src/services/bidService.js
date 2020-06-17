import api from "../utils/helpers/api";

const getHighestBid = (itemId) => {
  return api
    .get(`${process.env.REACT_APP_API_BIDS_ENDPOINT}/getHighestBid/${itemId}`)
    .then((response) => response);
};

export default { getHighestBid };
