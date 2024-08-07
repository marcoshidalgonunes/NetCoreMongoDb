import axios, {AxiosInstance } from "axios"
import AuthUser from "./AuthUser";

class Api {
    api: AxiosInstance;

    constructor() { 
        this.api = axios.create({
            baseURL: process.env.REACT_APP_API_URL,
            headers: {
                'Content-type': 'application/json'
            }
        })

        this.api.interceptors.request.use(config => {
            const user = AuthUser.getUser();
            const token = user?.access_token;
            config.headers.Authorization = `Bearer ${token}`
            return config
        });        
    }

}

export default Api;