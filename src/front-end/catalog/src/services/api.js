import axios from "axios";

class Api {
  constructor() {
    this.apiClient = axios.create({
      baseURL: 'http://catalog/api',
      headers: {
        'Content-type': 'application/json'
      }
    });
  }
}

export default Api;