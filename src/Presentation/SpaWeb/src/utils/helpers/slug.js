import slugify from "react-slugify";

export const itemDetailsSlug = (title, id) => {
  return `/items/${slugify(title)}/${id}`;
};

export const itemEditSlug = (title, id) => {
  return `/items/edit/${slugify(title)}/${id}`;
};
