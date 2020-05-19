import slugify from "react-slugify";

export const itemDetailsSlug = (title, id) => {
  return `/items/${slugify(title)}/${id}`;
};

export const bidSlug = (itemTitle, itemId) => {
  return `/bids/${slugify(itemTitle)}/${itemId}`;
};
