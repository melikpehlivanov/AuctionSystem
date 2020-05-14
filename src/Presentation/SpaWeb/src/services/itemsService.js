import api from "../helpers/api";

const itemsApiPath = "/items";

const getHottestUpcomingItems = () => {
  const tommorowDate = new Date();
  tommorowDate.setDate(tommorowDate.getDate() + 1);

  const dateAfterTenDays = new Date();
  dateAfterTenDays.setDate(dateAfterTenDays.getDate() + 10);

  const params = {
    startingPrice: 5000,
    startTime: tommorowDate.toUTCString(),
    endTime: dateAfterTenDays.toUTCString(),
  };
  return api.get(itemsApiPath, { params }).then((response) => response);
};

const getLiveItems = () => {
  const params = {
    getLiveItems: true,
  };
  return api.get(itemsApiPath, { params }).then((response) => response);
};

export default {
  getHottestUpcomingItems,
  getLiveItems,
};
