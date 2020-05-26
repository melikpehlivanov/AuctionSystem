import api from "../utils/helpers/api";

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
    pageSize: 10,
  };
  return api.get(itemsApiPath, { params }).then((response) => response);
};

const getItems = (query) => {
  return api.get(itemsApiPath, { params: query }).then((response) => response);
};

const getItemById = (id) => {
  return api.get(`${itemsApiPath}/${id}`).then((response) => response);
};

const createItem = (body) => {
  return api.post(itemsApiPath, body).then((response) => response);
};

export default {
  getHottestUpcomingItems,
  getLiveItems,
  getItems,
  getItemById,
  createItem,
};
