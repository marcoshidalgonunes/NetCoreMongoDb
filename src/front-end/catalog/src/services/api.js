import axios from "axios";

class Api {
  constructor() {
    this.apiClient = axios.create({
      baseURL: '/api',
      headers: {
        'Content-type': 'application/json'
      }
    });
  }
}

export default Api;