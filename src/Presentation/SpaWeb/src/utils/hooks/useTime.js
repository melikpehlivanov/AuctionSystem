import { useState, useEffect } from "react";
import moment from "moment";

export const useTime = (interval = 60000) => {
  const [currentTime, setCurrentTime] = useState(moment().toDate());

  useEffect(() => {
    const timer = setInterval(() => {
      setCurrentTime(moment().toDate());
    }, interval);

    return () => clearInterval(timer);
  }, [currentTime, interval]);

  return { currentTime };
};
