import api from "../utils/helpers/api";

const bidsApiPath = "/bids";

const getHighestBid = (itemId) => {
  return api
    .get(`${bidsApiPath}/getHighestBid/${itemId}`)
    .then((response) => response);
};

export default { getHighestBid };
