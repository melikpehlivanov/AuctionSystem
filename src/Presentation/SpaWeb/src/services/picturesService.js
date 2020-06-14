const createImageObj = (pictures) => {
  let data = [];
  pictures.forEach((element) => {
    data.push({
      original: element.url,
      thumbnail: element.url,
    });
  });

  return data;
};

export default createImageObj;
