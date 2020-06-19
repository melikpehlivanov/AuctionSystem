import api from "../utils/helpers/api";

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
  return api
    .get(process.env.REACT_APP_API_ITEMS_ENDPOINT, { params })
    .then((response) => response);
};

const getLiveItems = () => {
  const params = {
    getLiveItems: true,
    pageSize: 10,
  };
  return api
    .get(process.env.REACT_APP_API_ITEMS_ENDPOINT, { params })
    .then((response) => {
      return {
        ...response,
        data: {
          ...response.data,
          data: response.data.data.map((item) => ({
            ...item,
            pictures:
              item.pictures.length === 0
                ? [
                    {
                      id: process.env.REACT_APP_DEFAULT_PICTURE_ID,
                      url: process.env.REACT_APP_DEFAULT_PICTURE_URL,
                    },
                  ]
                : item.pictures,
          })),
        },
      };
    });
};

const getItems = (query) => {
  return api
    .get(process.env.REACT_APP_API_ITEMS_ENDPOINT, { params: query })
    .then((response) => {
      return {
        ...response,
        data: {
          ...response.data,
          data: response.data.data.map((item) => ({
            ...item,
            pictures:
              item.pictures.length === 0
                ? [
                    {
                      id: process.env.REACT_APP_DEFAULT_PICTURE_ID,
                      url: process.env.REACT_APP_DEFAULT_PICTURE_URL,
                    },
                  ]
                : item.pictures,
          })),
        },
      };
    });
};

const getItemById = (id) => {
  return api
    .get(`${process.env.REACT_APP_API_ITEMS_ENDPOINT}/${id}`)
    .then((response) => {
      const item = response.data.data;
      return {
        ...response,
        data: {
          ...response.data,
          data: {
            ...item,
            pictures:
              item.pictures.length === 0
                ? [
                    {
                      id: process.env.REACT_APP_DEFAULT_PICTURE_ID,
                      url: process.env.REACT_APP_DEFAULT_PICTURE_URL,
                    },
                  ]
                : item.pictures,
          },
        },
      };
    });
};

const createItem = (body) => {
  return api
    .post(process.env.REACT_APP_API_ITEMS_ENDPOINT, body)
    .then((response) => response);
};

const editItem = (id, data) => {
  return api
    .put(`${process.env.REACT_APP_API_ITEMS_ENDPOINT}/${id}`, data)
    .then((response) => response);
};

const deleteItem = (id) => {
  return api
    .delete(`${process.env.REACT_APP_API_ITEMS_ENDPOINT}/${id}`)
    .then((response) => response);
};

export default {
  getHottestUpcomingItems,
  getLiveItems,
  getItems,
  getItemById,
  createItem,
  editItem,
  deleteItem,
};
