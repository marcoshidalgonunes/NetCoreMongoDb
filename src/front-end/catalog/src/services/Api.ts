import axios, {AxiosInstance } from "axios"
import { User } from "oidc-client-ts"

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
            const user = this.getUser();
            const token = user?.access_token;
            config.headers.Authorization = `Bearer ${token}`
            return config
        });        
    }

    getUser() {
        const authority = process.env.REACT_APP_AUTHORITY;
        const clientId = process.env.REACT_APP_CLIENT_ID;
        const oidcStorage = localStorage.getItem(`oidc.user:${authority}:${clientId}`);
        if (!oidcStorage) {
            return null;
        }
    
        return User.fromStorageString(oidcStorage);
    }
}

export default Api;