import axios from "axios";
import config from "../config.json"

export default axios.create({
  baseURL: config.UrlApi,
  headers: {
    "Content-type": "application/json"
  }
});