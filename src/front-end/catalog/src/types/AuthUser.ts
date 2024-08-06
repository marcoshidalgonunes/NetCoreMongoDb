import { User } from "oidc-client-ts"

class AuthUser { 
    static getUser(): User | null {
        const authority = process.env.REACT_APP_AUTHORITY;
        const clientId = process.env.REACT_APP_CLIENT_ID;
        const oidcStorage = localStorage.getItem(`oidc.user:${authority}:${clientId}`);
        if (!oidcStorage) {
            return null;
        }
    
        const user: User = User.fromStorageString(oidcStorage);
        
        return user.expired ? null : user;
    }    
}

export default AuthUser;